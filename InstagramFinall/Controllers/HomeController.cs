using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Models;
using InstagramFinall.ViewModels;
using InstagramFinall.Globals;

namespace Instagram.Controllers
{
    public class HomeController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Home
        public ActionResult ViewAllPosts()
        {

            int likesCounter;
            int disLikesCounter;
            List<FullHomePage> fullHomePosts = new List<FullHomePage>();
            List<Post> allPostsFromDatabase = db.Posts.ToList();
            List<FriendRequest> allFriendsFromDatabase = db.FriendRequests.ToList();
            List<Comment> commentsForSpecificPost = new List<Comment>();
            foreach (var friendRequest in allFriendsFromDatabase)
            {
                //to make sure only users you follow comes up
                if (friendRequest.Status == true & friendRequest.SenderId == GlobalUserId.Get())
                {
                    List<Like> allLikesFromDatabase = db.Likes.ToList();
                    Like isLiked = new Like();
                    User postOwner = new User();
                    List<Comment> allCommentsFromDatabase = db.Comments.ToList();
                    postOwner = db.Users.Find(friendRequest.RecieverId);
                    foreach (var post in allPostsFromDatabase)
                    {
                        if (post.OwnerId == postOwner.UserId)
                        {
                            likesCounter = 0;
                            disLikesCounter = 0;

                            //to check if the logged in user already made like or dislike
                            foreach (var like in allLikesFromDatabase)
                            {
                                if (like.PostId == post.PostId && like.UserId == GlobalUserId.Get())
                                    isLiked = like;
                            }

                            //to count how many likes and dis likes
                            foreach (var like in allLikesFromDatabase)
                            {
                                if (like.PostId == post.PostId && like.NuLikes == true)
                                { likesCounter++; }
                                else if (like.PostId == post.PostId && like.NuDislikes == true)
                                {
                                    disLikesCounter++;
                                }
                            }
                            //to make sure each comment stays with his post
                            foreach (var comment in allCommentsFromDatabase)
                            {
                                if (comment.PostId == post.PostId)
                                    commentsForSpecificPost.Add(comment);
                            }

                            FullHomePage NewHomePost = new FullHomePage
                            {
                                User = postOwner,
                                Posts = post,
                                Like = isLiked,
                                LikesCounter = likesCounter,
                                DisLikesCounter = disLikesCounter,
                                Comments = commentsForSpecificPost
                            };
                            //to make sure no dublicates
                            if (!fullHomePosts.Contains(NewHomePost))
                            { fullHomePosts.Add(NewHomePost); }
                        }
                    }

                }
            }

            return View(fullHomePosts);
        }

        public ActionResult Like(int id)
        {

            Like newLike = new Like();
            newLike = newLike.CreateLike(id);
            db.Likes.Add(newLike);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Undo(int id)
        {
            Like removedLikeOrDisLike = db.Likes.Find(id);
            db.Likes.Remove(removedLikeOrDisLike);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }

        public ActionResult Dislike(int id)
        {

            Like newDisLike = new Like();
            newDisLike = newDisLike.DisLike(id);
            db.Likes.Add(newDisLike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts");
        }
        [HttpGet]
        public ActionResult MakeComment(int id)
        {
            Comment newComment = new Comment();
            newComment.PostId = id;

            return View(newComment);
        }
        [HttpPost]
        public ActionResult MakeComment(Comment comment)
        {
            Comment newComment = new Comment();
            if (ModelState.IsValid)
            {
                newComment = newComment.NewComment(comment);
                db.Comments.Add(newComment);
                db.SaveChanges();
            }

            return RedirectToAction("ViewAllPosts");
        }

    }
}