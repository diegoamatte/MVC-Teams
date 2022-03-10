using System.ComponentModel.DataAnnotations;

namespace Teams.Models
{
    public class Team
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string League { get; set; }

        [Required]
        public string TeamLogoUrl { get; set; }

    }
}
