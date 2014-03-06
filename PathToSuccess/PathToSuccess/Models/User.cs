using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    class User
    {
        private string _login;
        public string Login
        {
            get 
            {
                return _login;
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        private string _surname;
        public string Surname
        {
            get 
            {
                return _surname;
            }
        }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get 
            {
                return _dateOfBirth;
            }
        }
        private DateTime _dateOfRegistration;
        private int _password;

        public User(string login, string name, string sname, DateTime birth, string pass)
        {
            this._login = login;
            this._name = name;
            this._surname = sname;
            this._dateOfBirth = birth;
            this._dateOfRegistration = DateTime.Now;
            this._password = pass.GetHashCode();
        }
        public User(string login, string name, string sname, DateTime birth, DateTime registration, int pass)
        {
            this._login = login;
            this._name = name;
            this._surname = sname;
            this._dateOfBirth = birth;
            this._dateOfRegistration = registration;
            this._password = pass;
        }
        public bool ComparePass(string pass)
        {
            return _password == pass.GetHashCode();
        }
    }
}
