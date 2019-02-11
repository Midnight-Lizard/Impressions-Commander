using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MidnightLizard.Impressions.Commander.Infrastructure.ActionFilters;
using MidnightLizard.Impressions.Commander.Infrastructure.Authentication;
using MidnightLizard.Impressions.Commander.Infrastructure.Queue;
using MidnightLizard.Impressions.Commander.Requests.AddToFavorites;
using MidnightLizard.Impressions.Commander.Requests.RemoveFromFavorites;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Controllers
{
    [Authorize]
    [ValidateModelState]
    [ApiVersion("1.0")]
    [Route("[controller]/[action]")]
    public class FavoritesController : Controller
    {
        protected readonly ILogger logger;
        protected readonly IRequestQueuer<FAVORITES_QUEUE_CONFIG> requestQueuer;

        protected UserId GetUserId()
        {
            if (this.User != null)
            {
                var subClaim = this.User.FindFirst("sub");
                if (subClaim != null)
                {
                    return new UserId(subClaim.Value);
                }
            }
            throw new UnauthorizedAccessException();
        }

        public FavoritesController(
            ILogger<FavoritesController> logger,
            IRequestQueuer<FAVORITES_QUEUE_CONFIG> requestQueuer)
        {
            this.logger = logger;
            this.requestQueuer = requestQueuer;
        }

        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddToFavoritesRequest request)
        {
            await this.requestQueuer.QueueRequest(request, this.GetUserId());
            return this.Accepted(request.Id);
        }

        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Remove([FromBody] RemoveFromFavoritesRequest request)
        {
            await this.requestQueuer.QueueRequest(request, this.GetUserId());
            return this.Accepted(request.Id);
        }
    }
}
