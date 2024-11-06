using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Setting;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        // Constructor to inject the setting service
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for toggling maintenance mode, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPatch("MaintenanceMode")]
        public async Task<IActionResult> ToggleMaintenance()
        {
            // Call the service to toggle maintenance mode
            await _settingService.ToggleMaintenance();

            // Return success response
            return Ok();
        }

        // ----------------------------------------------------------------------------------------------
    }
}
