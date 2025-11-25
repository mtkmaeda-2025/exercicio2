using AgendaTelefonicaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaTelefonicaApi
{
    public class AgendaDbContext : DbContext
    {
        public DbSet<Contato> Contatos => Set<Contato>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=banco/agenda.db");
            base.OnConfiguring(optionsBuilder);
        }

    }
    
}
