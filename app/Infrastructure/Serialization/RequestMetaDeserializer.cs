﻿using Autofac.Features.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MidnightLizard.Impressions.Commander.Requests.Common;
using SemVer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Serialization
{
    public interface IRequestMetaDeserializer
    {
        DomainRequest Deserialize(Type requestType, SchemaVersion schemaVersion, ModelBindingContext bindingContext);
    }

    public class RequestMetaDeserializer : IRequestMetaDeserializer
    {
        protected readonly IEnumerable<Meta<Lazy<IRequestDeserializer>>> deserializers;

        public RequestMetaDeserializer(IEnumerable<Meta<Lazy<IRequestDeserializer>>> deserializers)
        {
            this.deserializers = deserializers;
        }

        public virtual DomainRequest Deserialize(Type requestType, SchemaVersion schemaVersion, ModelBindingContext bindingContext)
        {
            var deserializer = this.deserializers.FirstOrDefault(d =>
                d.Metadata[nameof(Type)] as Type == requestType &&
                (d.Metadata[nameof(SchemaVersionAttribute.VersionRange)] as Range).IsSatisfied(schemaVersion.Value));
            if (deserializer != null)
            {
                return (deserializer.Value.Value as IRequestDeserializer<DomainRequest>).Deserialize(bindingContext);
            }
            throw new ApplicationException($"Deserializer for {requestType} and schema version {schemaVersion} has not been found");
        }
    }
}
