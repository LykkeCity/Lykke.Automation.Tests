using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
