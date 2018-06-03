using LykkeAutomationPrivate.Api.PersonalDataResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.ServiceSettings;
using LykkeAutomationPrivate.Resources.RegistrationResourse;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using PrivateServices.Resources.CandlesHistory;

namespace LykkeAutomationPrivate.Api
{
    public class LykkeApi
    {
        public ServiceSettingsApi settings => new ServiceSettingsApi();
        public PersonalData PersonalData => new PersonalData();
        public Registration Registration => new Registration();
        public ClientAccountBase ClientAccount => new ClientAccountBase();
        public CandlesHistory CandleHistory => new CandlesHistory();
    }
}
