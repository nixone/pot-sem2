using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    /// <summary>
    /// Representation of database storage for played games
    /// </summary>
    public class PlayedGameContext : DbContext
    {
        public DbSet<PlayedGame> PlayedGames { get; set; }
    }
}
