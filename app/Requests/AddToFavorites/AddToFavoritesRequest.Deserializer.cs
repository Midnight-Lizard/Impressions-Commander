using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;

namespace MidnightLizard.Impressions.Commander.Requests.AddToFavorites
{
    [SchemaVersion(">=1")]
    public class AddToFavoritesRequestDeserializer_v1 : JsonRequestDeserializer<AddToFavoritesRequest>
    {
    }
}
