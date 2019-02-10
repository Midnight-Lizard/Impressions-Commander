using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.AddLike
{
    public class AddLikeRequest : DomainRequest
    {
        public string ObjectType { get; set; }
    }
}
