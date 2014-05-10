using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using PathToSuccess.DAL;

namespace PathToSuccess.Models
{
    [Table("users", Schema = "public")]
    public class User
    {
        [Key]
        [Column("login")]
        public string Login { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("birthdate")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column("password")]
        public int Password { get; set; }

        [Column("date_reg")]
        public DateTime DateReg { get; set; }

        public static User PIOS()
        {
            var pioooooos = (User)SqlRepository.Users.Find("xxxrassiyavperedxxx");
            return pioooooos;
        }

        public User()
        { }

        public User(string login, string name, DateTime birth, string pass, DateTime dateReg)
        {
            Login = login;
            Name = name;
            DateOfBirth = birth;
            Password = pass.GetHashCode();
            DateReg = dateReg;
        }

        public bool ComparePass(string pass)
        {
            return Password == pass.GetHashCode();
        }

        public static User CreateUser(string login, string name, DateTime birth, string pass, DateTime dateReg)
        {
            var set = DAL.SqlRepository.Users;
            var user = new User();

            if (set.Find(user.Login) == null)
            {
                user.Login = login;
                user.Name = name;
                user.DateOfBirth = birth;
                user.Password = pass.GetHashCode();
                user.DateReg = dateReg;

                set.Add(user);
                DAL.SqlRepository.Save();

                return user;
            }
            return null;
        }

        public static User CreateUser(string login, string name, DateTime birth, int pass, DateTime dateReg)
        {
            var set = DAL.SqlRepository.Users;
            var user = new User();

            if (set.Find(user.Login) == null)
            {
                user.Login = login;
                user.Name = name;
                user.DateOfBirth = birth;
                user.Password = pass;
                user.DateReg = dateReg;

                set.Add(user);
                DAL.SqlRepository.Save();

                return user;
            }
            return null;
        }

        public static void DeleteUser(User user)
        {
            var set = DAL.SqlRepository.Users;
            var usr = set.Find(user.Login);
            if (usr != null)
            {
                set.Remove(usr);
                DAL.SqlRepository.Save();
            }
        }

        public static void ResetPass(User user, string newPass)
        {
            var u = (User)DAL.SqlRepository.Users.Find(user.Login);
            u.Password = newPass.GetHashCode();
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static bool CheckLoginIsUnique(string login)
        {
            return DAL.SqlRepository.Users.Find(login) == null;
        }
    }
}
