using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceBL.Models
{
    public class Geolocation
    {
        [Required]
        [JsonProperty]
        public int X { get; set; }

        [Required]
        [JsonProperty]
        public int Y { get; set; }

        public Geolocation()
        {

        }

        public Geolocation(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
