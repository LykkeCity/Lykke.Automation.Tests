using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestData.Domains.ApiV2
{
    public class ClientDTO
    {
        public string Token { get; set; }
        public string NotificationsId { get; set; }
        public PersonalDataDTO PersonalData { get; set; }
        public bool CanCashInViaBankCard { get; set; }
        public bool SwiftDepositEnabled { get; set; }
    }

    public class PersonalDataDTO
    {
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
    }

    public class ClientRegisterDTO
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ContactPhone { get; set; }
        public string Password { get; set; }
        public string Hint { get; set; }
        public string ClientInfo { get; set; }
        public string PartnerId { get; set; }
    }
}
