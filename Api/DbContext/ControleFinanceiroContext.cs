using Microsoft.EntityFrameworkCore;
using Api.Entities;

namespace Api.DBContext
{
    public class ControleFinanceiroContext : DbContext
    {
        public ControleFinanceiroContext()
        { }

        public ControleFinanceiroContext(DbContextOptions<ControleFinanceiroContext> options)
           : base(options)
        { }

        public DbSet<CategoriaEntity> CategoriaEntities { get; set; }
        public DbSet<SubcategoriaEntity> SubcategoriaEntities { get; set; }
        public DbSet<LancamentoEntity> LancamentoEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoriaEntity>()
                .HasIndex(c => c.Nome).IsUnique();

            modelBuilder.Entity<SubcategoriaEntity>()
                .HasIndex(c => new { c.Nome, c.id_categoria }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}