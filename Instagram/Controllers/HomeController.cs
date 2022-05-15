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
            int likesCounter ;
            int dislikesCounter;
            List<FullHomePage> home = new List<FullHomePage>();
            List<post> posts = db.posts.ToList();
            List<FriendRequest> friends = db.FriendRequests.ToList();
            List<comment> mycomments = new List<comment>();
            foreach (var x in friends) {
                if (x.Status==true&x.SenderID==GlobalUserID.globalUserID)
                {
                    List<like> likes = db.likes.ToList();
                    like like = new like();
                    user user = new user();
                    List<comment> comnts = db.comments.ToList();
                    user = db.users.Find(x.RecieverID);
                    foreach (var item in posts) {
                        if (item.OwnerID == user.UserID)
                        {
                            likesCounter = 0;
                            dislikesCounter = 0;
                            foreach (var y in likes)
                            {
                                if (y.PostID == item.PostID&&y.UserID==GlobalUserID.globalUserID)
                                    like = y;
                            }
                            foreach (var y in likes)
                            {
                                if (y.PostID == item.PostID && y.NuLikes == true)
                                { likesCounter++; }
                                else if (y.PostID == item.PostID && y.NuDislikes == true)
                                {
                                    dislikesCounter++;
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
                                LikesCounter = likesCounter,
                                DislikesCounter = dislikesCounter,
                                comments = mycomments
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
        [HttpGet]
        public ActionResult makeComment(int id)
        {
            comment c = new comment();
            c.PostID = id;

            return View(c);
        }
        [HttpPost]
        public ActionResult makeComment(comment comment)
        {
            comment myc = new comment();
            if (ModelState.IsValid)
            {
                myc.PostID = comment.PostID;
                myc.UserID = GlobalUserID.globalUserID;
                myc.data = comment.data;
                db.comments.Add(myc);
                db.SaveChanges();
            }

            return RedirectToAction("ViewAllPosts");
        }

    }
}