using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Requests.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public abstract class JsonRequestDeserializer<TRequest> : BaseRequestDeserializer<TRequest>
        where TRequest : DomainRequest
    {
        protected override TRequest DeserializeRequest(string requestJson)
        {
            return JsonConvert.DeserializeObject<TRequest>(requestJson);
        }
    }
}
