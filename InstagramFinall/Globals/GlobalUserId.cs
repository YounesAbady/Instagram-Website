using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramFinall.Globals
{
    public class GlobalUserId
    {

        private static GlobalUserId s_globalUser;
        private static int s_loggedInUserId;
        private GlobalUserId(int id)
        {
            s_loggedInUserId = id;
        }
        public static void Instance(int id)
        {
            if (s_globalUser == null)
            {
                s_globalUser = new GlobalUserId(id);
            }
        }
        public static int Get() { return s_loggedInUserId; }
    }

}