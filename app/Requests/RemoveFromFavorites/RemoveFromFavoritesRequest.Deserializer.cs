using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;

namespace MidnightLizard.Impressions.Commander.Requests.RemoveFromFavorites
{
    [SchemaVersion(">=1")]
    public class RemoveFromFavoritesRequestDeserializer_v1 : JsonRequestDeserializer<RemoveFromFavoritesRequest>
    {
    }
}
