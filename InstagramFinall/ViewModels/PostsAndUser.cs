using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramFinall.Models;
namespace InstagramFinall.ViewModels
{
    public class PostsAndUser
    {
        public Post Post { get; set; }
        public User User { get; set; }
        public Like Like { get; set; }
        public List<Comment> Comments { get; set; }
        public int likesCounter = 0;
        public int DisLikesCounter = 0;
        public Comment MakeComment { get; set; }

    }
}