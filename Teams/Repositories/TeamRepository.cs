using Teams.Data;
using Teams.Models;

namespace Teams.Repositories
{
    public class TeamRepository : BaseRepository<Team>
    {
        public TeamRepository(TeamsContext context) : base(context)
        {
        }
    }
}
