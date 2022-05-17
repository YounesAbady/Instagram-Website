using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class PostsAndUser
    {
        public post post { get; set; }
        public user user { get; set; }
        public like Like { get; set; }
        public List<comment> comments { get; set; }
        public int likesCounter = 0;
        public int disLikesCounter = 0;
        public comment makeComment { get; set; }

    }
}