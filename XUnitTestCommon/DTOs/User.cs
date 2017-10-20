using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Utils;

namespace XUnitTestCommon.DTOs
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientInfo { get; set; }
        public string PartnerId { get; set; }

        public User() { }

        public User(string email, string password, string clientInfo, string partnerId)
        {
            Email = email;
            Password = password;
            ClientInfo = clientInfo;
            PartnerId = partnerId;
        }
    }
}
