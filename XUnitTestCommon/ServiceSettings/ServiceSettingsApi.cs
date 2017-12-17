using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.ServiceSettings.SettingsModels;

namespace TestsCore.ServiceSettings
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
