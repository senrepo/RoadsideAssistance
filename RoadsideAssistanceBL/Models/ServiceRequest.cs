using System.ComponentModel.DataAnnotations;

namespace RoadsideAssistanceBL.Models
{
    public class ServiceRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProblemDescription { get; set; }
        public Vehicle? Vehicle { get; set; }
        public List<string>? StatusLog { get; set; }

        public ServiceRequest()
        {
            StatusLog = new List<string>();
        }
    }
}
