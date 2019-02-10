using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MidnightLizard.Impressions.Commander.Controllers
{
    [ApiVersionNeutral]
    [Route("[controller]/[action]")]
    public class StatusController : Controller
    {
        public IActionResult IsReady()
        {
            return Ok("impressions commander is ready");
        }

        public IActionResult IsAlive()
        {
            return Ok("impressions commander is alive");
        }
    }
}
