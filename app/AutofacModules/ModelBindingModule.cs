using Autofac;
using MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.AutofacModules
{
    public class ModelBindingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestSchemaVersionAccessor>().AsSelf();
        }
    }
}
