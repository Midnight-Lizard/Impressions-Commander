using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Requests.Common
{
    public class RequestValidator: AbstractValidator<DomainRequest>
    {
        public RequestValidator()
        {
            RuleFor(r => r.AggregateId).NotEmpty();
            RuleFor(r => r.Id).NotEmpty();
        }
    }
}
