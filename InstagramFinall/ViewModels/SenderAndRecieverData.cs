using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InstagramFinall.Models;
namespace InstagramFinall.ViewModels
{
    public class SenderAndReceiverData
    {
        public User Sender { get; set; }
        public FriendRequest Request { get; set; }
    }
}