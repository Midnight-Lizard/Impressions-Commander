using Microsoft.AspNetCore.Mvc.ModelBinding;
using MidnightLizard.Impressions.Commander.Requests.Common;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public abstract class JsonRequestDeserializer<TRequest> : BaseRequestDeserializer<TRequest>
        where TRequest : DomainRequest
    {
        protected override TRequest DeserializeRequest(ModelBindingContext bindingContext)
        {
            using (var bodyReader = new StreamReader(bindingContext.HttpContext.Request.Body))
            using (var bodyJsonReader = new JsonTextReader(bodyReader))
            {
                var serializer = new JsonSerializer();

                return serializer.Deserialize<TRequest>(bodyJsonReader);
            }
        }
    }
}
