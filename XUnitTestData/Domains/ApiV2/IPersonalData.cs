using System;

namespace XUnitTestData.Domains.ApiV2
{
    public interface IPersonalData : IDictionaryItem
    {
        string City { get; set; }
        string ContactPhone { get; set; }
        string Country { get; set; }
        string CountryFromID { get; set; }
        string CountryFromPOA { get; set; }
        string Email { get; set; }
        string FullName { get; set; }
        string Isp { get; set; }
        string PasswordHint { get; set; }
        DateTime Regitered { get; set; }
    }
}