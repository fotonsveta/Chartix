using System.Threading.Tasks;
using Chartix.Infrastructure.Telegram.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Chartix.Web.Controllers
{
    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private static int _currentUpdateId;

        private readonly IBotUpdateService _updateService;

        public UpdateController(IBotUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            // While debug one request, the same request may come again
#if DEBUG
            if (_currentUpdateId == update.Id)
            {
                return Ok();
            }
#pragma warning disable S2696 // Instance members should not write to "static" fields
            _currentUpdateId = update.Id;
#pragma warning restore S2696 // Instance members should not write to "static" fields
#endif

            await _updateService.HandleAsync(update);
            return Ok();
        }
    }
}
