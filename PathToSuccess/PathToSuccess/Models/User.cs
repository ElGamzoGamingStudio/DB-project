using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using PathToSuccess.DAL;
using System.Security.Cryptography;
using System.Text;

namespace PathToSuccess.Models
{
    [Table("users", Schema = "public")]
    public class User
    {
        protected bool Equals(User other)
        {
            return string.Equals(Login, other.Login) && string.Equals(Name, other.Name) &&
                   DateOfBirth.Equals(other.DateOfBirth) && Password == other.Password && DateReg.Equals(other.DateReg);
        }

        public override int GetHashCode() // MERCY
        {
            unchecked
            {
                var hashCode = (Login != null ? Login.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ DateOfBirth.GetHashCode();
                hashCode = (hashCode*397) ^ Password;
                hashCode = (hashCode*397) ^ DateReg.GetHashCode();
                return hashCode;
            }
        }

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
            //Password = pass.GetHashCode();
            Password = BL.Application.Hash(pass);
            DateReg = dateReg;
        }

        public bool ComparePass(string pass)
        {
            //return Password == pass.GetHashCode();
            return Password == BL.Application.Hash(pass);
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
                user.Password = Convert.ToInt32(BL.Application.Hash(pass));
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
            //u.Password = newPass.GetHashCode();
            u.Password = BL.Application.Hash(newPass);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static bool CheckLoginIsUnique(string login)
        {
            return DAL.SqlRepository.Users.Find(login) == null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }
    }
}
