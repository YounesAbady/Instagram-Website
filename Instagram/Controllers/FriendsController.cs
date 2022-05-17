using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
using Instagram.ViewModels;
using System.Data.Entity;
namespace Instagram.Controllers
{
    public class FriendsController : Controller
    {  ProjectDataBaseEntities db = new ProjectDataBaseEntities();

        // GET: Friends
        public ActionResult ViewRequests()
        {
            int loggedInUserID = GlobalUserID.get();
            List<SenderAndRecieverData> SenderAndReciever = new List<SenderAndRecieverData>();
            List<FriendRequest> allFriendRequestsFromDatabase = db.FriendRequests.ToList();
            foreach (var friendRequest in allFriendRequestsFromDatabase)
            {
                if (friendRequest.RecieverID == loggedInUserID)
                {
                    var Follower=new user();
                    Follower = db.users.Single(x => x.UserID == friendRequest.SenderID); 
                    SenderAndRecieverData s = new SenderAndRecieverData
                    {
                        Sender = Follower,
                        request = friendRequest
                    };
                SenderAndReciever.Add(s);
                }
            }
            return View(SenderAndReciever);
        }
        public ActionResult Accept(int id)
        {
            var EditedRequest = db.FriendRequests.Find(id);
            EditedRequest.Status = true;
            db.Entry(EditedRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
        public ActionResult Reject(int id)
        {
            var EditedRequest = db.FriendRequests.Find(id);
            EditedRequest.Status = false;
            db.Entry(EditedRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }

        public ActionResult AddFriend(int id)
        {
            int loggedInUserID = GlobalUserID.get();
            FriendRequest newRequest = new FriendRequest
            {
                SenderID = loggedInUserID,
                RecieverID = id,

            };
            db.FriendRequests.Add(newRequest);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts","Home");
        }
    }
}