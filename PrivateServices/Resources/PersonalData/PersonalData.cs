using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lykke.Client.AutorestClient.Models;
using System.Net;
using TestsCore.ServiceSettings.SettingsModels;
using System.Net.Http;
using LykkeAutomationPrivate.Models;
using System.IO;
using TestsCore.TestsCore;
using TestsCore.RestRequests.Interfaces;
using TestsCore.RestRequests;

namespace LykkeAutomationPrivate.Api.PersonalDataResource
{
    public class PersonalData
    {
        private const string resource = "/PersonalData";
        private static PersonalDataSettings _settings;

        private static string apiKey { get { return Settings().ApiKey; } set { } }
        private static string ServiceUri { get { return Settings().ServiceUri; } set { } }
        private static string ExternalServiceUri { get { return Settings().ServiceExternalUri; } set { } }

        protected IRequestBuilder Request => Requests.For(ExternalServiceUri + "/api");


        private static PersonalDataSettings Settings()       
        {
            if(_settings == null)
            {
                _settings = new LykkeApi().settings.PersonalDataSettings().PersonalDataSettings;    
            }
                
            return _settings;
        }

        #region GET requests
        public IResponse<PersonalDataModel> GetPersonalDataResponseByEmail(string email)
        {
            return Request.Get(resource + $"?email={WebUtility.UrlEncode(email)}").WithHeaders("api-key", apiKey).Build().Execute<PersonalDataModel>();
        }

        //list
        public IResponse<List<PersonalDataModel>> GetPersonalDataListResponse()
        {
            return Request.Get(resource + "/public/list").WithHeaders("api-key", apiKey).Build().Execute<List<PersonalDataModel>>();
        }

        //personal data by id
        public IResponse<PersonalDataModel> GetPersonalDataById(string id)
        {
            return Request.Get(resource + $"/{id}").WithHeaders("api-key", apiKey).Build().Execute<PersonalDataModel>();
        }

        //full personal
        public IResponse<FullPersonalDataModel> GetFullPersonalDataById(string id)
        {
            return Request.Get(resource + $"/full/{id}").WithHeaders("api-key", apiKey).Build().Execute<FullPersonalDataModel>();
        }

        //profile data
        public IResponse<ProfilePersonalData> GetProfilePersonalDataById(string id)
        {
            return Request.Get(resource + $"/profile/{id}").WithHeaders("api-key", apiKey).Build().Execute<ProfilePersonalData>();
        }

        //search
        public IResponse<SearchPersonalDataModel> GetSearchPersonalData(string querry)
        {
            return Request.Get(resource + $"/search?phrase={querry}").WithHeaders("api-key", apiKey).Build().Execute<SearchPersonalDataModel>();
        }

        //get document by id
        public IResponse GetDocumentScanById(string id)
        {
            return Request.Get(resource + $"/documentscan/{id}").WithHeaders("api-key", apiKey).Build().Execute();
        }

        public string GetDocumentScanByIdModel(string id) => GetDocumentScanById(id)?.Content;
        #endregion

        #region Post Requests
        public IResponse<List<PersonalDataModel>> PostPersonalDataListById(params string[] id)
        {
            return Request.Post(resource + $"/list").WithHeaders("api-key", apiKey).AddJsonBody(id).Build().Execute<List<PersonalDataModel>>();
        }

        //post full list
        public IResponse<List<FullPersonalDataModel>> PostFullPersonalDataListById(params string[] id)
        {
            return Request.Post(resource + $"/list").WithHeaders("api-key", apiKey).AddJsonBody(id).Build().Execute<List<FullPersonalDataModel>>();
        }

        //post paged Exclude
        public IResponse<PagedResultModelPersonalDataModel> PostPageExclude(PagedRequestModel pagedRequest)
        {
            return Request.Post(resource + $"/list/pagedExclude").WithHeaders("api-key", apiKey).AddJsonBody(pagedRequest).Build().Execute<PagedResultModelPersonalDataModel>();
        }

        //POST /api/PersonalData/list/paged
        public IResponse<PagedResultModelPersonalDataModel> PostPage(PagingInfoModel pageInfo)
        {
            return Request.Post(resource + $"/list/paged").WithHeaders("api-key", apiKey).AddJsonBody(pageInfo).Build().Execute<PagedResultModelPersonalDataModel>();
        }

        //POST /api/PersonalData/list/pagedIncludeOnly
        public IResponse<PagedResultModelPersonalDataModel> PostPagedIncludedOnly(PagedRequestModel pagedRequest)
        {
            return Request.Post(resource + $"/list/pagedIncludeOnly").WithHeaders("api-key", apiKey).AddJsonBody(pagedRequest).Build().Execute<PagedResultModelPersonalDataModel>();
        }

        //POST /api/PersonalData/list/byRegistrationDate
        public IResponse<List<FullPersonalDataModel>> PostListbyRegistrationDate(RegistrationDatesModel registrationDates)
        {
            return Request.Post(resource + $"/list/pagedIncludeOnly").WithHeaders("api-key", apiKey).AddJsonBody(registrationDates).Build().Execute<List<FullPersonalDataModel>>();        
        }
        
        //POST /api/PersonalData  Save personal info
        //no result model
        public IResponse PostPersonalData(FullPersonalDataModel fullPersonalDataModel)
        {
            return Request.Post(resource).WithHeaders("api-key", apiKey).AddJsonBody(fullPersonalDataModel).Build().Execute();
        }

        //POST /api/PersonalData/{id}/archive Delete item with id provided
        //no result model
        public IResponse PostPersonalDataArchive(ArchiveRequest archiveRequest)
        {
            return Request.Post(resource + $"/{archiveRequest.ClientId}/archive").WithHeaders("api-key", apiKey).AddJsonBody(archiveRequest).Build().Execute();
        }

        //POST /api/PersonalData/{id}/email  Changing email
        public IResponse<ChangeFieldResponseModel> PostPersonalDataChangeEmail(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Post(resource + $"/{changeFieldRequest.ClientId}/email").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute<ChangeFieldResponseModel>();
        }
        
        //POST /api/PersonalData/avatar/{id}  Add avatar
        public IResponse PostAddAvatar(string id, string filePath)
        {
            var request = Request.Post(resource + $"/avatar/{id}").WithHeaders("api-key", apiKey);
            byte[] x = File.ReadAllBytesAsync(filePath).Result;
            var content = new ByteArrayContent(x);
            request.AddObject(content);
            return request.Build().Execute();
        }
        #endregion

        #region PUT
        //PUT /api/PersonalData Update personal info
        public IResponse PutPersonalData(PersonalDataModel personalDataModel)
        {
            return Request.Put(resource).WithHeaders("api-key", apiKey).AddJsonBody(personalDataModel).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/fullname
        public IResponse PutPersonalDataFullName(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/fullname").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/firstName
        public IResponse PutPersonalDataFirstName(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/firstName").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/lastName
        public IResponse PutPersonalDataLastName(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/lastName").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/dateOfBirth  Change date of birth (MM/DD/YYYY)
        public IResponse PutPersonalDataDateOfBirth(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/dateOfBirth").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/DateOfExpiryOfID  Change date of expiry of ID birth (MM/DD/YYYY)
        public IResponse PutPersonalDataDateOfExpiryOfId(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/DateOfExpiryOfID").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/country  Change country
        public IResponse PutPersonalDataCountry(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/country").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/countryFromID  Change country from ID
        public IResponse PutPersonalDataCountryFromId(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/countryFromID").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/countryFromPOA  Change country from IP
        public IResponse PutPersonalDataCountryFromPOA(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/countryFromPOA").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/city  Change city
        public IResponse PutPersonalDataCity(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/city").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/zip  Change zip
        public IResponse PutPersonalDataZip(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/zip").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/address  Change address
        public IResponse PutPersonalDataAddress(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/address").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/phoneNumber  Change contact phone number
        public IResponse PutPersonalDataPhoneNumber(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/phoneNumber").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/geolocation  Change geolocation data
        public IResponse PutPersonalDataGeolocation(string id, ChangeGeolocationRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{id}/geolocation").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/passwordHint  Change password hint
        public IResponse PutPersonalDataPasswordHint(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/passwordHint").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/refCode  Set referral code
        public IResponse PutPersonalDataRefCode(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/refCode").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/spotRegulator  Change spot regulator
        public IResponse PutPersonalDataSpotRegulator(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/spotRegulator").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/marginRegulator  Change margin regulator
        public IResponse PutPersonalDataMarginRegulator(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/marginRegulator").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/{id}/paymentSystem  Change payment system
        public IResponse PutPersonalDataPaymentSystem(ChangeFieldRequest changeFieldRequest)
        {
            return Request.Put(resource + $"/{changeFieldRequest.ClientId}/paymentSystem").WithHeaders("api-key", apiKey).AddJsonBody(changeFieldRequest).Build().Execute();
        }

        //PUT /api/PersonalData/profile  Update profile info
        public IResponse PutPersonalDataProfile(UpdateProfileInfoRequest updateProfileInfoRequest)
        {
            return Request.Put(resource + $"/profile").WithHeaders("api-key", apiKey).AddJsonBody(updateProfileInfoRequest).Build().Execute();
        }

        //PUT /api/PersonalData/documentscan/{id}  Add document scan
        public IResponse PutPersonalDataDocumentScan(string id, string filePath)
        {
            byte[] x = File.ReadAllBytesAsync(filePath).Result;
            var content = new ByteArrayContent(x);

            return Request.Put(resource + $"/documentscan/{id}").WithHeaders("api-key", apiKey).AddObject(content).Build().Execute();
        }
        #endregion

        #region DELETE

        //DELETE /api/PersonalData/avatar/{id}  Delete avatar
        public IResponse DELETEPersonalDataAvatar(string id)
        {
            return Request.Delete(resource + $"/avatar/{id}").WithHeaders("api-key", apiKey).Build().Execute();
        }

        //DELETE /api/PersonalData/cache  Clears cache
        public IResponse DELETEPersonalDataCache(string id)
        {
            return Request.Delete(resource + $"/cache").WithHeaders("api-key", apiKey).Build().Execute();
        }        
        #endregion
    }
}
