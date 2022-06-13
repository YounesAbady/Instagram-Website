using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramFinall.Models;
namespace InstagramFinall.ViewModels
{
    public class PostsAndUserAndFriendshipAndComments
    {
        public User User { get; set; }
        public FriendRequest NewFriendRequest { get; set; }
        public int LoggedinUser { get; set; }
        public List<FullHomePage> Pages = new List<FullHomePage>();

    }
}