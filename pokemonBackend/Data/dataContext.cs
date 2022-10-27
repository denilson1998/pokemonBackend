using Microsoft.EntityFrameworkCore;
using pokemonBackend.Models;
using System.Collections.Generic;

namespace pokemonBackend.Data
{
    public class dataContext : DbContext
    {
        public dataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<pokemonModel> pokemons { get; set; }
    }
}
