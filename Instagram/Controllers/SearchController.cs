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
            int id = GlobalUserID.loggedInUserID;
            ViewData["ID"] = id;
            return View(db.users.Where(x => x.Fname.StartsWith(Searching) || x.Lname.StartsWith(Searching) || Searching == null||x.email.StartsWith(Searching)).ToList());
        }
        /*
        public ActionResult ViewOtherProfile(int id) {
            bool isFriebds = false;
            PostsAndUserAndFriendshipAndComments fullPost = new PostsAndUserAndFriendshipAndComments();
            user user = db.users.Single(x => x.UserID == id);
            fullPost.user = user;
            List <FriendRequest>allFriendReqsFromDatabase= db.FriendRequests.ToList();
            foreach (FriendRequest f in allFriendReqsFromDatabase)
            {
                if (f.SenderID == GlobalUserID.loggedInUserID && f.RecieverID == id)
                {
                    fullPost.f = f;
                    if (f.Status == true)
                        isFriebds = true;
                }
            }
            fullPost.LoggedinUser = GlobalUserID.loggedInUserID;
            if (isFriebds)
                fullPost.posts = db.posts.ToList();
    

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(fullPost);

        }
        */
        public ActionResult ViewOtherProfile(int id) {
            bool isFriends = false;
            int likesCounter;
            int dislikesCounter;
            PostsAndUserAndFriendshipAndComments page = new PostsAndUserAndFriendshipAndComments();
            List<post> allPostsFromDatabase = db.posts.ToList();
            page.user = db.users.Single(x => x.UserID == id);
            List<FriendRequest> allFriendReqsFromDatabase = db.FriendRequests.ToList();
            foreach (FriendRequest f in allFriendReqsFromDatabase)
            {
                if (f.SenderID == GlobalUserID.loggedInUserID && f.RecieverID == id)
                {
                    page.f = f;
                    if (f.Status == true)
                        isFriends = true;
                }
            }
            if(isFriends)
            {
                like like = new like();
                List<like> likes = db.likes.ToList();
                List<comment> allCommentsFromDatabase = db.comments.ToList();
                List<comment> mycomments = new List<comment>();
                foreach (var item in allPostsFromDatabase)
                {
                    if (item.OwnerID == page.user.UserID)
                    {
                        likesCounter = 0;
                        dislikesCounter = 0;
                        foreach (var y in likes)
                        {
                            if (y.PostID == item.PostID && y.UserID == GlobalUserID.loggedInUserID)
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
                        foreach (var y in allCommentsFromDatabase)
                        {
                            if (y.PostID == item.PostID)
                                mycomments.Add(y);
                        }

                        FullHomePage h = new FullHomePage
                        {
                            User = page.user,
                            posts = item,
                            Like = like,
                            LikesCounter = likesCounter,
                            DislikesCounter = dislikesCounter,
                            comments = mycomments
                        };
                        page.pages.Add(h);
                    }
                }
            }


            return View(page);
        }

        public ActionResult AddFriend(int id) {
            FriendRequest newRequest = new FriendRequest {
                SenderID = GlobalUserID.loggedInUserID,
                RecieverID = id,

            };
            db.FriendRequests.Add(newRequest);
            db.SaveChanges();
            return RedirectToAction("Search");
        }
        public ActionResult Like(int id)
        {

            like newLike = new like
            {
                PostID = id,
                NuLikes = true,
                NuDislikes = false,
                UserID = GlobalUserID.loggedInUserID
            };

            db.likes.Add(newLike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts","Home");
        }

        public ActionResult Undo(int id)
        {
            like like = db.likes.Find(id);
            db.likes.Remove(like);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts", "Home");
        }

        public ActionResult Dislike(int id)
        {

            like newDislike = new like();
            newDislike.PostID = id;
            newDislike.NuLikes = false;
            newDislike.NuDislikes = true;
            newDislike.UserID = GlobalUserID.loggedInUserID;
            db.likes.Add(newDislike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts", "Home");
        }
    }
}