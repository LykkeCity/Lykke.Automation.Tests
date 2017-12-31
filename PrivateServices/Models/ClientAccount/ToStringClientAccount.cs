using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LykkeAutomationPrivate.Models.ClientAccount.Models
{
    public partial class Partner
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public partial class BannedClient
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public partial class ClientAccountInformation : IEquatable<ClientAccountInformation>
    {
        public bool Equals(ClientAccountInformation other)
        {
            return this.Registered == other.Registered &&
                   this.Id == other.Id &&
                   this.Email == other.Email &&
                   this.Phone == other.Phone &&
                   this.Pin == other.Pin &&
                   this.NotificationsId == other.NotificationsId &&
                   this.PartnerId == other.PartnerId &&
                   this.IsReviewAccount == other.IsReviewAccount &&
                   this.IsTrusted == other.IsTrusted;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public partial class ClientResponseModel : IEquatable<ClientAccountInformation>
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public bool Equals(ClientAccountInformation other)
        {
            return this.Id == other.Id &&
                   this.Email == other.Email &&
                   this.PartnerId == other.PartnerId &&
                   this.Phone == other.Phone &&
                   this.Pin == other.Pin &&
                   this.NotificationsId == other.NotificationsId &&
                   this.Registered == other.Registered &&
                   this.IsReviewAccount == other.IsReviewAccount;
        }
    }
}