using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Instagram.Globals
{
    public class GlobalUserID
    {

        private static GlobalUserID globalUser;
        private static int loggedInUserID;
        private GlobalUserID(int id) {
            loggedInUserID = id;
        }
        public static void Instance(int id)
        {
            if (globalUser == null)
            {
                globalUser = new GlobalUserID(id);
            }
        }
        public static int get() { return loggedInUserID; }
    }

}