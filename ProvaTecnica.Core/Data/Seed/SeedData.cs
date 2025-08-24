using Bogus;
using Bogus.Extensions.Brazil;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProvaTecnica.Core.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Data.Seed
{
    public static class SeedData
    {
        // Método principal que orquestra o povoamento
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedUsersAsync(userManager);
            await SeedTurmasAsync(context);
            await SeedAlunosAsync(context);
        }

        // Cria os usuários Admin e Professor
        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            // Se já existir algum usuário, não faz nada
            if (userManager.Users.Any())
            {
                return;
            }

            // Cria o usuário Admin
            var adminUser = new ApplicationUser { UserName = "admin@teste.com", Email = "admin@teste.com", EmailConfirmed = true };
            var adminResult = await userManager.CreateAsync(adminUser, "MeContrata@123");
            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Cria o usuário Professor
            var professorUser = new ApplicationUser { UserName = "professor@teste.com", Email = "professor@teste.com", EmailConfirmed = true };
            var professorResult = await userManager.CreateAsync(professorUser, "MeContrata@123");
            if (professorResult.Succeeded)
            {
                await userManager.AddToRoleAsync(professorUser, "Professor");
            }
        }

        // Cria algumas turmas de exemplo
        private static async Task SeedTurmasAsync(ApplicationDbContext context)
        {
            if (context.Turmas.Any())
            {
                return;
            }

            context.Turmas.AddRange(
                new Turma { Nome = "Programação Orientada a Objetos", AnoLetivo = 2025 },
                new Turma { Nome = "Do Monolito aos Microsserviços", AnoLetivo = 2025 },
                new Turma { Nome = "Modelagem de dados", AnoLetivo = 2025 },
                new Turma { Nome = "Suporte Técnico", AnoLetivo = 2025 },
                new Turma { Nome = "Desenvolvimento Frontend com Blazor Server", AnoLetivo = 2025 }
            );

            await context.SaveChangesAsync();
        }

        // Gera 50 alunos de teste com dados realistas usando a biblioteca Bogus
        private static async Task SeedAlunosAsync(ApplicationDbContext context)
        {
            if (context.Alunos.Any())
            {
                return;
            }

            // Configura o gerador de dados falsos para o português do Brasil
            var alunoFaker = new Faker<Aluno>("pt_BR")
                .RuleFor(a => a.Nome, f => f.Name.FullName())
                .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.Nome.ToLower().Replace(" ", ".")))
                .RuleFor(a => a.CPF, f => f.Person.Cpf())
                .RuleFor(a => a.Ativo, f => f.Random.Bool(0.8f)) 
                .RuleFor(a => a.CEP, f => f.Address.ZipCode("#####-###"))
                .RuleFor(a => a.Logradouro, f => f.Address.StreetAddress())
                .RuleFor(a => a.Bairro, f => f.Address.County())
                .RuleFor(a => a.Cidade, f => f.Address.City())
                .RuleFor(a => a.UF, f => f.Address.StateAbbr());

            var alunos = alunoFaker.Generate(50);
            context.Alunos.AddRange(alunos);
            await context.SaveChangesAsync();
        }
    }
}