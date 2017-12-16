using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsCore.TestsData;
using Newtonsoft.Json;
using LykkeAutomation.Api.ApiModels.ErrorModel;

namespace LykkeAutomation.ApiModels.PersonalDataModels
{
    public class PersonalDataModel
    {
        [JsonProperty("Result")]
        public PersonalData PersonalData { get; set; }
        public Error Error { get; set; }
    }

    public class PersonalData
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


        public PersonalData()
        {
            SetName();
            Email = TestData.GenerateEmail();
        }

        public void SetName()
        {
            var person = TestData.FirstLastName();

            this.FirstName = person.Key;
            this.LastName = person.Value;
            this.FullName = $"{FirstName} {LastName}";
        }
    }
}
