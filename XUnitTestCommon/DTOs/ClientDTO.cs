using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs
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
        public string UserAgent { get; set; }
        public string PartnerId { get; set; }
        public string Ip { get; set; }
        public string Changer { get; set; }
        public int IosVersion { get; set; }
        public string Referer { get; set; }
    }

    public class ClientRegisterResponseDTO
    {
        public AccountDTO Account { get; set; }
        public string Token { get; set; }
        public string NotificationsId { get; set; }
        public PersonalDataDTO PersonalData { get; set; }
        public bool CanCashInViaBankCard { get; set; }
        public bool SwiftDepositEnabled { get; set; }
    }

    public class AccountDTO
    {
        public string Id { get; set; }
        public DateTime Registered { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Pin { get; set; }
        public string NotificationsId { get; set; }
        public string PartnerId { get; set; }
        public bool IsReviewAccount { get; set; }
        public bool IsTrusted { get; set; }
    }
}
