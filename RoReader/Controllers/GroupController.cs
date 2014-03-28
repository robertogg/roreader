using System;
using System.Web.Mvc;
using RoReader.AzureStorage.Core;
using RoReader.Infrastructure.Core;
using RoReader.Models;

namespace RoReader.Controllers
{
    public class GroupController : Controller
    {
        private readonly IRepository<GroupFeed> _groupFeedRepository;
        private readonly string _userId;

        public GroupController(IRepository<GroupFeed> groupFeedRepository, IClaimsInfo claimsInfo)
        {
            _groupFeedRepository = groupFeedRepository;

            _userId= claimsInfo.GetCurrentUserId();
        }
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string partitionKey, string rowKey)
        {
            var group=new GroupFeed();
            if (!string.IsNullOrEmpty(partitionKey) && !string.IsNullOrEmpty(rowKey))
            { 
                group = _groupFeedRepository.GetByPartitionKeyAndRowKey(partitionKey, rowKey);
            }
            return View(group);
        }

        [HttpPost]
        public ActionResult Edit(GroupFeed group)
        {
            if (string.IsNullOrEmpty(group.RowKey))
            {
                group.RowKey = Guid.NewGuid().ToString();
            }

            var groupFeed = new GroupFeed
            {
                PartitionKey = _userId,
                RowKey = group.RowKey,
                Title = group.Title,
                Description = group.Description,
                Timestamp = new DateTimeOffset(DateTime.Now)
            };

            _groupFeedRepository.InsertOrReplace(groupFeed);
            return RedirectToAction("Index");
        }

        public JsonResult GetGroups()
        {
            var groups = _groupFeedRepository.GetByPartitionKey(_userId);

            return Json(groups,JsonRequestBehavior.AllowGet);
        }
	}
}