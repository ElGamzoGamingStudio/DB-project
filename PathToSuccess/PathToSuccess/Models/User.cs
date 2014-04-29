using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    [Table("users",Schema = "public")]
    class User
    {
        [Key]
        [Column("login")]
        public string Login { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("birthday")]
        public DateTime DateOfBirth { get; set; }

        [Column("password")]
        public int Password { get; set; }

        public User()
        {}

        public User(string login, string name, string sname, DateTime birth, string pass)
        {
            Login = login;
            Name = name;
            Surname = sname;
            DateOfBirth = birth;
            Password = pass.GetHashCode();
        }
        public bool ComparePass(string pass)
        {
            return Password == pass.GetHashCode();
        }
    }
}
