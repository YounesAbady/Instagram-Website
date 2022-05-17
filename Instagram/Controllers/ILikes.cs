using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Instagram.Models;

namespace Instagram.Controllers
{
    
    public interface ILikes
    {
        
        void Like(int id);
    }
}