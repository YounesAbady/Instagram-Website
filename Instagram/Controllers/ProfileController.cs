using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
using System.Data.Entity;
using System.IO;
using Instagram.ViewModels;

namespace Instagram.Controllers
{
    public class ProfileController : Controller // , ILikes
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Profile

        public ActionResult ViewProfile()
        {
            int loggedInUserID= GlobalUserID.get();
            int likesCounter;
            int dislikesCounter;
            List<PostsAndUser> fullPosts = new List<PostsAndUser>();
            user loggedUser = db.users.Single(x => x.UserID == loggedInUserID);
            List<post> allPostsFromDatabase = db.posts.ToList();
            List<comment> commentsForSpecificPost = new List<comment>();
            List<like> allLikesFromDatabase = db.likes.ToList();
            List<comment> allCommentsFromDatabase = db.comments.ToList();
            like isLiked = new like();
            foreach (post post in allPostsFromDatabase)
            {
                likesCounter = 0;
                dislikesCounter = 0;
                //to make sure only logged in user posts goes to view
                if (post.OwnerID == loggedInUserID)
                {
                    //to check if the logged in user already made like or dislike
                    foreach (var like in allLikesFromDatabase)
                    {
                        if (like.PostID == post.PostID && like.UserID == loggedInUserID)
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
                    PostsAndUser finallPost = new PostsAndUser
                    {
                        post = post,
                        user = loggedUser,
                        likesCounter = likesCounter,
                        disLikesCounter = dislikesCounter,
                        comments = commentsForSpecificPost,
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

            if (fullPosts.Count==0)
            {
                //to check if profile empty or not
                PostsAndUser finallPost = new PostsAndUser
                {
                    
                    user = loggedUser
      
                };
                fullPosts.Add(finallPost);
            }
            return View(fullPosts);
        }

        [HttpGet]
        public ActionResult EditProfile(int id)
        {
            //load old profile data
            var oldUser = db.users.Single(x => x.UserID == id);
            if (oldUser == null)
                return HttpNotFound();
            else
            {
                return View(oldUser);
            }

        }
        [HttpPost]
        public ActionResult EditProfile(user editedUser)
        {
            if (ModelState.IsValid)
            {
                var oldUser = db.users.Single(x => x.UserID == editedUser.UserID);
                 //change to new data
                oldUser.Fname = editedUser.Fname;
                oldUser.Lname = editedUser.Lname;
                oldUser.Password = editedUser.Password;
                oldUser.mobile = editedUser.mobile;
                oldUser.email = editedUser.email;

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

            like newLike = new like();
            newLike = newLike.Like(id);
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
            newDislike = newDislike.disLike(id);
            db.likes.Add(newDislike);
            db.SaveChanges();

            return RedirectToAction("ViewAllPosts", "Home");
        }
        public ActionResult ViewOtherProfile(int id)
        {
            int loggedInUserID = GlobalUserID.get();
            bool isFriends = false;
            int likesCounter;
            int dislikesCounter;
            PostsAndUserAndFriendshipAndComments page = new PostsAndUserAndFriendshipAndComments();
            List<post> allPostsFromDatabase = db.posts.ToList();
            page.user = db.users.Single(x => x.UserID == id);
            List<FriendRequest> allFriendReqsFromDatabase = db.FriendRequests.ToList();
            foreach (FriendRequest f in allFriendReqsFromDatabase)
            {
                //to make sure of already friends or not
                if (f.SenderID == loggedInUserID && f.RecieverID == id)
                {
                    page.f = f;
                    if (f.Status == true)
                        isFriends = true;
                }
            }
            if (isFriends)
            {
                like isLiked = new like();
                List<like> allLikesFromDatabase = db.likes.ToList();
                List<comment> allCommentsFromDatabase = db.comments.ToList();
                List<comment> mycomments = new List<comment>();
                foreach (var post in allPostsFromDatabase)
                {
                    //to make sure only can see posts for that user
                    if (post.OwnerID == page.user.UserID)
                    {
                        likesCounter = 0;
                        dislikesCounter = 0;
                        //to know if you already liked or not

                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostID == post.PostID && like.UserID == loggedInUserID)
                                isLiked = like;
                        }
                        //to count how many likes or dislikes
                        foreach (var like in allLikesFromDatabase)
                        {
                            if (like.PostID == post.PostID && like.NuLikes == true)
                            { likesCounter++; }
                            else if (like.PostID == post.PostID && like.NuDislikes == true)
                            {
                                dislikesCounter++;
                            }
                        }
                        //to make sure each comment you can see in its post
                        foreach (var comment in allCommentsFromDatabase)
                        {
                            if (comment.PostID == post.PostID)
                                mycomments.Add(comment);
                        }

                        FullHomePage h = new FullHomePage
                        {
                            User = page.user,
                            posts = post,
                            Like = isLiked,
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


