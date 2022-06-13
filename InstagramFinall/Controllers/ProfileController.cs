using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramFinall.Models;
using InstagramFinall.Globals;
using System.Data.Entity;
using System.IO;
using InstagramFinall.ViewModels;

namespace Instagram.Controllers
{
    public class ProfileController : Controller // , ILikes
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Profile

        public ActionResult ViewProfile()
        {
            int loggedInUserID = GlobalUserId.Get();
            int likesCounter;
            int dislikesCounter;
            List<PostsAndUser> fullPosts = new List<PostsAndUser>();
            User loggedUser = db.Users.Single(x => x.UserId == loggedInUserID);
            List<Post> allPostsFromDatabase = db.Posts.ToList();
            List<Comment> commentsForSpecificPost = new List<Comment>();
            List<Like> allLikesFromDatabase = db.Likes.ToList();
            List<Comment> allCommentsFromDatabase = db.Comments.ToList();
            Like isLiked = new Like();
            foreach (Post post in allPostsFromDatabase)
            {
                likesCounter = 0;
                dislikesCounter = 0;
                //to make sure only logged in user posts goes to view
                if (post.OwnerId == loggedInUserID)
                {
                    //to check if the logged in user already made like or dislike
                    foreach (var like in allLikesFromDatabase)
                    {
                        if (like.PostId == post.PostId && like.UserId == loggedInUserID)
                            isLiked = like;
                    }
                    //to count how many likes and dis likes
                    foreach (var like in allLikesFromDatabase)
                    {

                        if (like.PostId == post.PostId && like.NuLikes == true)
                        { likesCounter++; }
                        else if (like.PostId == post.PostId && like.NuDislikes == true)
                        {
                            dislikesCounter++;
                        }
                    }
                    //to make sure each comment stays with his post
                    foreach (var comment in allCommentsFromDatabase)
                    {
                        if (comment.PostId == post.PostId)
                            commentsForSpecificPost.Add(comment);
                    }
                    PostsAndUser finallPost = new PostsAndUser
                    {
                        Post = post,
                        User = loggedUser,
                        likesCounter = likesCounter,
                        DisLikesCounter = dislikesCounter,
                        Comments = commentsForSpecificPost,
                        Like = isLiked
                    };
                    //to make sure no dublicates
                    if (!fullPosts.Contains(finallPost))
                        fullPosts.Add(finallPost);


                }
            }
            if (loggedUser == null)
            {
                return HttpNotFound();
            }

            if (fullPosts.Count == 0)
            {
                //to check if profile empty or not
                PostsAndUser finallPost = new PostsAndUser
                {

                    User = loggedUser

                };
                fullPosts.Add(finallPost);
            }
            return View(fullPosts);
        }

        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            //load old profile data
            var oldUser = db.Users.Single(x => x.UserId == id);
            if (oldUser == null)
                return HttpNotFound();
            else
            {
                return View(oldUser);
            }

        }
        [HttpPost]
        public ActionResult EditProfile(User editedUser)
        {
            if (ModelState.IsValid)
            {
                var oldUser = db.Users.Single(x => x.UserId == editedUser.UserId);
                //change to new data
                oldUser.FirstName = editedUser.FirstName;
                oldUser.LastName = editedUser.LastName;
                oldUser.Password = editedUser.Password;
                oldUser.Mobile = editedUser.Mobile;
                oldUser.Email = editedUser.Email;

                //to cheack if he changed picture or stayed with the same
                if (editedUser.ImagePath != null)
                {
                    try
                    {
                        string FileName = Path.GetFileNameWithoutExtension(editedUser.ImageFile.FileName);
                        string Extension = Path.GetExtension(editedUser.ImageFile.FileName);
                        FileName = FileName + DateTime.Now.ToString("yymmssffff") + Extension;
                        editedUser.ImagePath = "~/Image/" + FileName;
                        oldUser.ImagePath = editedUser.ImagePath;
                        FileName = Path.Combine(Server.MapPath("~/Image/"), FileName);
                        editedUser.ImageFile.SaveAs(FileName);
                    }
                    catch
                    {

                    }
                }

                db.Entry(oldUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewProfile");
            }
            return View();

        }
        public ActionResult Like(int id)
        {

            Like newLike = new Like();
            newLike = newLike.CreateLike(id);
            db.Likes.Add(newLike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts", "Home");
        }

        public ActionResult Undo(int id)
        {
            Like like = db.Likes.Find(id);
            db.Likes.Remove(like);
            db.SaveChanges();
            return RedirectToAction("ViewAllPosts", "Home");
        }

        public ActionResult Dislike(int id)
        {

            Like newDislike = new Like();
            newDislike = newDislike.DisLike(id);
            db.Likes.Add(newDislike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts", "Home");
        }
        public ActionResult ViewOtherProfile(int id)
        {
            int loggedInUserID = GlobalUserId.Get();
            bool isFriends = false;
            int likesCounter;
            int dislikesCounter;
            PostsAndUserAndFriendshipAndComments page = new PostsAndUserAndFriendshipAndComments();
            List<Post> allPostsFromDatabase = db.Posts.ToList();
            page.User = db.Users.Single(x => x.UserId == id);
            List<FriendRequest> allFriendReqsFromDatabase = db.FriendRequests.ToList();
            foreach (FriendRequest f in allFriendReqsFromDatabase)
            {
                //to make sure of already friends or not
                if (f.SenderId == loggedInUserID && f.RecieverId == id)
                {
                    page.NewFriendRequest = f;
                    if (f.Status == true)
                        isFriends = true;
                }
            }
            if (isFriends)
            {
                Like isLiked = new Like();
                List<Like> allLikesFromDatabase = db.Likes.ToList();
                List<Comment> allCommentsFromDatabase = db.Comments.ToList();
                List<Comment> mycomments = new List<Comment>();
                foreach (var post in allPostsFromDatabase)
                {
                    //to make sure only can see posts for that user
                    if (post.OwnerId == page.User.UserId)
                    {
                        likesCounter = 0;
                        dislikesCounter = 0;
                        //to know if you already liked or not

                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostId == post.PostId && like.UserId == loggedInUserID)
                                isLiked = like;
                        }
                        //to count how many likes or dislikes
                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostId == post.PostId && like.NuLikes == true)
                            { likesCounter++; }
                            else if (like.PostId == post.PostId && like.NuDislikes == true)
                            {
                                dislikesCounter++;
                            }
                        }
                        //to make sure each comment you can see in its post
                        foreach (var comment in allCommentsFromDatabase)
                        {
                            if (comment.PostId == post.PostId)
                                mycomments.Add(comment);
                        }

                        FullHomePage h = new FullHomePage
                        {
                            User = page.User,
                            Posts = post,
                            Like = isLiked,
                            LikesCounter = likesCounter,
                            DisLikesCounter = dislikesCounter,
                            Comments = mycomments
                        };
                        page.Pages.Add(h);
                    }
                }
            }


            return View(page);
        }
        /*
         void ILikes.Like(int id){

            like newLike = new like();
            newLike = newLike.Like(id);
            db.likes.Add(newLike);
            db.SaveChanges();

        } 
         */

    }
}


