using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class ConstantsResponse
    {
        public Publicaddressextension publicAddressExtension { get; set; }
    }
    public class Publicaddressextension
    {
        public string separator { get; set; }
        public string displayName { get; set; }
    }
}
