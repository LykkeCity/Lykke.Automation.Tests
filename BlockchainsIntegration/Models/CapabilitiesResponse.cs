using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Client.AutorestClient.Models
{
    public class CapabilitiesResponse
    {
        public bool isTransactionsRebuildingSupported { get; set; }
        public bool areManyInputsSupported { get; set; }
        public bool areManyOutputsSupported { get; set; }
    }
}
