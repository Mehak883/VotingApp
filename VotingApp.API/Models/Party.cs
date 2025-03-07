using System.ComponentModel.DataAnnotations;

namespace VotingApp.API.Models
{
    public class Party
    {
        public Guid Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Symbol { get; set; }

    }
}
