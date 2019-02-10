using FluentValidation;
using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.AddToFavorites
{
    public class AddToFavoritesRequestValidator : AbstractValidator<AddToFavoritesRequest>
    {
        public AddToFavoritesRequestValidator()
        {
            this.Include(new RequestValidator());
            this.RuleFor(x => x.ObjectType).NotEmpty().MaximumLength(50);
        }
    }
}
