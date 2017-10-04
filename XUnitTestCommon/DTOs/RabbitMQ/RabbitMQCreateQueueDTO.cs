using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs.RabbitMQ
{
    public class RabbitMQCreateQueueDTO
    {
        public bool auto_delete { get; set; }
        public bool durable { get; set; }
        public Arguments arguments { get; set; }
        public string node { get; set; }

        public class Arguments
        {
        }
    }
}
