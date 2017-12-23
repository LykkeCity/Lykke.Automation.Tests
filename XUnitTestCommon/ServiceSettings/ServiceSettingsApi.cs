using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.ServiceSettings.SettingsModels;

namespace XUnitTestCommon.ServiceSettings
{
    public class ServiceSettingsApi
    {
        private ServiceSettingsProvider serviceSettingsProvider;

        public ServiceSettingsApi()
        {
            serviceSettingsProvider = new ServiceSettingsProvider();
        }

        public PersonalDataServiceModel PersonalDataSettings()
        {
            PersonalDataServiceModel settings = new PersonalDataServiceModel();
            serviceSettingsProvider.ServiceSettings(SettingsTokenContainer.PersonalDataToken, ref settings);
            return settings;
        }
    }
}
