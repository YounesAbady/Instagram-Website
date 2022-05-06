using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class FullHomePage
    {
        public List<post> posts { get; set; }
        public List<user> User { get; set; }
        public like Like { get; set; }
        public List<comment> comments { get; set; }
    }
}