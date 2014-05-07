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
            return DAL.SqlRepository.Users.Find(login) != null;
        }

        //login is meant to be existing. validate using method above.
        public static bool Authenticate(string login, string pass)
        {
            var user = (User)DAL.SqlRepository.Users.Find(login);
            if (user!=null && user.ComparePass(pass))
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
            var usr = User.CreateUser(user.Login, user.Name, user.DateOfBirth, user.Password, user.DateReg);
            Application.CurrentUser = usr;
            return true;
        }

        public static void LogOut()
        {
            Application.CurrentTree = null;
            Application.CurrentUser = null;
            //login window should not be opened here.
        }
    }
}
