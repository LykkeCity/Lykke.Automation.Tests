using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.TestsCore;
using LykkeAutomationPrivate.Models;
using LykkeAutomationPrivate.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestsCore.TestsData;
using static LykkeAutomationPrivate.Tests.Helpers;

namespace LykkeAutomationPrivate.Tests.PersonalData
{
    public class PersonalDataTests
    {

        public class PersonalDataBaseTest : BaseTest
        {
            [SetUp]
            public void BeforeTest()
            {
                var expectedVersion = Environment.GetEnvironmentVariable("ApiVersion");
                
                if (expectedVersion != null)
                {
                    var actual = lykkeApi.PersonalData.GetIsAlive();
                    if (actual.Version != expectedVersion)
                    {
                        Assert.Ignore($"actual service version:{actual.Version}  is not as expected: {expectedVersion}");
                        var oo = TestContext.CurrentContext;
                    }
                        
                }
            }
        }

        #region GET
        public class GetPersonalDataByEmail : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Get PersonalData By Email")]
            [Parallelizable]
            public void GetPersonalDataByEmailTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var test = lykkeApi.PersonalData.GetPersonalDataModel(client.Email);
                Assert.That(test.Email, Is.EqualTo(client.Email), "Email is not as expected");
            }
        }

        public class GetPersonalDataList: PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Parallelizable]
            public void GetPersonalDataListTest()
            {
                var list = lykkeApi.PersonalData.GetPersonalDataListModel();
                Assert.That(list.Count, Is.GreaterThan(0), "List count is not as expected");
            }
        }

        public class GetPersonalDataById : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Parallelizable]
            public void GetPersonalDataByIdTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var finded = lykkeApi.PersonalData.GetPersonalDataModelById(client.Id);
                Assert.That(client.Email, Is.EqualTo(finded.Email), "Finded email for personal data is  not equal");
                Assert.That(client.Id, Is.EqualTo(finded.Id), "Finded ID for personal data is  not equal");
                Assert.That(client.FirstName, Is.EqualTo(finded.FirstName), "Finded First Name for personal data is  not equal");
                Assert.That(client.FullName, Is.EqualTo(finded.FullName), "Finded First Name for personal data is  not equal");
                Assert.That(client.Country, Is.EqualTo(finded.Country), "Finded Country for personal data is  not equal");
                Assert.That(client.Regitered, Is.EqualTo(finded.Regitered), "Finded Registered for personal data is  not equal");
            }
        }

        public class GetFullPersonalDataById : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Parallelizable]
            public void GetFullPersonalDataByIdTest()
            {
                var first = lykkeApi.PersonalData.GetPersonalDataListModel()[0];
                var finded = lykkeApi.PersonalData.GetFullPersonalDataModelById(first.Id);
                Assert.That(first.Email, Is.EqualTo(finded.Email), "Finded email for personal data is  not equal");
                Assert.That(first.Id, Is.EqualTo(finded.Id), "Finded ID for personal data is  not equal");
                Assert.That(first.FirstName, Is.EqualTo(finded.FirstName), "Finded First Name for personal data is  not equal");
                Assert.That(finded.FullName, Is.Not.Null, "Finded Full Name for personal data is  not equal");
                Assert.That(first.Country, Is.EqualTo(finded.Country), "Finded Country for personal data is  not equal");
                //Assert.That(first.Regitered, Is.EqualTo(finded.Regitered), "Finded Registered for personal data is  not equal");
            }
        }

        public class GetProfilePersonalDataById : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Parallelizable]
            public void GetProfilePersonalDataByIdTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var finded = lykkeApi.PersonalData.GetProfilePersonalDataModelById(client.Id);
                Assert.That(client.Email, Is.EqualTo(finded.Email), "Finded email for personal data is  not equal");
                Assert.That(client.Address, Is.EqualTo(finded.Address), "Finded ID for personal data is  not equal");
                Assert.That(client.FirstName, Is.EqualTo(finded.FirstName), "Finded First Name for personal data is  not equal");
            }
        }

        public class SearchPersonalDataModel : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Search client by part of full name, email or contact phone")]
            [Parallelizable]
            public void SearchPersonalDataModelTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var findedByEmail = lykkeApi.PersonalData.GetSearchPersonalDataModel(client.Email.Substring(0, client.Email.Length-2));
                Assert.That(client.Id, Is.EqualTo(findedByEmail.Id), "Id are not equals");

                var findedByContactPhone = lykkeApi.PersonalData.GetSearchPersonalDataModel(client.ContactPhone.Substring(0, client.ContactPhone.Length - 2));
                Assert.That(client.Id, Is.EqualTo(findedByContactPhone.Id), "ContactPhone are not equals");
            }
        }

        #endregion

        #region Post
        public class PostPersonalDataList : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal data list by ids")]
            [Parallelizable]
            public void PostPersonalDataListTest()
            {
                
                var list = lykkeApi.PersonalData.GetPersonalDataListModel();
                Random r = new Random();
                int size = r.Next(1, 15);
                string[] array = new string[size];
                for (int i = 0; i < size; i++)
                    array[i] = list[i].Id;
                
                var postList = lykkeApi.PersonalData.PostPersonalDataListByIdModel(array);
                Assert.That(postList.Count, Is.EqualTo(size), "Personal Data list size is not as expected");
            }
        }

        public class FullPostPersonalDataList : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal Full data list by ids")]
            [Parallelizable]
            public void FullPostPersonalDataListTest()
            {

                var list = lykkeApi.PersonalData.GetPersonalDataListModel();
                Random r = new Random();
                int size = r.Next(1, 15);
                string[] array = new string[size];
                for (int i = 0; i < size; i++)
                    array[i] = list[i].Id;

                var postList = lykkeApi.PersonalData.PostFullPersonalDataListByIdModel(array);
                Assert.That(postList.Count, Is.EqualTo(size), "Personal Data list size is not as expected");
            }
        }

        public class PostListExcludedPage : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal Full data list by ids")]
            public void PostListExcludedPageTest()
            {
                int pagesNumber = 10;
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var page = new PagingInfoModel(pagesNumber);
                var pageModel = lykkeApi.PersonalData.PostPageModel(page);

                var pagedRequestModel = new PagedRequestModel(new List<string>() { pageModel.PagingInfo.NextPage }, pagesNumber);
                var excludedPageModel = lykkeApi.PersonalData.PostPageExcludeModel(pagedRequestModel);
                Assert.That(excludedPageModel.Result.Count, Is.EqualTo(pagesNumber), "Unexpected result count");
            }
        }

        public class PostListPage : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal Full data list by ids")]
            public void PostListPageTest()
            {
                int pagesNumber = 10;
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var page = new PagingInfoModel(pagesNumber);
                var pageModel = lykkeApi.PersonalData.PostPageModel(page);
                Assert.That(pageModel.Result.Count, Is.EqualTo(pagesNumber), "Unexpected result count");
            }
        }

        public class PostPagedIncludedOnly : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal Full data list by ids")]
            public void PostPagedIncludedOnlyTest()
            {
                int pagesNumber = 10;
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var page = new PagingInfoModel(pagesNumber);
                var pageModel = lykkeApi.PersonalData.PostPageModel(page);

                var pagedRequestModel = new PagedRequestModel(new List<string>() { client.Id }, pagesNumber);
                var includedPageModel = lykkeApi.PersonalData.PostPagedIncludedOnlyModel(pagedRequestModel);
                Assert.That(includedPageModel.Result.Count, Is.EqualTo(1), "Unexpected result count");
                AreEqualByJson(client.PersonalDataModel(), includedPageModel.Result[0]);
            }
        }

        public class PostListbyRegistrationDate : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Get Personal Full data list by registration date")]
            public void PostListbyRegistrationDateTest()
            {
                RegistrationDatesModel registrationDates = new RegistrationDatesModel();
                registrationDates.DateFrom = DateTime.Now.AddMonths(-5).ToUniversalTime().Date;
                registrationDates.DateTo = DateTime.Now.ToUniversalTime().Date;

                var list = lykkeApi.PersonalData.PostListbyRegistrationDateModel(registrationDates);

                list.ForEach(l => Assert.That(l.Regitered.Value.Date, Is.LessThanOrEqualTo(registrationDates.DateTo).And.GreaterThanOrEqualTo(registrationDates.DateFrom), "Unexpected registered date"));
            }
        }

        public class PostPersonalDataArchive : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Archive personal data")]
            public void PostPersonalDataArchiveTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");
                var actual = lykkeApi.PersonalData.GetFullPersonalDataById(client.Id);
                Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                ArchiveRequest archive = new ArchiveRequest(client.Id);
                var archiveResponse = lykkeApi.PersonalData.PostPersonalDataArchive(archive);
                Assert.That(archiveResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));//temp, discover After issue
                var actualArchive = lykkeApi.PersonalData.GetFullPersonalDataById(client.Id);
                
                Assert.That(() => actualArchive.StatusCode, Is.EqualTo(HttpStatusCode.NoContent).After(5*1000, 1*1000), "Unexpected status code");
            }
        }

        public class PostPersonalDataChangeEmail : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Change email of personal data")]
            public void PostPersonalDataChangeEmailTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.Email = TestData.GenerateEmail();
                var cfr = new ChangeFieldRequest(client.Email, client.Id);
                var actualResponse = lykkeApi.PersonalData.PostPersonalDataChangeEmailModel(cfr);
                
                Assert.That(actualResponse.ErrorMessage, Is.Null, "Unexpected Error message");

                var actual = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.Email, Is.EqualTo(actual.Email), "Email has not been changed");
            }
        }

        public class PostAddAvatar : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Add avatar")]
            public void PostAddAvatarTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var actual = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(actual.Avatar, Is.Null, "Avatar is not null");

                var avatarUpload = lykkeApi.PersonalData.PostAddAvatar(client.Id, TestData.AVATAR);
                Assert.That(avatarUpload.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected Status Code");

                var actualAfterUpload = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(actualAfterUpload.Avatar, Is.Not.Null, "Avatar is null"); //fail. bug?
            }
        }

        public class PostPersonalDataCreate : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. save personal data")]
            public void PostPersonalDataCreateTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var actual = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);

                AreEqualByJson(client, actual);
            }
        }

        #endregion

        #region PUT
        //put
        public class PutPersonalData : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. save personal data")]
            public void PutPersonalDataTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.Address = TestData.GenerateString(6);
                personalData.Email = TestData.GenerateEmail();
                var putResponse = lykkeApi.PersonalData.PutPersonalData(personalData);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                AreEqualByJson(personalData, updatedClient);
            }
        }

        public class PutPersonalDataFullName : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataFullNameTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                var fullName = TestData.GenerateString(6);

                var changeRequest = new ChangeFieldRequest(fullName, personalData.Id);
                var putResponse = lykkeApi.PersonalData.PutPersonalDataFullName(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(fullName, Is.EqualTo(updatedClient.FullName), "Unexpected Full name");
            }
        }

        public class PutPersonalDataFirstName : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataFirstNameTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                var firstName = TestData.GenerateString(6);

                var changeRequest = new ChangeFieldRequest(firstName, personalData.Id);
                var putResponse = lykkeApi.PersonalData.PutPersonalDataFirstName(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(firstName, Is.EqualTo(updatedClient.FirstName), "Unexpected First name");
            }
        }

        public class PutPersonalDataLastName : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataLastNameTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                var lastName = TestData.GenerateString(6);

                var changeRequest = new ChangeFieldRequest(lastName, personalData.Id);
                var putResponse = lykkeApi.PersonalData.PutPersonalDataLastName(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(lastName, Is.EqualTo(updatedClient.LastName), "Unexpected Last name");
            }
        }

        public class PutPersonalDataDateOfBirth : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataDateOfBirthTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                var newDoB = DateTime.Now.AddDays(-1).AddMonths(-2).AddYears(-3).ToUniversalTime().ToString("MM/dd/yyyy");

                var changeRequest = new ChangeFieldRequest(newDoB, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataDateOfBirth(changeRequest);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(newDoB, Is.EqualTo(updatedClient.DateOfBirth.Value.ToString("MM/dd/yyyy")), "Unexpected DateOfBirth");
            }
        }

        public class PutPersonalDataDateOfExpiryOfId : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataDateOfExpiryOfIdTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                var newTime = personalData.DateOfExpiryOfID.Value.AddDays(-1).AddMonths(-2).AddYears(-3).ToUniversalTime().ToString("MM/dd/yyyy");
                var changeRequest = new ChangeFieldRequest(newTime, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataDateOfExpiryOfId(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(newTime, Is.EqualTo(updatedClient.DateOfExpiryOfID.Value.Date.ToString("MM/dd/yyyy")), "Unexpected DateOfBirth");
            }
        }

        public class PutPersonalDataCountry : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataCountryTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.Country = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.Country, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataCountry(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.Country, Is.EqualTo(updatedClient.Country), "Unexpected Country");
            }
        }

        public class PutPersonalDataCountryFromId : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataCountryFromIdTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.CountryFromID = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.CountryFromID, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataCountryFromId(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.CountryFromID, Is.EqualTo(updatedClient.CountryFromID), "Unexpected CountryFromID");
            }
        }

        public class PutPersonalDataCountryFromPOA : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataCountryFromPOATest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.CountryFromPOA = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.CountryFromPOA, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataCountryFromPOA(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.CountryFromPOA, Is.EqualTo(updatedClient.CountryFromPOA), "Unexpected CountryFromPOA");
            }
        }

        public class PutPersonalDataCity : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataCityTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.City = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.City, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataCity(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.City, Is.EqualTo(updatedClient.City), "Unexpected CountryFromPOA");
            }
        }

        public class PutPersonalDataZip : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataZipTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.Zip = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.Zip, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataZip(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.Zip, Is.EqualTo(updatedClient.Zip), "Unexpected Zip");
            }
        }

        public class PutPersonalDataAddress : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataAddressTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.Address = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.Address, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataAddress(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.Address, Is.EqualTo(updatedClient.Address), "Unexpected Address");
            }
        }

        public class PutPersonalDataPhoneNumber : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataPhoneNumberTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var personalData = client.PersonalDataModel();
                personalData.ContactPhone = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(personalData.ContactPhone, personalData.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataPhoneNumber(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetPersonalDataModelById(personalData.Id);
                Assert.That(personalData.ContactPhone, Is.EqualTo(updatedClient.ContactPhone), "Unexpected ContactPhone");
            }
        }

        public class PutPersonalDataGeolocation : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataGeolocationTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var countryCode = TestData.GenerateString(8);
                var city = TestData.GenerateString(8);
                var isp = TestData.GenerateString(8);

                var changeRequest = new ChangeGeolocationRequest(countryCode, city, isp);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataGeolocation(client.Id, changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(city, Is.EqualTo(updatedClient.City), "Unexpected City");
                Assert.That(countryCode, Is.EqualTo(updatedClient.Country), "Unexpected Country");
            }
        }

        public class PutPersonalDataPasswordHint : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataPasswordHintTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.PasswordHint = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(client.PasswordHint, client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataPasswordHint(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.PasswordHint, Is.EqualTo(updatedClient.PasswordHint), "Unexpected PasswordHint");
            }
        }

        public class PutPersonalDataRefCode : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataRefCodeTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.ReferralCode = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(client.ReferralCode, client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataRefCode(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.ReferralCode, Is.EqualTo(updatedClient.ReferralCode), "Unexpected ReferralCode");
            }
        }

        public class PutPersonalDataSpotRegulator : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataSpotRegulatorTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.SpotRegulator = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(client.SpotRegulator, client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataSpotRegulator(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.SpotRegulator, Is.EqualTo(updatedClient.SpotRegulator), "Unexpected SpotRegulator");
            }
        }

        public class PutPersonalDataMarginRegulator : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataMarginRegulatorTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.MarginRegulator = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(client.MarginRegulator, client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataMarginRegulator(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.MarginRegulator, Is.EqualTo(updatedClient.MarginRegulator), "Unexpected MarginRegulator");
            }
        }

        public class PutPersonalDataPaymentSystem : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataPaymentSystemTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                client.PaymentSystem = TestData.GenerateString(8);
                var changeRequest = new ChangeFieldRequest(client.PaymentSystem, client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataPaymentSystem(changeRequest);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(client.PaymentSystem, Is.EqualTo(updatedClient.PaymentSystem), "Unexpected PaymentSystem");
            }
        }

        public class PutPersonalDataProfile : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataProfileTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var updatedProfile = new UpdateProfileInfoRequest(client.Id);

                var putResponse = lykkeApi.PersonalData.PutPersonalDataProfile(updatedProfile);

                var updatedClient = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);

                Assert.That(updatedProfile.Address, Is.EqualTo(updatedClient.Address), "Unexpected Address");
                Assert.That(updatedProfile.Email, Is.EqualTo(updatedClient.Email), "Unexpected Email");
                Assert.That(updatedProfile.Facebook, Is.EqualTo(updatedClient.Facebook), "Unexpected Facebook");
                Assert.That(updatedProfile.Github, Is.EqualTo(updatedClient.Github), "Unexpected Github");
                Assert.That(updatedProfile.Twitter, Is.EqualTo(updatedClient.Twitter), "Unexpected Twitter");
                Assert.That(updatedProfile.Website, Is.EqualTo(updatedClient.Website), "Unexpected Website");
                Assert.That(updatedProfile.ClientId, Is.EqualTo(updatedClient.Id), "Unexpected ClientId");
            }
        }

        public class PutPersonalDataDocumentScan : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("")]
            public void PutPersonalDataDocumentScanTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var result = lykkeApi.PersonalData.PutPersonalDataDocumentScan(client.Id, TestData.AVATAR);
                Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected status code");

                var document = lykkeApi.PersonalData.GetDocumentScanByIdModel(client.Id);
                Assert.That(document.ToLower(), Does.Contain("png"), "Response content does not contain png");
            }
        }
        #endregion

        #region DELETE
        public class DELETEPersonalDataAvatar : PersonalDataBaseTest
        {
            [Test]
            [Category("PersonalDataService"), Category("ServiceAll")]
            [Description("Post request. Add avatar")]
            public void DELETEPersonalDataAvatarTest()
            {
                var client = new FullPersonalDataModel().Init();
                var response = lykkeApi.PersonalData.PostPersonalData(client);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "UnExpected status code");

                var actual = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(actual.Avatar, Is.Null, "Avatar is not null");

                var avatarUpload = lykkeApi.PersonalData.PostAddAvatar(client.Id, TestData.AVATAR);
                Assert.That(avatarUpload.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Unexpected Status Code");

                var actualAfterUpload = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(actualAfterUpload.Avatar, Is.Not.Null, "Avatar is null"); //fail. bug?

                var deleteAvatar = lykkeApi.PersonalData.DELETEPersonalDataAvatar(client.Id);

                var actualAfterDelete = lykkeApi.PersonalData.GetFullPersonalDataModelById(client.Id);
                Assert.That(actualAfterDelete.Avatar, Is.Null, "Avatar is not null");
            }
        }
        #endregion
    }
}
