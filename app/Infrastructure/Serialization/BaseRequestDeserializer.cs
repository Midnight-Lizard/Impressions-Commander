using MidnightLizard.Impressions.Commander.Requests.Common;
using System;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public interface IRequestDeserializer { }
    public interface IRequestDeserializer<out TRequest> : IRequestDeserializer
        where TRequest : DomainRequest
    {
        TRequest Deserialize(string data);
    }

    public abstract class BaseRequestDeserializer<TRequest> : IRequestDeserializer<TRequest>
        where TRequest : DomainRequest
    {
        public virtual TRequest Deserialize(string requestData)
        {
            var request = this.DeserializeRequest(requestData);
            this.StartAdvancingToTheLatestVersion(request);
            request.Id = request.Id == default ? Guid.NewGuid() : request.Id;
            request.DeserializerType = this.GetType();
            return request;
        }

        protected abstract TRequest DeserializeRequest(string data);

        public virtual void StartAdvancingToTheLatestVersion(TRequest message) { }
        protected virtual void AdvanceToTheLatestVersion(TRequest request) { }
    }
}
