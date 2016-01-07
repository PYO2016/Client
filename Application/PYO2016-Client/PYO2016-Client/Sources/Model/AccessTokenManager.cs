using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PYO2016_Client.Sources.Model
{
    class AccessTokenManager
    {
        static private AccessTokenManager a = null;

        private string token;

        private AccessTokenManager()
        {
        }

        public void setToken(string s)
        {
            token = s;
            return;
        }

        public string getToken()
        {
            return token;
        }

        static public AccessTokenManager getInstance()
        {
            if (a == null)
                a = new AccessTokenManager();
            return a;
        }
    }
}
