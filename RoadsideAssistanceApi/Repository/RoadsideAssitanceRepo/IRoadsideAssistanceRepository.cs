using Microsoft.AspNetCore.Mvc;
using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceApi.Repository
{
    public interface IRoadsideAssistanceRepository
    {
        Task<List<Assistant>> FindNearestAssistants(Geolocation geolocation, int limit);
        Task<Assistant?> ReserveAssistant(Customer customer, Geolocation location);
        Task ReleaseAssistant(Customer customer, Assistant assistant);

        Task UpdateAssistantLocation(Assistant assistant, Geolocation location);
    }
}
