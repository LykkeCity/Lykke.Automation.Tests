using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoStoreData.DTOs
{
    class ClientAccount
    {
        public DateTime Registered { get; set; }
        public String Id { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Pin { get; set; }
        public String NotificationsId { get; set; }
        public String PartnerId { get; set; }
        public bool IsReviewAccount { get; set; }
        public bool IsTrusted { get; set; }
    }
}
