using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
    public class UserSingleton
    {
        private static UserSingleton _instance;
        private V1User _user;

        public static UserSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSingleton();
                }
                return _instance;
            }
        }

        public void SetUser(V1User user)
        {
            _user = user;
        }

        public V1User GetUser()
        {
            return _user;
        }
    }
}

