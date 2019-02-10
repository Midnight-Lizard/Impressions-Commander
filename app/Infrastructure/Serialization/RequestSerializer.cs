using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Requests.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public interface IRequestSerializer
    {
        string Serialize(DomainRequest request, UserId userId);
    }

    public class RequestSerializer : IRequestSerializer
    {
        private readonly SchemaVersion version;
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            //Formatting = Formatting.Indented,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = MessageContractResolver.Default,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            Converters = { new StringEnumConverter(camelCaseText: true) }
        };

        public RequestSerializer(SchemaVersion version)
        {
            this.version = version;
        }

        public string Serialize(DomainRequest request, UserId userId)
        {
            return JsonConvert.SerializeObject(new
            {
                CorrelationId = request.Id,
                Type = request.GetType().Name,
                Version = this.version.ToString(),
                UserId = userId.Value,
                Payload = request
            }, serializerSettings);
        }
    }
}
