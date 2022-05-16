using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.ViewModels;
using Instagram.Globals;
namespace Instagram.Controllers
{
    public class SearchController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Search

        public ActionResult Search(string Searching)
        {
            int id = GlobalUserID.globalUserID;
            ViewData["ID"] = id;
            return View(db.users.Where(x => x.Fname.StartsWith(Searching) || x.Lname.StartsWith(Searching) || Searching == null||x.email.StartsWith(Searching)).ToList());
        }
        public ActionResult ViewOtherProfile(int id) {
            PostsAndUserAndFriendshipAndComments p = new PostsAndUserAndFriendshipAndComments();
            user user = db.users.Single(x => x.UserID == id);
            p.u = user;
            p.p = db.posts.ToList();
            List < FriendRequest >friendRequs= db.FriendRequests.ToList();
            foreach (FriendRequest f in friendRequs)
            {
                if ((f.SenderID == GlobalUserID.globalUserID&&f.RecieverID==id)|| (f.RecieverID == GlobalUserID.globalUserID && f.SenderID == id))
                    p.f = f;
            }
            p.LoggedinUser = GlobalUserID.globalUserID;
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(p);

        }

        public ActionResult AddFriend(int id) {
            FriendRequest f = new FriendRequest {
                SenderID = GlobalUserID.globalUserID,
                RecieverID = id,

            };
            db.FriendRequests.Add(f);
            db.SaveChanges();
            return RedirectToAction("Search");
        }
    }
}