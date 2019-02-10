using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding
{
    public class RequestSchemaVersionAccessor
    {
        public string VersionKey { get; } = "schema-version";

        public virtual SchemaVersion GetSchemaVersion(ModelBindingContext bindingContext)
        {
            var versionValue = string.Empty;
            if (bindingContext.HttpContext.Request.Query.Keys.Contains(VersionKey))
            {
                versionValue = bindingContext.HttpContext.Request.Query[VersionKey];
            }
            if (versionValue == string.Empty &&
                bindingContext.HttpContext.Request.Headers.Keys.Contains(VersionKey))
            {
                versionValue = bindingContext.HttpContext.Request.Headers[VersionKey];
            }
            if (versionValue != string.Empty)
            {
                return new SchemaVersion(versionValue);
            }
            return SchemaVersion.Unspecified;
        }
    }
}
