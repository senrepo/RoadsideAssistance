using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using Serilog;

namespace RoadsideAssistanceBL.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IRoadSideDataStore _dataStore;

        public CustomerService(IRoadSideDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await Task.Run(() =>
            {
                return _dataStore.GetCustomers();
            });
        }

        public async Task<bool> IsValidCustomer(int custId)
        {
            var list = await GetCustomers();
            return list.Any(x => x.Id == custId);
        }
        public async Task Save(Customer customer)
        {
            if (customer == null) return;

            await Task.Run(() =>
            {
                var list = _dataStore.GetCustomers();
                var dbCustomer = list.Where(x => x.Id == customer.Id).FirstOrDefault();
                //save logic
                if (dbCustomer != null)
                {
                    dbCustomer.Id = customer.Id;
                    dbCustomer.Name = customer.Name;
                    dbCustomer.PhoneNumber = customer.PhoneNumber;
                    dbCustomer.serviceRequest = customer.serviceRequest;
                    Log.Information($"Customer:{customer.Name} record updated in the database");
                }
            });
        }
    }
}
