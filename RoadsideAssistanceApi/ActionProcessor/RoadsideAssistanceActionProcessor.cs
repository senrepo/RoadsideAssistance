using RoadsideAssistanceApi.Dto;
using RoadsideAssistanceApi.Repository;
using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceApi.ActionProcessor
{
    public class RoadsideAssistanceActionProcessor : IRoadsideAssistanceActionProcessor
    {
        private readonly IRoadsideAssistanceRepository _roadSideRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAssistantRepository _assistantRepository;
        private readonly ILogger<RoadsideAssistanceActionProcessor> _logger;
        public RoadsideAssistanceActionProcessor(IRoadsideAssistanceRepository roadSideRepository,
                                                 ICustomerRepository customerRepository,
                                                 IAssistantRepository assistantRepository,
                                                 ILogger<RoadsideAssistanceActionProcessor> logger)
        {
            _roadSideRepository = roadSideRepository;
            _customerRepository = customerRepository;
            _assistantRepository = assistantRepository;
            _logger = logger;
        }

        public async Task<List<Assistant>> FindNearestAssistants(Geolocation location, int limit)
        {
            var response = await _roadSideRepository.FindNearestAssistants(location, limit);
            return response;
        }

        public async Task<Assistant?> ReserveAssistant(ReserveAssistantDto reserveAssistantDto)
        {
            Assistant? response = null;
            if (await _customerRepository.IsValidCustomer(reserveAssistantDto.Customer.Id))
            {
                response = await _roadSideRepository.ReserveAssistant(reserveAssistantDto.Customer, reserveAssistantDto.Geolocation);
            }
            return response;
        }

        public async Task ReleaseAssistant(ReleaseAssistantDto releaseAssistantDto)
        {
            if (await _customerRepository.IsValidCustomer(releaseAssistantDto.Customer.Id) &&
                await _assistantRepository.IsValidAssistant(releaseAssistantDto.Assistant.Id))
            {
                await _roadSideRepository.ReleaseAssistant(releaseAssistantDto.Customer, releaseAssistantDto.Assistant);
            }
        }

        public async Task UpdateAssistantLocation(UpdateAssistantLocationDto updateAssistantLocationDto)
        {
            if (await _assistantRepository.IsValidAssistant(updateAssistantLocationDto.Assistant.Id))
            {
                await _roadSideRepository.UpdateAssistantLocation(updateAssistantLocationDto.Assistant, updateAssistantLocationDto.Geolocation);
            }

        }
    }
}
