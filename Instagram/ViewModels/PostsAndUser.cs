using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class PostsAndUser
    {
        public List<post> p { get; set; }
        public user u { get; set; }

    }
}