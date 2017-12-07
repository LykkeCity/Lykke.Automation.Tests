using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs.ReferralLinks
{
    public class RefLinksStatisticsDTO
    {
        public int NumberOfInvitationLinksSent { get; set; }
        public int NumberOfInvitationLinksAccepted { get; set; }
        public int NumberOfGiftLinksSent { get; set; }
        public double AmountOfGiftCoinsDistributed { get; set; }
        public int NumberOfNewUsersBroughtIn { get; set; }
    }
}
