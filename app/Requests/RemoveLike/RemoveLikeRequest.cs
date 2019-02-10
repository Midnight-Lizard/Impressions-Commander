using MidnightLizard.Impressions.Commander.Requests.Common;

namespace MidnightLizard.Impressions.Commander.Requests.RemoveLike
{
    public class RemoveLikeRequest : DomainRequest
    {
        public string ObjectType { get; set; }
    }
}
