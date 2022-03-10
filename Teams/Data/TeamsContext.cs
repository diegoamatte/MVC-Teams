#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teams.Models;

namespace Teams.Data
{
    public class TeamsContext : DbContext
    {
        public TeamsContext (DbContextOptions<TeamsContext> options)
            : base(options)
        {
        }

        public DbSet<Teams.Models.Player> Player { get; set; }

        public DbSet<Teams.Models.Team> Team { get; set; }
    }
}
