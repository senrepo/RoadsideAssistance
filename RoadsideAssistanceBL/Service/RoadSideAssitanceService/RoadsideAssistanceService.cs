using RoadsideAssistanceBL.Models;
using Serilog;

namespace RoadsideAssistanceBL.Service
{
    public class RoadsideAssistanceService : IRoadsideAssistanceService
    {
        private readonly IAssistantService _assistantSevice;
        private readonly ICustomerService _customerSevice;

        public RoadsideAssistanceService(IAssistantService assistantSevice, ICustomerService customerSevice)
        {
            _assistantSevice = assistantSevice;
            _customerSevice = customerSevice;
        }

        public async Task<List<Assistant>> FindNearestAssistants(Geolocation geolocation, int limit)
        {
            return await Task.Run(() =>
            {
                var nearbyAssitants = new SortedSet<Assistant>(new SortAssitantByDistanceComparer(geolocation));
                var availableAssitants = _assistantSevice.GetAssistants().Result.Where(x => x.IsAvailable() == true);
                nearbyAssitants.UnionWith(availableAssitants);
                return nearbyAssitants.Take(limit).ToList();
            });
        }

        public async Task<Assistant?> ReserveAssistant(Customer customer, Geolocation customerLocation)
        {
            return await Task.Run(() =>
            {
                var statusLog = customer.serviceRequest?.StatusLog;
                statusLog.Add($"{DateTime.Now} requested for an assitant");
                Log.Information($"Customer {customer.Name} requested for an assitant at location ({customerLocation.X},{customerLocation.Y})");
                var list = FindNearestAssistants(customerLocation, 3).Result;
                Assistant assistant = null;
                if (list.Count > 0)
                {
                    Log.Information($"Assistants {string.Join(",", list.Select(x => x.Name).ToList())} are allocated to Customer {customer.Name} Service request: {customer.serviceRequest.Id}");
                    var requestPublisher = new RequestPublisher(list, customer.serviceRequest);
                    assistant = requestPublisher.GetConfirmedAssistant().Result;
                    assistant?.Process(customer.serviceRequest);
                    _assistantSevice.Save(assistant);
                }
                else
                {
                    Log.Information($"None of Assistants are available now");
                }
                return assistant;
            });
        }

        public async Task ReleaseAssistant(Customer customer, Assistant assistant)
        {
            await Task.Run(() =>
            {
                var statusLog = customer.serviceRequest?.StatusLog;
                statusLog.Add($"{DateTime.Now} Assitant {assistant.Name} is released");
                assistant.ChangeAssignment(false);
                _assistantSevice.Save(assistant);
                _customerSevice.Save(customer);
            });
        }

        public async Task UpdateAssistantLocation(Assistant assistant, Geolocation location)
        {
            await Task.Run(() =>
            {
                _assistantSevice.UpdateLocation(assistant, location);
                _assistantSevice.Save(assistant);
            });
        }
    }
}
