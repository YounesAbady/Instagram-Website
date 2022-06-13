using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramFinall.Models;
namespace InstagramFinall.ViewModels
{
    public class FullHomePage
    {
        public Post Posts { get; set; }
        public User User { get; set; }
        public Like Like { get; set; }
        public List<Comment> Comments { get; set; }
        public int LikesCounter = 0;
        public int DisLikesCounter = 0;
        public Comment MakeComment { get; set; }
    }
}