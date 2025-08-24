using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ProvaTecnica.Core.Data
{
    // DbContext personalizado que inclui auditoria automática
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Mapeia suas entidades para tabelas no banco de dados
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed para as Roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "a1e1b5f2-9f0a-4b0e-9f5a-1f1f1b0e9f5a",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "b2f2c6g3-0g1b-5c1f-0g6b-2g2g2c1f0g6b",
                    Name = "Professor",
                    NormalizedName = "PROFESSOR"
                }
            );
        }

        // Sobrescreve o método SaveChangesAsync para incluir auditoria automática
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DataCriacao = DateTime.UtcNow;
                        entry.Entity.CriadoPorId = userId ?? "Sistema";
                        entry.Entity.DataUltimaEdicao = DateTime.UtcNow;
                        entry.Entity.EditadoPorId = userId ?? "Sistema";
                        break;

                    case EntityState.Modified:
                        entry.Entity.DataUltimaEdicao = DateTime.UtcNow;
                        entry.Entity.EditadoPorId = userId ?? "Sistema";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}