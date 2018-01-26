using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LykkeAutomationPrivate.Models.ClientAccount.Models
{
    public partial class Partner : IEquatable<Partner>
    {
        public bool Equals(Partner other)
        {
            return this.PublicId == other.PublicId && this.InternalId == other.InternalId &&
                this.Name == other.Name && this.AssetPrefix == other.AssetPrefix &&
                this.RegisteredUsersCount == other.RegisteredUsersCount;
        }

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

    public partial class ClientAccountInformation : IEquatable<ClientAccountInformation>,
                                                    IEquatable<ClientRegistrationModel>
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

        public bool Equals(ClientRegistrationModel other)
        {
            return this.Email == other.Email &&
                   this.Phone == other.Phone &&
                   this.PartnerId == other.PartnerId;
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