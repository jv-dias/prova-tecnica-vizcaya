using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Entities;
using System.Security.Claims;

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


        // A "mágica" da auditoria automática acontece aqui!
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