using Microsoft.AspNetCore.Mvc.ModelBinding;
using MidnightLizard.Impressions.Commander.Requests.Common;
using System;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public class AggregateIdRequestDeserializer<TRequest> : BaseRequestDeserializer<TRequest>
        where TRequest : DomainRequest, new()
    {
        protected override TRequest DeserializeRequest(ModelBindingContext bindingContext)
        {
            var aggId = Guid.Parse(bindingContext.ValueProvider
                            .GetValue(bindingContext.BinderModelName)
                            .FirstValue);
            return new TRequest
            {
                AggregateId = aggId
            };
        }
    }
}
