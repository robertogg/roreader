using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure.Storage.Queue;
using RoReader.AzureStorage.Core;
using RoReader.Hubs;
using RoReader.Infrastructure.Core;
using RoReader.Models;

namespace RoReader.Controllers
{
    public class FeedController : Controller
    {
        private readonly IRepository<Feed> _feedRepository;
        private readonly IRepository<GroupFeed> _groupFeedRepository;
        private readonly IAzureQueue _azureQueue;
        private readonly string _userId;

        public FeedController(IRepository<Feed> feedRepository, 
            IRepository<GroupFeed> groupFeedRepository, 
            IClaimsInfo claimsInfo,
            IAzureQueue azureQueue)
        {
            _feedRepository = feedRepository;
            _groupFeedRepository = groupFeedRepository;
            _azureQueue = azureQueue;

            _userId = claimsInfo.GetCurrentUserId();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            var feed = new Feed();
           
            if (!string.IsNullOrEmpty(partitionKey) && !string.IsNullOrEmpty(rowKey))
            {
                feed = _feedRepository.GetByPartitionKeyAndRowKey(partitionKey, rowKey);
            }
            var feedViewModel = new FeedViewModel
            {
                Feed = feed,
                GroupFeeds = _groupFeedRepository.GetByPartitionKey(_userId)
            };

            return View(feedViewModel);
        }

        [HttpPost]
        public ActionResult Edit(Feed feed)
        {
            if (string.IsNullOrEmpty(feed.RowKey))
            {
                feed.RowKey = Guid.NewGuid().ToString();
            }

            var feedData = new Feed
            {
                PartitionKey = _userId,
                RowKey = feed.RowKey,
                Title = feed.Title,
                Description = feed.Description,
                Timestamp = new DateTimeOffset(DateTime.Now),
                FeedGroup = feed.FeedGroup,
                Url = feed.Url
            };

            _feedRepository.InsertOrReplace(feedData);
            _azureQueue.AddMessage(new CloudQueueMessage(string.Format("{{'partitionKey':'{0}','rowKey':'{1}'}}", feedData.PartitionKey, feedData.RowKey)));
            return RedirectToAction("Index");
        }

        public JsonResult GetFeeds()
        {
            var groups = _groupFeedRepository.GetByPartitionKey(_userId);
            var feeds = from feed in _feedRepository.GetByPartitionKey(_userId)
                        join dataGroup in groups on feed.FeedGroup equals dataGroup.RowKey
                        select new Feed
                        {
                            PartitionKey = feed.PartitionKey,
                            RowKey = feed.RowKey,
                            Title = feed.Title,
                            FeedGroup = dataGroup.Title,
                            Description = feed.Description,
                            Url = feed.Url,
                            ETag = feed.ETag,
                            Timestamp = feed.Timestamp
                        };
            return Json(feeds, JsonRequestBehavior.AllowGet);
        }
	}
}