using RoadsideAssistanceBL.Service;

namespace RoadsideAssistanceApi.Repository
{
    public class AssistantRepository : IAssistantRepository
    {
        private readonly IAssistantService _service;
        private readonly ILogger<AssistantRepository> _logger;
        public AssistantRepository(ILogger<AssistantRepository> logger, IAssistantService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<bool> IsValidAssistant(int assistantId)
        {
            return await _service.IsValidAssistant(assistantId);
        }
    }
}
