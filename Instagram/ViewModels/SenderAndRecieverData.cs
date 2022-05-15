using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;
namespace Instagram.ViewModels
{
    public class SenderAndRecieverData
    {
        public user Sender { get; set; }
        public FriendRequest request { get; set; }
    }
}