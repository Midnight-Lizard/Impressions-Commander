using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.RemoveFromFavorites
{
    public class RemoveFromFavoritesRequest : DomainRequest
    {
        public string ObjectType { get; set; }
    }
}
