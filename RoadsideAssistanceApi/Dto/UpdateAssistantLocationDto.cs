
using RoadsideAssistanceBL.Models;
using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceApi.Dto
{
    public class UpdateAssistantLocationDto
    {
        [Required]
        public Assistant Assistant { get; set; }
        [Required]
        public Geolocation Geolocation { get; set; }
    }
}
