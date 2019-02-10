using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.ModelBinding
{
    public class RequestBodyAccessor
    {
        public virtual async Task<string> ReadAsync(ModelBindingContext bindingContext)
        {
            using (var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                return await streamReader.ReadToEndAsync();
            }
        }
    }
}