using MidnightLizard.Impressions.Commander.Infrastructure.Serialization;

namespace MidnightLizard.Impressions.Commander.Requests.AddLike
{
    [SchemaVersion(">=1")]
    public class AddLikeRequestDeserializer_v1 : JsonRequestDeserializer<AddLikeRequest>
    {
    }
}
