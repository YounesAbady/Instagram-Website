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
            int loggedInUserId = GlobalUserId.Get();
            List<SenderAndReceiverData> senderAndReceiver = new List<SenderAndReceiverData>();
            List<FriendRequest> allFriendRequestsFromDatabase = Db.FriendRequests.ToList();
            foreach (var friendRequest in allFriendRequestsFromDatabase)
            {
                if (friendRequest.RecieverId == loggedInUserId)
                {
                    var follower = new User();
                    follower = Db.Users.Single(x => x.UserId == friendRequest.SenderId);
                    SenderAndReceiverData Instance = new SenderAndReceiverData
                    {
                        Sender = follower,
                        Request = friendRequest
                    };
                    senderAndReceiver.Add(Instance);
                }
            }
            return View(senderAndReceiver);
        }
        public ActionResult Accept(int id)
        {
            var editedRequest = Db.FriendRequests.Find(id);
            editedRequest.Status = true;
            Db.Entry(editedRequest).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
        public ActionResult Reject(int id)
        {
            var editedRequest = Db.FriendRequests.Find(id);
            editedRequest.Status = false;
            Db.Entry(editedRequest).State = EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }

        public ActionResult AddFriend(int id)
        {
            int loggedInUserID = GlobalUserId.Get();
            FriendRequest NewRequest = new FriendRequest
            {
                SenderId = loggedInUserID,
                RecieverId = id,

            };
            Db.FriendRequests.Add(NewRequest);
            Db.SaveChanges();
            return RedirectToAction("ViewAllPosts", "Home");
        }
    }
}