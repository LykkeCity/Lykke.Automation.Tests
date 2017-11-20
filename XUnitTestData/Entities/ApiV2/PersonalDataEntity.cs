using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using System.Threading.Tasks;
using AzureStorage;

namespace XUnitTestData.Entities.ApiV2
{
    public class PersonalDataEntity : TableEntity, IPersonalData
    {
        public static string GeneratePartitionKey()
        {
            return "PD";
        }

        public string Id => RowKey;

        public string City { get; set; }
        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public string CountryFromID { get; set; }
        public string CountryFromPOA { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Isp { get; set; }
        public string PasswordHint { get; set; }
        public DateTime Regitered { get; set; }

    }
}
