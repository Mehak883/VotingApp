using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.Models
{
    public class State
    {
        public Guid Id { get; set; }
        [Required]
        public required String Name { get; set; }

    }
}
