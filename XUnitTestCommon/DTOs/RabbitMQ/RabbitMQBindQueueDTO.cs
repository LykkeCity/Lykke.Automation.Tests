using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs.RabbitMQ
{
    class RabbitMQBindQueueDTO
    {
        public string routing_key { get; set; }
        public Arguments arguments { get; set; }

        public class Arguments
        {
        }
    }
}
