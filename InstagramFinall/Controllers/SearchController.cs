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
    public class SearchController : Controller
    {
        ProjectDataBaseEntities db = new ProjectDataBaseEntities();
        // GET: Search

        public ActionResult Search(string searching)
        {
            int id = GlobalUserId.Get();
            ViewData["ID"] = id;
            //user can search by first name-last name-email
            return View(db.Users.Where(x => x.FirstName.StartsWith(searching) || x.LastName.StartsWith(searching) || searching == null || x.Email.StartsWith(searching)).ToList());
        }



    }
}