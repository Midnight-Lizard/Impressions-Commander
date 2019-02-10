using FluentValidation;
using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.RemoveLike
{
    public class RemoveLikeRequestValidator : AbstractValidator<RemoveLikeRequest>
    {
        public RemoveLikeRequestValidator()
        {
            this.Include(new RequestValidator());
            this.RuleFor(x => x.ObjectType).NotEmpty().MaximumLength(50);
        }
    }
}
