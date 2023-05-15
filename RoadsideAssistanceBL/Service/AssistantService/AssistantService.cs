using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace RoadsideAssistanceBL.Service
{
    public class AssistantService : IAssistantService
    {
        private readonly IRoadSideDataStore _dataStore;

        public AssistantService(IRoadSideDataStore dataStore)
        {
            _dataStore = dataStore;

        }

        public async Task<ConcurrentBag<Assistant>> GetAssistants()
        {
            return await Task.Run(() =>
            {
                return new ConcurrentBag<Assistant>(_dataStore.GetAssitants());
            });
        }

        public async Task<bool> IsValidAssistant(int assistantId)
        {
            var list = _dataStore?.GetAssitants();
            return list.Any(x => x.Id == assistantId);
        }

        public async Task Save(Assistant assistant)
        {
            await Task.Run(() =>
            {
                var list = _dataStore.GetAssitants();
                var dbAssistant = list.Where(x => x.Id == assistant.Id).FirstOrDefault();
                //save logic
                if(dbAssistant != null)
                {
                    dbAssistant.Id = assistant.Id;
                    dbAssistant.Name = assistant.Name;
                    dbAssistant.IsOccupied = assistant.IsOccupied;
                    Log.Information($"Assistant:{assistant.Name} record updated in the database");
                }
            });
        }
        public async Task UpdateLocation(Assistant assistant, Geolocation location)
        {
            await Task.Run(() =>
            {
                var list = _dataStore.GetAssitants();
                var dbAssistant = list.Where(x => x.Id == assistant.Id).FirstOrDefault();
                //save logic
                if (dbAssistant != null)
                {
                    dbAssistant.UpdateLocation(location);
                    Log.Information($"Assistant:{assistant.Name} location updated in the database");
                }
            });
        }
    }
}

