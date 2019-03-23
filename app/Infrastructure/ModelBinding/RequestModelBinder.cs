using Microsoft.AspNetCore.Mvc.ModelBinding;
using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;
using System;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding
{
    public class RequestModelBinder : IModelBinder
    {
        protected readonly IRequestMetaDeserializer requestSerializer;
        protected readonly RequestSchemaVersionAccessor requestSchemaVersionAccessor;

        public RequestModelBinder(IRequestMetaDeserializer requestSerializer,
            RequestSchemaVersionAccessor requestVersionAccessor)
        {
            this.requestSerializer = requestSerializer;
            this.requestSchemaVersionAccessor = requestVersionAccessor;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var schemaVersion = this.requestSchemaVersionAccessor.GetSchemaVersion(bindingContext);
                if (schemaVersion != SchemaVersion.Unspecified)
                {
                    var request = this.requestSerializer.Deserialize(bindingContext.ModelType, schemaVersion, bindingContext);

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
            return Task.CompletedTask;
        }
    }
}
