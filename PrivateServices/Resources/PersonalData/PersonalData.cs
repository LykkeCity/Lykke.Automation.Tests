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
using TestsCore.ApiRestClient;

namespace LykkeAutomationPrivate.Api.PersonalDataResource
{
    public class PersonalData : RestApi
    {
        private const string resource = "/PersonalData";
        private static PersonalDataSettings _settings;
        
        private static PersonalDataSettings Settings()       
        {
            if(_settings == null)
            {
                _settings = new LykkeApi().settings.PersonalDataSettings().PersonalDataSettings;    
            }
                
            return _settings;
        }

        private static string apiKey { get { return Settings().ApiKey; } set { } }
        private static string ServiceUri { get { return Settings().ServiceUri; } set { } }
        private static string ExternalServiceUri { get { return Settings().ServiceExternalUri; } set { } }

        public PersonalData() : base(ExternalServiceUri + "/api")
        {
        }

        public override void SetAllureProperties()
        {
            var isAlive = GetIsAlive();
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", ExternalServiceUri + "/api" + resource);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Environment", isAlive.Env);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
        }

        public IsAliveResponse GetIsAlive()
        {
            var request = new RestRequest("/IsAlive", Method.GET);
            var response = client.Execute(request);
            var isAlive = JsonConvert.DeserializeObject<IsAliveResponse>(response.Content);
            return isAlive;
        }

        #region GET requests
        public IRestResponse GetPersonalDataResponseByEmail(string email)
        {
            var request = new RestRequest(resource + $"?email={WebUtility.UrlEncode(email)}", Method.GET);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public PersonalDataModel GetPersonalDataModel(string email) => JsonConvert.DeserializeObject<PersonalDataModel>(GetPersonalDataResponseByEmail(email)?.Content);


        //list
        public IRestResponse GetPersonalDataListResponse()
        {
            var request = new RestRequest(resource + "/public/list", Method.GET);
            request.AddHeader("api-key", apiKey);

            var response = client.Execute(request);
            return response;
        }

        public List<PersonalDataModel> GetPersonalDataListModel() => JsonConvert.DeserializeObject<List<PersonalDataModel>>(GetPersonalDataListResponse()?.Content);

        //personal data by id
        public IRestResponse GetPersonalDataById(string id)
        {
            var request = new RestRequest(resource + $"/{id}", Method.GET);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public PersonalDataModel GetPersonalDataModelById(string id) => JsonConvert.DeserializeObject<PersonalDataModel>(GetPersonalDataById(id)?.Content);

        //full personal
        public IRestResponse GetFullPersonalDataById(string id)
        {
            var request = new RestRequest(resource + $"/full/{id}", Method.GET);//////////////////////////////////
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public FullPersonalDataModel GetFullPersonalDataModelById(string id) => JsonConvert.DeserializeObject<FullPersonalDataModel>(GetFullPersonalDataById(id)?.Content);

        //profile data
        public IRestResponse GetProfilePersonalDataById(string id)
        {
            var request = new RestRequest(resource + $"/profile/{id}", Method.GET);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public ProfilePersonalData GetProfilePersonalDataModelById(string id) => JsonConvert.DeserializeObject<ProfilePersonalData>(GetProfilePersonalDataById(id)?.Content);

        //search
        public IRestResponse GetSearchPersonalData(string querry)
        {
            var request = new RestRequest(resource + $"/search?phrase={querry}", Method.GET);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public SearchPersonalDataModel GetSearchPersonalDataModel(string id) => JsonConvert.DeserializeObject<SearchPersonalDataModel>(GetSearchPersonalData(id)?.Content);

        //get document by id
        public IRestResponse GetDocumentScanById(string id)
        {
            var request = new RestRequest(resource + $"/documentscan/{id}", Method.GET);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public string GetDocumentScanByIdModel(string id) => GetDocumentScanById(id)?.Content;
        #endregion

        #region Post Requests
        public IRestResponse PostPersonalDataListById(params string[] id)
        {
            var request = new RestRequest(resource + $"/list", Method.POST);
            request.AddJsonBody(id);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        public List<PersonalDataModel> PostPersonalDataListByIdModel(params string[] id) => JArray.Parse(PostPersonalDataListById(id)?.Content).ToObject<List<PersonalDataModel>>();

        //post full list
        public IRestResponse PostFullPersonalDataListById(params string[] id)
        {
            var request = new RestRequest(resource + $"/list", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(id);
            StringContent content = new StringContent(JsonConvert.SerializeObject(id));
            var response = client.Execute(request);
            return response;
        }

        public List<FullPersonalDataModel> PostFullPersonalDataListByIdModel(params string[] id) => JArray.Parse(PostFullPersonalDataListById(id)?.Content).ToObject<List<FullPersonalDataModel>>();

        //post paged Exclude
        public IRestResponse PostPageExclude(PagedRequestModel pagedRequest)
        {
            var request = new RestRequest(resource + $"/list/pagedExclude", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(pagedRequest);
            var response = client.Execute(request);
            return response;
        }

        public PagedResultModelPersonalDataModel PostPageExcludeModel(PagedRequestModel pagedRequest) => JsonConvert.DeserializeObject<PagedResultModelPersonalDataModel>(PostPageExclude(pagedRequest)?.Content);

        //POST /api/PersonalData/list/paged
        public IRestResponse PostPage(PagingInfoModel pageInfo)
        {
            var request = new RestRequest(resource + $"/list/paged", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(pageInfo);
            var response = client.Execute(request);
            return response;
        }

        public PagedResultModelPersonalDataModel PostPageModel(PagingInfoModel pageInfo) => JsonConvert.DeserializeObject<PagedResultModelPersonalDataModel>(PostPage(pageInfo)?.Content);

        //POST /api/PersonalData/list/pagedIncludeOnly
        public IRestResponse PostPagedIncludedOnly(PagedRequestModel pagedRequest)
        {
            var request = new RestRequest(resource + $"/list/pagedIncludeOnly", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(pagedRequest);
            var response = client.Execute(request);
            return response;
        }

        public PagedResultModelPersonalDataModel PostPagedIncludedOnlyModel(PagedRequestModel pagedRequest) => JsonConvert.DeserializeObject<PagedResultModelPersonalDataModel>(PostPagedIncludedOnly(pagedRequest)?.Content);

        //POST /api/PersonalData/list/byRegistrationDate
        public IRestResponse PostListbyRegistrationDate(RegistrationDatesModel registrationDates)
        {
            var request = new RestRequest(resource + $"/list/byRegistrationDate", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(registrationDates);
            var response = client.Execute(request);
            return response;
        }

        public List<FullPersonalDataModel> PostListbyRegistrationDateModel(RegistrationDatesModel registrationDates) => JArray.Parse(PostListbyRegistrationDate(registrationDates)?.Content).ToObject<List<FullPersonalDataModel>>();

        //POST /api/PersonalData  Save personal info
        //no result model
        public IRestResponse PostPersonalData(FullPersonalDataModel fullPersonalDataModel)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(fullPersonalDataModel);
            var response = client.Execute(request);
            return response;
        }

        //POST /api/PersonalData/{id}/archive Delete item with id provided
        //no result model
        public IRestResponse PostPersonalDataArchive(ArchiveRequest archiveRequest)
        {
            var request = new RestRequest(resource + $"/{archiveRequest.ClientId}/archive", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(archiveRequest);
            var response = client.Execute(request);
            return response;
        }

        //POST /api/PersonalData/{id}/email  Changing email
        public IRestResponse PostPersonalDataChangeEmail(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/email", Method.POST);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        public ChangeFieldResponseModel PostPersonalDataChangeEmailModel(ChangeFieldRequest changeFieldRequest) => JsonConvert.DeserializeObject<ChangeFieldResponseModel>(PostPersonalDataChangeEmail(changeFieldRequest)?.Content);

        //POST /api/PersonalData/avatar/{id}  Add avatar
        public IRestResponse PostAddAvatar(string id, string filePath)
        {
            var request = new RestRequest(resource + $"/avatar/{id}", Method.POST);
            request.AddHeader("api-key", apiKey);
            byte[] x = File.ReadAllBytesAsync(filePath).Result;
            var content = new ByteArrayContent(x);
            request.AddObject(content);

            var response = client.Execute(request);
            return response;
        }
        #endregion

        #region PUT
        //PUT /api/PersonalData Update personal info
        public IRestResponse PutPersonalData(PersonalDataModel personalDataModel)
        {
            var request = new RestRequest(resource, Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(personalDataModel);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/fullname
        public IRestResponse PutPersonalDataFullName(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/fullname", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/firstName
        public IRestResponse PutPersonalDataFirstName(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/firstName", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            //StringContent content = new StringContent(JsonConvert.SerializeObject(changeFieldRequest));
            //var response = client.PutAsync(resource + $"/{changeFieldRequest.ClientId}/firstName", content);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/lastName
        public IRestResponse PutPersonalDataLastName(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/lastName", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/dateOfBirth  Change date of birth (MM/DD/YYYY)
        public IRestResponse PutPersonalDataDateOfBirth(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/dateOfBirth", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/DateOfExpiryOfID  Change date of expiry of ID birth (MM/DD/YYYY)
        public IRestResponse PutPersonalDataDateOfExpiryOfId(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/DateOfExpiryOfID", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/country  Change country
        public IRestResponse PutPersonalDataCountry(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/country", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/countryFromID  Change country from ID
        public IRestResponse PutPersonalDataCountryFromId(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/countryFromID", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/countryFromPOA  Change country from IP
        public IRestResponse PutPersonalDataCountryFromPOA(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/countryFromPOA", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/city  Change city
        public IRestResponse PutPersonalDataCity(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/city", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/zip  Change zip
        public IRestResponse PutPersonalDataZip(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/zip", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/address  Change address
        public IRestResponse PutPersonalDataAddress(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/address", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/phoneNumber  Change contact phone number
        public IRestResponse PutPersonalDataPhoneNumber(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/phoneNumber", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/geolocation  Change geolocation data
        public IRestResponse PutPersonalDataGeolocation(string id, ChangeGeolocationRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{id}/geolocation", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/passwordHint  Change password hint
        public IRestResponse PutPersonalDataPasswordHint(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/passwordHint", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/refCode  Set referral code
        public IRestResponse PutPersonalDataRefCode(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/refCode", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/spotRegulator  Change spot regulator
        public IRestResponse PutPersonalDataSpotRegulator(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/spotRegulator", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/marginRegulator  Change margin regulator
        public IRestResponse PutPersonalDataMarginRegulator(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/marginRegulator", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/{id}/paymentSystem  Change payment system
        public IRestResponse PutPersonalDataPaymentSystem(ChangeFieldRequest changeFieldRequest)
        {
            var request = new RestRequest(resource + $"/{changeFieldRequest.ClientId}/paymentSystem", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(changeFieldRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/profile  Update profile info
        public IRestResponse PutPersonalDataProfile(UpdateProfileInfoRequest updateProfileInfoRequest)
        {
            var request = new RestRequest(resource + $"/profile", Method.PUT);
            request.AddHeader("api-key", apiKey);
            request.AddJsonBody(updateProfileInfoRequest);
            var response = client.Execute(request);
            return response;
        }

        //PUT /api/PersonalData/documentscan/{id}  Add document scan
        public IRestResponse PutPersonalDataDocumentScan(string id, string filePath)
        {
            var request = new RestRequest(resource + $"/documentscan/{id}", Method.PUT);
            request.AddHeader("api-key", apiKey);
            byte[] x = File.ReadAllBytesAsync(filePath).Result;
            var content = new ByteArrayContent(x);

            request.AddJsonBody(content);
            var response = client.Execute(request);
            return response;
        }
        #endregion

        #region DELETE

        //DELETE /api/PersonalData/avatar/{id}  Delete avatar
        public IRestResponse DELETEPersonalDataAvatar(string id)
        {
            var request = new RestRequest(resource + $"/avatar/{id}", Method.DELETE);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        //DELETE /api/PersonalData/cache  Clears cache
        public IRestResponse DELETEPersonalDataCache(string id)
        {
            var request = new RestRequest(resource + $"/cache", Method.DELETE);
            request.AddHeader("api-key", apiKey);
            var response = client.Execute(request);
            return response;
        }

        
        #endregion
    }
}
