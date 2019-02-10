using FluentValidation;
using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.AddLike
{
    public class AddLikeRequestValidator : AbstractValidator<AddLikeRequest>
    {
        public AddLikeRequestValidator()
        {
            this.Include(new RequestValidator());
            this.RuleFor(x => x.ObjectType).NotEmpty().MaximumLength(50);
        }
    }
}
