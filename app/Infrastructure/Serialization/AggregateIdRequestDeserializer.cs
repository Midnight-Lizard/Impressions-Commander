using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Requests.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public class AggregateIdRequestDeserializer<TRequest> : BaseRequestDeserializer<TRequest>
        where TRequest : DomainRequest, new()
    {
        protected override TRequest DeserializeRequest(string aggregateId)
        {
            return new TRequest
            {
                AggregateId = Guid.Parse(aggregateId)
            };
        }
    }
}
