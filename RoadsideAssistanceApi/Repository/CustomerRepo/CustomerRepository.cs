using RoadsideAssistanceBL.Service;

namespace RoadsideAssistanceApi.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomerRepository> _logger;
        public CustomerRepository(ILogger<CustomerRepository> logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task<bool> IsValidCustomer(int custId)
        {
            return await _service.IsValidCustomer(custId);
        }
    }
}
