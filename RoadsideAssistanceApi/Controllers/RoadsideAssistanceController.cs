using Microsoft.AspNetCore.Mvc;
using RoadsideAssistanceApi.ActionProcessor;
using RoadsideAssistanceApi.Dto;
using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceApi.Controllers
{
    [Route("api/roadsideAssistance")]
    [ApiController]
    public class RoadsideAssistanceController : ControllerBase
    {
        private readonly ILogger<RoadsideAssistanceController> _logger;
        private readonly IRoadsideAssistanceActionProcessor _processor;

        public RoadsideAssistanceController(IRoadsideAssistanceActionProcessor procesor, ILogger<RoadsideAssistanceController> logger)
        {
            _processor = procesor ?? throw new ArgumentNullException(nameof(procesor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("findNearestAssistants/{limit}")]
        public async Task<ActionResult> FindNearestAssistants(Geolocation locationDto, int limit)
        {
            var response = await _processor.FindNearestAssistants(locationDto, limit);
            return Ok(response);
        }

        [HttpPost("reserveAssistant")]
        public async Task<ActionResult> ReserveAssistant(ReserveAssistantDto reserveAssistantDto)
        {
            var response = await _processor.ReserveAssistant(reserveAssistantDto);
            return Ok(response);
        }


        [HttpPut("releaseAssistant")]
        public async Task<ActionResult> ReleaseAssistant(ReleaseAssistantDto releaseAssistantDto)
        {
            await _processor.ReleaseAssistant(releaseAssistantDto);
            return NoContent();
        }

        [HttpPut("updateAssistantLocation")]
        public async Task<ActionResult> UpdateAssistantLocation(UpdateAssistantLocationDto updateAssistantLocationDto)
        {
            return NoContent();
        }
    }
}
