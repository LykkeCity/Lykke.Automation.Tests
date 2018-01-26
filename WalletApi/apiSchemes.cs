using LykkeAutomation.Api.JsonSchemes.AccountExist;
using LykkeAutomation.Api.JsonSchemes.Auth;
using LykkeAutomation.Api.JsonSchemes.PersonalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LykkeAutomation.Api
{
    public class ApiSchemes
    {
        public PersonalDataSchemes PersonalDataSheme => new PersonalDataSchemes();
        public AuthScheme AuthScheme => new AuthScheme();
        public AccountExistSchemes AccountExistSchemes => new AccountExistSchemes();
    }
}
