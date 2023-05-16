using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceBL.Models
{
    public class Assistant
    {
        [Required]
        [JsonProperty]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public bool IsOccupied { get; set; }

        [JsonProperty]
        private Geolocation _location;

        private static object myLocker = new object();

        public async void Notify(ServiceRequest request, Action<Assistant, string> callback)
        {
            Log.Information($"Assitant {this.Name} notified for service request {request.Id}");
            var decision = await GetDecision();
            Log.Information($"Assitant {this.Name} send {decision} signal for service request {request.Id}");
            callback(this, decision);
        }

        private async Task<string> GetDecision()
        {
            var task = Task<string>.Factory.StartNew(() =>
            {
                //Delay 1 to 5 seconds randomly to provide the response
                var seconds = new Random().Next(1, 5);
                Thread.Sleep(seconds * 1000);
                return "accepted";
            });
            return await task;
        }

        public void Process(ServiceRequest request)
        {
            Log.Information($"Assitant {this.Name} report the status as occupied for service request {request.Id}");
            //process the service request
            request.StatusLog.Add($"{DateTime.Now} Assitant {this.Name} is on the way for service request {request.Id}");
        }

        public bool ChangeAssignment(bool isOccupied)
        {
            lock(myLocker)
            {
                IsOccupied = isOccupied;
                if (IsOccupied) Log.Information($"Assitant {this.Name} assigned for work");
                else Log.Information($"Assitant {this.Name} reports back to available pool");
            }
            return IsOccupied;
        }

        public void UpdateLocation(Geolocation loc)
        {
            _location = loc;
            Log.Information($"Assitant {this.Name} updated current location at ({_location.X},{_location.Y})");
        }
        public Geolocation GetLocation()
        {
            return _location;
        }

        public bool IsAvailable()
        {
            return !IsOccupied && _location != null;
        }
    }
}
