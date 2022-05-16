using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Instagram.Models;
using Instagram.Globals;
using Instagram.ViewModels;
using System.Data.Entity;
namespace Instagram.Controllers
{
    public class FriendsController : Controller
    {  ProjectDataBaseEntities db = new ProjectDataBaseEntities();

        // GET: Friends
        public ActionResult ViewRequests()
        {
            List<SenderAndRecieverData> SenderAndReciever = new List<SenderAndRecieverData>();
            List<FriendRequest> friends = db.FriendRequests.ToList();
            foreach (var item in friends)
            {
                if (item.RecieverID == GlobalUserID.globalUserID||item.SenderID==GlobalUserID.globalUserID)
                {
                    var user=new user();
                    if (item.RecieverID == GlobalUserID.globalUserID)
                    {  user = db.users.Single(x => x.UserID == item.SenderID); }
                    else
                    {  user = db.users.Single(x => x.UserID == item.RecieverID); }
                    SenderAndRecieverData s = new SenderAndRecieverData
                    {
                        Sender = user,
                        request = item
                    };
                SenderAndReciever.Add(s);
                }
            }
            return View(SenderAndReciever);
        }
        public ActionResult Accept(int id)
        {
            var EditedRequest = db.FriendRequests.Find(id);
            EditedRequest.Status = true;
            db.Entry(EditedRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
        public ActionResult Reject(int id)
        {
            var EditedRequest = db.FriendRequests.Find(id);
            EditedRequest.Status = false;
            db.Entry(EditedRequest).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ViewRequests");
        }
    }
}