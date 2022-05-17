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
    public class ProfileController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Profile

        public ActionResult ViewProfile()
        {
            int likesCounter;
            int dislikesCounter;
            List<PostsAndUser> fullPost = new List<PostsAndUser>();
            
            user user = db.users.Single(x => x.UserID == GlobalUserID.loggedInUserID);
            List<post> allPostsFromDatabase = db.posts.ToList();
            List<comment> mycomments = new List<comment>();
            List<like> allLikesFromDatabase = db.likes.ToList();
            List<comment> allCommentsFromDatabase = db.comments.ToList();
            like like = new like();
            foreach (post s in allPostsFromDatabase)
            {
                likesCounter = 0;
                dislikesCounter = 0;
                if (s.OwnerID == GlobalUserID.loggedInUserID)
                {
                    
                    foreach (var y in allLikesFromDatabase)
                    {
                        if (y.PostID == s.PostID && y.UserID == GlobalUserID.loggedInUserID)
                            like = y;
                    }
                    foreach (var y in allLikesFromDatabase)
                    {

                        if (y.PostID == s.PostID && y.NuLikes == true)
                        { likesCounter++; }
                        else if (y.PostID == s.PostID && y.NuDislikes == true)
                        {
                            dislikesCounter++;
                        }
                    }
                    foreach (var y in allCommentsFromDatabase)
                    {
                        if (y.PostID == s.PostID)
                            mycomments.Add(y);
                    }
                    PostsAndUser finallPost = new PostsAndUser
                    {
                        p = s,
                        u = user,
                        LikesCounter = likesCounter,
                        DislikesCounter = dislikesCounter,
                        comments = mycomments,
                        Like = like
                    };
                    if (!fullPost.Contains(finallPost))
                        fullPost.Add(finallPost);


                }
            }
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(fullPost);
        }

        [HttpGet]
        public ActionResult EditProfile(int id)
        {
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
                oldUser.Fname = editedUser.Fname;
                oldUser.Lname = editedUser.Lname;
                oldUser.Password = editedUser.Password;
                oldUser.mobile = editedUser.mobile;
                oldUser.email = editedUser.email;
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

            like newLike = new like
            {
                PostID = id,
                NuLikes = true,
                NuDislikes = false,
                UserID = GlobalUserID.loggedInUserID
            };

            db.likes.Add(newLike);
            db.SaveChanges();

            return RedirectToAction("ViewProfile");
        }

        public ActionResult Undo(int id)
        {
            like like = db.likes.Find(id);
            db.likes.Remove(like);
            db.SaveChanges();
            return RedirectToAction("ViewProfile");
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

            return RedirectToAction("ViewProfile");
        }

    }
}


