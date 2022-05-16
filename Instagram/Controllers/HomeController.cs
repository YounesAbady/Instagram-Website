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
    public class HomeController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Home
        public ActionResult ViewAllPosts()
        {
            int likescounter ;
            int dislikescounter;
            List<FullHomePage> home = new List<FullHomePage>();
            List<post> posts = db.posts.ToList();
            List<FriendRequest> friends = db.FriendRequests.ToList();
            List<comment> mycomments = new List<comment>();
            foreach (var x in friends) {
                if (x.Status==true&&(x.RecieverID==GlobalUserID.globalUserID||x.SenderID==GlobalUserID.globalUserID))
                {
                    List<like> likes = db.likes.ToList();
                    like like = new like();
                    user user = new user();
                    List<comment> comnts = db.comments.ToList();
                    if (x.RecieverID == GlobalUserID.globalUserID) { user = db.users.Find(x.SenderID); }
                    else { user = db.users.Find(x.RecieverID); }
                    foreach (var item in posts) {
                        if (item.OwnerID == user.UserID)
                        {
                            likescounter = 0;
                            dislikescounter = 0;
                            foreach (var y in likes)
                            {
                                if (y.PostID == item.PostID&&y.UserID==GlobalUserID.globalUserID)
                                    like = y;
                            }
                            foreach (var y in likes)
                            {
                                if (y.PostID == item.PostID && y.NuLikes == true)
                                { likescounter++; }
                                else if (y.PostID == item.PostID && y.NuDislikes == true)
                                {
                                    dislikescounter++;
                                }
                            }
                            foreach (var y in comnts)
                            {
                                if (y.PostID == item.PostID)
                                    mycomments.Add(y);
                            }

                            FullHomePage h = new FullHomePage
                            {
                                User = user,
                                posts = item,
                                Like = like,
                                LikesCounter = likescounter,
                                DislikesCounter= dislikescounter,
                                comments=mycomments
                            };
                            if (!home.Contains(h))
                                 { home.Add(h); }
                        }
                    }

                }
            }
            
            return View(home);
        }

        public ActionResult Like (int id)
        {
            
            List<like> l = db.likes.ToList();
            like like = new like {
                PostID=id,
                NuLikes=true,
                NuDislikes=false,
                UserID=GlobalUserID.globalUserID
            };

                db.likes.Add(like);
                db.SaveChanges();
            
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Undo(int id)
        {
            like like = db.likes.Find(id);
            db.likes.Remove(like);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Dislike(int id)
        {

            List<like> l = db.likes.ToList();
            like like = new like();
            like.PostID = id;
            like.NuLikes = false;
            like.NuDislikes = true;
            like.UserID = GlobalUserID.globalUserID;
            db.likes.Add(like);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts");
        }

    }
}