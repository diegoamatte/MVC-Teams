
namespace Teams.ViewModels.Team
{
    public class TeamDetailsViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string League { get; set; }

        public string? TeamLogoUrl { get; set; }

        public List<Teams.Models.Player>? Players { get; set; }
    }
}
