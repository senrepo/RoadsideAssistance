using RoadsideAssistanceBL.Models;
using RoadsideAssistanceBL.Service;

namespace RoadsideAssistanceApi.Repository
{
    public class RoadsideAssistanceRepository : IRoadsideAssistanceRepository
    {
        private readonly IRoadsideAssistanceService _service;
        private readonly ILogger<RoadsideAssistanceRepository> _logger;
        public RoadsideAssistanceRepository(ILogger<RoadsideAssistanceRepository> logger, IRoadsideAssistanceService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<List<Assistant>> FindNearestAssistants(Geolocation location, int limit)
        {
           return await _service.FindNearestAssistants(location, limit);
        }

        public async Task<Assistant?> ReserveAssistant(Customer customer, Geolocation location)
        {
            return await _service.ReserveAssistant(customer, location);
        }

        public async Task ReleaseAssistant(Customer customer, Assistant assistant)
        {
            await _service.ReleaseAssistant(customer, assistant);
        }

        public async Task UpdateAssistantLocation(Assistant assistant, Geolocation location)
        {
            await _service.UpdateAssistantLocation(assistant, location);
        }
    }
}
