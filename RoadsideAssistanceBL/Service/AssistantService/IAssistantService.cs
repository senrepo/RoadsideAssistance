using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.Service
{
    public interface IAssistantService
    {
        Task<ConcurrentBag<Assistant>> GetAssistants();
        Task<bool> IsValidAssistant(int assistantId);
        Task Save(Assistant assistant);
        Task UpdateLocation(Assistant assistant, Geolocation location);
    }
}
