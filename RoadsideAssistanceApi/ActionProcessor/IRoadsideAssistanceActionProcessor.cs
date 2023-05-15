using Microsoft.AspNetCore.Mvc;
using RoadsideAssistanceApi.Dto;
using RoadsideAssistanceBL.Models;

namespace RoadsideAssistanceApi.ActionProcessor
{
    public interface IRoadsideAssistanceActionProcessor
    {
        Task<List<Assistant>> FindNearestAssistants(Geolocation geolocation, int limit);
        Task<Assistant?> ReserveAssistant(ReserveAssistantDto reserveAssistantDto);
        Task ReleaseAssistant(ReleaseAssistantDto releaseAssistantDto);
        Task UpdateAssistantLocation(UpdateAssistantLocationDto updateAssistantLocationDto);
    }
}
