using DesafioSprint3.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioSprint3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Cada DbSet representa uma tabela no banco de dados
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}