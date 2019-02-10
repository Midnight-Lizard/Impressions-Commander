using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Queue
{
    public abstract class QueueConfig
    {
        public string TopicName { get; set; }
        public Dictionary<string, object> ProducerSettings { get; set; }
    }

    public class LIKES_QUEUE_CONFIG : QueueConfig { }

    public class FAVORITES_QUEUE_CONFIG : QueueConfig { }
}
