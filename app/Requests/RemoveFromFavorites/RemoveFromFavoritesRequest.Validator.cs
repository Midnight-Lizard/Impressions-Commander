using FluentValidation;
using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.RemoveFromFavorites
{
    public class RemoveFromFavoritesRequestValidator : AbstractValidator<RemoveFromFavoritesRequest>
    {
        public RemoveFromFavoritesRequestValidator()
        {
            this.Include(new RequestValidator());
            this.RuleFor(x => x.ObjectType).NotEmpty().MaximumLength(50);
        }
    }
}
