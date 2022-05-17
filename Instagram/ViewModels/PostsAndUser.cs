using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class PostsAndUser
    {
        public post p { get; set; }
        public user u { get; set; }
        public like Like { get; set; }
        public List<comment> comments { get; set; }
        public int LikesCounter = 0;
        public int DislikesCounter = 0;
        public comment makeComment { get; set; }

    }
}