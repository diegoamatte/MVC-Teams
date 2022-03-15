using Teams.Models;

namespace Teams.ViewModels.Player
{
    public class PlayerIndexViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ushort Age { get; set; }

        public string? Nationality { get; set; }

        public Teams.Models.Team Team { get; set; }
    }
}
