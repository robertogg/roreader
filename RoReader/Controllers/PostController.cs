using System.Linq;
using System.Web.Mvc;
using RoReader.AzureStorage.Core;
using RoReader.Constants;
using RoReader.Infrastructure.Core;
using RoReader.Models;

namespace RoReader.Controllers
{
    public class PostController : Controller
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<GroupFeed> _groupFeedRepository;
        private readonly IRepository<Feed> _feedRepository;
        private readonly IClaimsInfo _claimsInfo;
        private readonly IAzureCache _azureCache;
        private readonly string _userId;

        public PostController(IRepository<GroupFeed> groupFeedRepository, 
                              IRepository<Feed> feedRepository, 
                              IRepository<Post> postRepository, 
                              IClaimsInfo claimsInfo,
                              IAzureCache azureCache )
        {
            _postRepository = postRepository;
            _groupFeedRepository = groupFeedRepository;
            _feedRepository = feedRepository;
            _claimsInfo = claimsInfo;
            _azureCache = azureCache;

            _userId = _claimsInfo.GetCurrentUserId();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPost(string id)
        {
            var post = _azureCache.MakeCached<Post>(CacheKeys.Post(_userId, id), (data) => _postRepository.GetByPartitionKeyAndRowKey(_userId, id));
            return Json(post, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts()
        {
            var groupFeeds = _groupFeedRepository.GetByPartitionKey(_userId);
            var feeds = _feedRepository.GetByPartitionKey(_userId);
            var posts = _postRepository.GetByPartitionKey(_userId);

            var dataFeeds = from data in groupFeeds
                select new GroupInfo
                {
                    Title = data.Title,
                    FeedInfo =  from dataFeed in feeds
                            where dataFeed.FeedGroup == data.RowKey
                            select new FeedInfo
                            {
                                Title= dataFeed.Title,
                                PostInfo=from dataPost in posts
                                      where dataPost.FeedId==dataFeed.RowKey
                                      select dataPost
                            }
                };
            return Json(dataFeeds, JsonRequestBehavior.AllowGet);
        }
    }
}