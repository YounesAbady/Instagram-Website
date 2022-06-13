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
            int loggedInUserId = GlobalUserId.Get();
            int likesCounter;
            int disLikesCounter;
            List<PostsAndUser> fullPosts = new List<PostsAndUser>();
            User loggedUser = db.Users.Single(x => x.UserId == loggedInUserId);
            List<Post> allPostsFromDatabase = db.Posts.ToList();
            List<Comment> commentsForSpecificPost = new List<Comment>();
            List<Like> allLikesFromDatabase = db.Likes.ToList();
            List<Comment> allCommentsFromDatabase = db.Comments.ToList();
            Like isLiked = new Like();
            foreach (Post post in allPostsFromDatabase)
            {
                likesCounter = 0;
                disLikesCounter = 0;
                //to make sure only logged in user posts goes to view
                if (post.OwnerId == loggedInUserId)
                {
                    //to check if the logged in user already made like or dislike
                    foreach (var like in allLikesFromDatabase)
                    {
                        if (like.PostId == post.PostId && like.UserId == loggedInUserId)
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
                    PostsAndUser FinallPost = new PostsAndUser
                    {
                        Post = post,
                        User = loggedUser,
                        likesCounter = likesCounter,
                        DisLikesCounter = disLikesCounter,
                        Comments = commentsForSpecificPost,
                        Like = isLiked
                    };
                    //to make sure no dublicates
                    if (!fullPosts.Contains(FinallPost))
                        fullPosts.Add(FinallPost);


                }
            }
            if (loggedUser == null)
            {
                return HttpNotFound();
            }

            if (fullPosts.Count == 0)
            {
                //to check if profile empty or not
                PostsAndUser FinallPost = new PostsAndUser
                {

                    User = loggedUser

                };
                fullPosts.Add(FinallPost);
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
                        string fileName = Path.GetFileNameWithoutExtension(editedUser.ImageFile.FileName);
                        string extension = Path.GetExtension(editedUser.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                        editedUser.ImagePath = "~/Image/" + fileName;
                        oldUser.ImagePath = editedUser.ImagePath;
                        fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        editedUser.ImageFile.SaveAs(fileName);
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

            Like newDisLike = new Like();
            newDisLike = newDisLike.DisLike(id);
            db.Likes.Add(newDisLike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts", "Home");
        }
        public ActionResult ViewOtherProfile(int id)
        {
            int loggedInUserId = GlobalUserId.Get();
            bool isFriends = false;
            int likesCounter;
            int disLikesCounter;
            PostsAndUserAndFriendshipAndComments page = new PostsAndUserAndFriendshipAndComments();
            List<Post> allPostsFromDatabase = db.Posts.ToList();
            page.User = db.Users.Single(x => x.UserId == id);
            List<FriendRequest> allFriendReqsFromDatabase = db.FriendRequests.ToList();
            foreach (FriendRequest friendReq in allFriendReqsFromDatabase)
            {
                //to make sure of already friends or not
                if (friendReq.SenderId == loggedInUserId && friendReq.RecieverId == id)
                {
                    page.NewFriendRequest = friendReq;
                    if (friendReq.Status == true)
                        isFriends = true;
                }
            }
            if (isFriends)
            {
                Like isLiked = new Like();
                List<Like> allLikesFromDatabase = db.Likes.ToList();
                List<Comment> allCommentsFromDatabase = db.Comments.ToList();
                List<Comment> myComments = new List<Comment>();
                foreach (var post in allPostsFromDatabase)
                {
                    //to make sure only can see posts for that user
                    if (post.OwnerId == page.User.UserId)
                    {
                        likesCounter = 0;
                        disLikesCounter = 0;
                        //to know if you already liked or not

                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostId == post.PostId && like.UserId == loggedInUserId)
                                isLiked = like;
                        }
                        //to count how many likes or dislikes
                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostId == post.PostId && like.NuLikes == true)
                            { likesCounter++; }
                            else if (like.PostId == post.PostId && like.NuDislikes == true)
                            {
                                disLikesCounter++;
                            }
                        }
                        //to make sure each comment you can see in its post
                        foreach (var comment in allCommentsFromDatabase)
                        {
                            if (comment.PostId == post.PostId)
                                myComments.Add(comment);
                        }

                        FullHomePage h = new FullHomePage
                        {
                            User = page.User,
                            Posts = post,
                            Like = isLiked,
                            LikesCounter = likesCounter,
                            DisLikesCounter = disLikesCounter,
                            Comments = myComments
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


