using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.DTOs.State
{
    public class StateRequest
    {
        [Required(ErrorMessage = "State name is required")]
        public required String Name { get; set; }
    }
}
