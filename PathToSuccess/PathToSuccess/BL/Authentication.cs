using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public static class Authentication
    {
        public static bool UserExists(string login)
        {
            return User.Find(login) != null;
        }

        //login is meant to be existing. validate using method above.
        public static bool Authenticate(string login, string pass)
        {
            var user = User.Find(login);
            if (user.ComparePass(pass))
            {
                Application.CurrentUser = user;
                return true;
            }
            return false;
        }

        public static bool Register(User user)
        {
            if (user == null || UserExists(user.Login)) 
                return false;
            User.CreateUser(user);
            Application.CurrentUser = user;
            return true;
        }
    }
}
