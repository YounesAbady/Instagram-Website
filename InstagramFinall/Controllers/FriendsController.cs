using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Models;
using InstagramFinall.Globals;
using InstagramFinall.ViewModels;
using System.Data.Entity;
namespace InstagramFinall.Controllers
{
    public class FriendsController : Controller
    {
        ProjectDataBaseEntities Db = new ProjectDataBaseEntities();

        // GET: Friends
        public ActionResult ViewRequests()
        {
            int LoggedInUserId = GlobalUserId.Get();
            List<SenderAndReceiverData> SenderAndReceiver = new List<SenderAndReceiverData>();
            List<FriendRequest> AllFriendRequestsFromDatabase = Db.FriendRequests.ToList();
            foreach (var FriendRequest in AllFriendRequestsFromDatabase)
            {
                if (FriendRequest.RecieverId == LoggedInUserId)
                {
                    var Follower = new User();
                    Follower = Db.Users.Single(x => x.UserId == FriendRequest.SenderId);
                    SenderAndReceiverData Instance = new SenderAndReceiverData
                    {
                        Sender = Follower,
                        Request = FriendRequest
                    };
                    SenderAndReceiver.Add(Instance);
                }
            }
            return View(SenderAndReceiver);
        }
        public ActionResult Accept(int id)
        {
            var EditedRequest = Db.FriendRequests.Find(id);
            EditedRequest.Status = true;
            Db.Entry(EditedRequest).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
        public ActionResult Reject(int id)
        {
            var EditedRequest = Db.FriendRequests.Find(id);
            EditedRequest.Status = false;
            Db.Entry(EditedRequest).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }

        public ActionResult AddFriend(int id)
        {
            int LoggedInUserID = GlobalUserId.Get();
            FriendRequest NewRequest = new FriendRequest
            {
                SenderId = LoggedInUserID,
                RecieverId = id,

            };
            Db.FriendRequests.Add(NewRequest);
            Db.SaveChanges();
            return RedirectToAction("ViewAllPosts", "Home");
        }
    }
}