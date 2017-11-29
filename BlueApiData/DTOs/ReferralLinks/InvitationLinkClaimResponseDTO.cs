using System;
using System.Collections.Generic;
using System.Text;

namespace BlueApiData.DTOs.ReferralLinks
{
    public class InvitationLinkClaimResponseDTO
    {
        public string TransactionRewardRecipient { get; set; }
        public string TransactionRewardSender { get; set; }
        public string SenderOffchainTransferId { get; set; }
    }
}
