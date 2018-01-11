using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public partial class AuthenticateModel
    {
        public AuthenticateModel(AccountRegistrationModel account)
        {
            Email = account.Email;
            Password = account.Password;
            ClientInfo = account.ClientInfo;
            PartnerId = account.PartnerId;
        }
    }
}
