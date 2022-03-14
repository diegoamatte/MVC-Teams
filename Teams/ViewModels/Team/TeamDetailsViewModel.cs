using Teams.Models;

namespace Teams.ViewModels.Team
{
    public class TeamDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string League { get; set; }

        public string? TeamLogoUrl { get; set; }

        public List<Player> Players { get; set; }
    }
}
