using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json.Linq;
using RoReader.AzureStorage;
using RoReader.AzureStorageTable;
using RoReader.Constants;
using RoReader.Hubs;
using RoReader.Infrastructure.Cache;
using RoReader.Models;
using Microsoft.AspNet.SignalR;

namespace RoReader.WorkerRole.Feeds
{
    public class WorkerRole : RoleEntryPoint
    {
        private AzureQueue _cloudQueue;
        
        public override void Run()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<PostHub>();
            var azureCache = new AzureCache();

            while (true)
            {
                var message=_cloudQueue.GetMessage();
                if (message != null)
                {
                    try
                    {
                        dynamic feedData = JObject.Parse(message.AsString);
                        var tableFeed = new Repository<Feed>();
                        var tablePost = new Repository<Post>();

                        var feed = tableFeed.GetByPartitionKeyAndRowKey(feedData.partitionKey.ToString(), feedData.rowKey.ToString());

                        var rssFeed = new QDFeedParser.HttpFeedFactory().CreateFeed(new Uri(feed.Url));
                        foreach (var data in rssFeed.Items)
                        {
                            try
                            {
                                var post = new Post
                                {
                                    FeedId = feedData.rowKey,
                                    PartitionKey = feedData.partitionKey,
                                    RowKey = Regex.Replace(data.Title, @"[^\w]", ""),
                                    Title = data.Title,
                                    Content = data.Content,
                                    Link = data.Link,
                                    Timestamp = new DateTimeOffset(DateTime.Now),
                                };
                                tablePost.InsertOrReplace(post);
                                azureCache.Put<Post>(CacheKeys.Post(post.PartitionKey,post.RowKey), post);
                            }
                            catch (Exception)
                            {
                                //Log and do something with this error
                            }
                        }
                        _cloudQueue.DeleteMessage(message);
                      
                        // The first time probably the stream is closed and you can receive a error. 
                        // If you like avoid this issue you can use a retry policy

                        context.Clients.All.refreshPost(feed.Title);
                    }
                    catch (Exception ex)
                    {
                        //Log and do something with this error
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public override bool OnStart()
        {
            _cloudQueue = new AzureQueue();
            GlobalHost.DependencyResolver.UseRedis(CloudConfigurationManager.GetSetting(Azure.RedisServer), 6379, CloudConfigurationManager.GetSetting(Azure.RedisPassword), "roreader");
            ServicePointManager.DefaultConnectionLimit = 12;
            return base.OnStart();
        }

        
    }
}
