using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceBL.Service
{
    public interface IRoadsideAssistanceService
    {
        Task UpdateAssistantLocation(Assistant assistant, Geolocation assistantLocation);
        Task<List<Assistant>> FindNearestAssistants(Geolocation geolocation, int limit);
        Task<Assistant?> ReserveAssistant(Customer customer, Geolocation customerLocation);
        Task ReleaseAssistant(Customer customer, Assistant assistant);

    }
}