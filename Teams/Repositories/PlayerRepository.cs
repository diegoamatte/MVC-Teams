using Teams.Data;
using Teams.Models;

namespace Teams.Repositories
{
    public class PlayerRepository : BaseRepository<Player>
    {
        public PlayerRepository(TeamsContext context) : base(context)
        {
        }
    }
}
