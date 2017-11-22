﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs.ReferralLinks
{
    public class InvitationLinkClaimDTO
    {
        public string RecipientClientId { get; set; }
        public string ReferalLinkId { get; set; }
        public string ReferalLinkUrl { get; set; }
        public bool IsNewClient { get; set; }
    }
}
