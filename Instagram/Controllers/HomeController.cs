using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.ViewModels;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
using Instagram.Globals;

>>>>>>> Stashed changes
=======
using Instagram.Globals;

>>>>>>> Stashed changes
namespace Instagram.Controllers
{
    public class HomeController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Home
        public ActionResult ViewAllPosts()
        {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            FullHomePage home = new FullHomePage
            {
                User = db.users.ToList(),
                posts=db.posts.ToList()
            };
=======
=======
>>>>>>> Stashed changes
            List<FullHomePage> home = new List<FullHomePage>();
            List<post> posts = db.posts.ToList();
            List<FriendRequest> friends = db.FriendRequests.ToList();
           
            foreach (var x in friends) {
                if (x.Status==true&&(x.RecieverID==GlobalUserID.globalUserID||x.SenderID==GlobalUserID.globalUserID))
                {
                    user user = new user();
                    if (x.RecieverID == GlobalUserID.globalUserID) { user = db.users.Find(x.SenderID); }
                    else { user = db.users.Find(x.RecieverID); }
                    foreach (var item in posts) {
                        if (item.OwnerID == user.UserID)
                        {
                            FullHomePage h = new FullHomePage
                            {
                                User=user,
                                posts=item
                            };
                            if (!home.Contains(h))
                                 { home.Add(h); }
                        }
                    }

                }
            }
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            
            return View(home);
        }
    }
}