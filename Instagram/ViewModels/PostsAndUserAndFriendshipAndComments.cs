using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class PostsAndUserAndFriendshipAndComments
    {
        public user user { get; set; }
        public FriendRequest f { get; set; }
        public int LoggedinUser { get; set; }
        public List<FullHomePage> pages = new List<FullHomePage>();

    }
}