using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;
using MidnightLizard.Impressions.Commander.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.AutofacModules
{
    public class SerializationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestMetaDeserializer>()
                .AsImplementedInterfaces();

            builder.RegisterType<RequestSerializer>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(SerializationModule).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestDeserializer<>))
                .As<IRequestDeserializer>()
                .WithMetadata(t => new Dictionary<string, object>
                {
                    [nameof(Type)] = t
                        .GetInterface(typeof(IRequestDeserializer<>).FullName)
                        .GetGenericArguments().First(),

                    [nameof(SchemaVersionAttribute.VersionRange)] = t
                        .GetCustomAttribute<SchemaVersionAttribute>()
                        .VersionRange
                });
        }
    }
}
