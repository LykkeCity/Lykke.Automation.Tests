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
}
