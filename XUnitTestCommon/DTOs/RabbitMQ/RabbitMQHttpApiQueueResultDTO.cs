using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.DTOs.RabbitMQ
{
    public class RabbitMQHttpApiQueueResultDTO
    {
        public MessagesDetails messages_details { get; set; }
        public int messages { get; set; }
        public MessagesUnacknowledgedDetails messages_unacknowledged_details { get; set; }
        public int messages_unacknowledged { get; set; }
        public MessagesReadyDetails messages_ready_details { get; set; }
        public int messages_ready { get; set; }
        public ReductionsDetails reductions_details { get; set; }
        public long reductions { get; set; }
        public string node { get; set; }
        public Arguments arguments { get; set; }
        public bool exclusive { get; set; }
        public bool auto_delete { get; set; }
        public bool durable { get; set; }
        public string vhost { get; set; }
        public string name { get; set; }
        public int message_bytes_paged_out { get; set; }
        public int messages_paged_out { get; set; }
        public BackingQueueStatus backing_queue_status { get; set; }
        public object head_message_timestamp { get; set; }
        public int message_bytes_persistent { get; set; }
        public int message_bytes_ram { get; set; }
        public int message_bytes_unacknowledged { get; set; }
        public int message_bytes_ready { get; set; }
        public int message_bytes { get; set; }
        public int messages_persistent { get; set; }
        public int messages_unacknowledged_ram { get; set; }
        public int messages_ready_ram { get; set; }
        public int messages_ram { get; set; }
        public GarbageCollection garbage_collection { get; set; }
        public string state { get; set; }
        public object recoverable_slaves { get; set; }
        public int consumers { get; set; }
        public object exclusive_consumer_tag { get; set; }
        public object policy { get; set; }
        public object consumer_utilisation { get; set; }
        public string idle_since { get; set; }
        public int memory { get; set; }

        public class MessagesDetails
        {
            public double rate { get; set; }
        }

        public class MessagesUnacknowledgedDetails
        {
            public double rate { get; set; }
        }

        public class MessagesReadyDetails
        {
            public double rate { get; set; }
        }

        public class ReductionsDetails
        {
            public double rate { get; set; }
        }

        public class Arguments
        {
        }

        public class BackingQueueStatus
        {
            public string mode { get; set; }
            public int q1 { get; set; }
            public int q2 { get; set; }
            public List<object> delta { get; set; }
            public int q3 { get; set; }
            public int q4 { get; set; }
            public int len { get; set; }
            public string target_ram_count { get; set; }
            public int next_seq_id { get; set; }
            public double avg_ingress_rate { get; set; }
            public double avg_egress_rate { get; set; }
            public double avg_ack_ingress_rate { get; set; }
            public double avg_ack_egress_rate { get; set; }
        }

        public class GarbageCollection
        {
            public int minor_gcs { get; set; }
            public int fullsweep_after { get; set; }
            public int min_heap_size { get; set; }
            public int min_bin_vheap_size { get; set; }
            public int max_heap_size { get; set; }
        }
    }
}
