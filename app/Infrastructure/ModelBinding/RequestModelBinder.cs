using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding;
using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding
{
    public class RequestModelBinder : IModelBinder
    {
        protected readonly IRequestMetaDeserializer requestSerializer;
        protected readonly RequestSchemaVersionAccessor requestSchemaVersionAccessor;
        protected readonly RequestBodyAccessor requestBodyAccessor;

        public RequestModelBinder(
            IRequestMetaDeserializer requestSerializer,
            RequestSchemaVersionAccessor requestVersionAccessor,
            RequestBodyAccessor requestBodyAccessor)
        {
            this.requestBodyAccessor = requestBodyAccessor;
            this.requestSerializer = requestSerializer;
            this.requestSchemaVersionAccessor = requestVersionAccessor;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var schemaVersion = this.requestSchemaVersionAccessor.GetSchemaVersion(bindingContext);
                if (schemaVersion != SchemaVersion.Unspecified)
                {
                    string requestData = "";

                    if (bindingContext.BindingSource == BindingSource.Body)
                    {
                        requestData = await this.requestBodyAccessor.ReadAsync(bindingContext);
                    }
                    else
                    {
                        requestData = bindingContext.ValueProvider
                            .GetValue(bindingContext.BinderModelName).FirstValue;
                    }

                    var request = this.requestSerializer.Deserialize(bindingContext.ModelType, schemaVersion, requestData);

                    bindingContext.Result = ModelBindingResult.Success(request);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(this.requestSchemaVersionAccessor.VersionKey, "Schema version is required");
                }
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError(bindingContext.FieldName, ex.Message);
            }
        }
    }
}
