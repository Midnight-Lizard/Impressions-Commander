using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.AddToFavorites
{
    public class AddToFavoritesRequest : DomainRequest
    {
        public string ObjectType { get; set; }
    }
}
