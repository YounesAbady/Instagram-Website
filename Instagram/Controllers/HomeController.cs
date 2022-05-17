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
            List<FullHomePage> fullHomePosts = new List<FullHomePage>();
            List<post> allPostsFromDatabase = db.posts.ToList();
            List<FriendRequest> allFriendsFromDatabase = db.FriendRequests.ToList();
            List<comment> commentsForSpecificPost = new List<comment>();
            foreach (var friendRequest in allFriendsFromDatabase) {
                if (friendRequest.Status==true&friendRequest.SenderID== GlobalUserID.get())
                {
                    List<like> allLikesFromDatabase = db.likes.ToList();
                    like isLiked = new like();
                    user postOwner = new user();
                    List<comment> allCommentsFromDatabase = db.comments.ToList();
                    postOwner = db.users.Find(friendRequest.RecieverID);
                    foreach (var post in allPostsFromDatabase) {
                        if (post.OwnerID == postOwner.UserID)
                        {
                            likesCounter = 0;
                            dislikesCounter = 0;

                            //to check if the logged in user already made like or dislike
                            foreach (var like in allLikesFromDatabase)
                            {
                                if (like.PostID == post.PostID&&like.UserID== GlobalUserID.get())
                                    isLiked = like;
                            }

                            //to count how many likes and dis likes
                            foreach (var like in allLikesFromDatabase)
                            {
                                if (like.PostID == post.PostID && like.NuLikes == true)
                                { likesCounter++; }
                                else if (like.PostID == post.PostID && like.NuDislikes == true)
                                {
                                    dislikesCounter++;
                                }
                            }
                            //to make sure each comment stays with his post
                            foreach (var comment in allCommentsFromDatabase)
                            {
                                if (comment.PostID == post.PostID)
                                    commentsForSpecificPost.Add(comment);
                            }

                            FullHomePage newHomePost = new FullHomePage
                            {
                                User = postOwner,
                                posts = post,
                                Like = isLiked,
                                LikesCounter = likesCounter,
                                DislikesCounter = dislikesCounter,
                                comments = commentsForSpecificPost
                            };
                            //to make sure no dublicates
                            if (!fullHomePosts.Contains(newHomePost))
                                 { fullHomePosts.Add(newHomePost); }
                        }
                    }

                }
            }
            
            return View(fullHomePosts);
        }

        public ActionResult Like (int id)
        {

            like newLike = new like();
            newLike = newLike.Like(id);
            db.likes.Add(newLike);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Undo(int id)
        {
            like removedLikeOrDislike = db.likes.Find(id);
            db.likes.Remove(removedLikeOrDislike);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Dislike(int id)
        {

            like newDislike = new like();
            newDislike = newDislike.disLike(id);
            db.likes.Add(newDislike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts");
        }
        [HttpGet]
        public ActionResult makeComment(int id)
        {
            comment newComment = new comment();
            newComment.PostID = id;

            return View(newComment);
        }
        [HttpPost]
        public ActionResult makeComment(comment comment)
        {
            comment newComment = new comment();
            if (ModelState.IsValid)
            {
                newComment = newComment.newComment(comment);
                db.comments.Add(newComment);
                db.SaveChanges();
            }

            return RedirectToAction("ViewAllPosts");
        }

    }
}