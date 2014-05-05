using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PathToSuccess.Models
{
    [Table("users", Schema="public")]
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

        public User()
        {}

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

        public static void CreateUser(User user)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<User>();

            if (set.Find(user.Login) == null)
            {
                set.Add(user);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }

        public static void DeleteUser(User user)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<User>();
            var usr = set.Find(user.Login);
            if (usr != null)
            {
                set.Remove(usr);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }

        public void ResetPass(string newPass)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<User>();
            Password = newPass.GetHashCode();
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static bool CheckLoginIsUnique(string login)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<User>();
            return set.Find(new object[] { login }) == null;
        }

        public static User Find(string login)
        {
            return (User) DAL.SqlRepository.DBContext.GetDbSet<User>()
                .Find(login);
        }
    }
}
