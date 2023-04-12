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
        private V1User _loggedInUser;
        private V1User _selectedUser;

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

        public void SetUser(V1User user, bool setLoggedInUser)
        {
            if (setLoggedInUser)
            {
                _loggedInUser = user;
            }
            else
            {
                _selectedUser = user;
            }
        }

        public V1User GetUser(bool getLoggedInUser)
        {
            return getLoggedInUser ? _loggedInUser : _selectedUser;
        }

        public V1User SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; }
        }
    }
}


