using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teams.ViewModels.Team
{
    public class CreateTeamViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string League { get; set; }

        [DisplayName("Logo URL")]
        [Url]
        public string? TeamLogoUrl { get; set; }
    }
}
