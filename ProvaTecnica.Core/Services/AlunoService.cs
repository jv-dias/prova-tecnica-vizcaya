using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data;
using ProvaTecnica.Core.Entities;
using ProvaTecnica.Core.Shared;

namespace ProvaTecnica.Core.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly ApplicationDbContext _context;

        public AlunoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ResultadoPaginado<Aluno>> GetAllAsync(int pageNumber, int pageSize, string? filtroNome, string? filtroCpf)
        {   
            // Query inicial
            var query = _context.Alunos.AsQueryable();

            // Adição do filtro de nome, se ele foi fornecido
            if (!string.IsNullOrWhiteSpace(filtroNome))
            {
                query = query.Where(a => a.Nome != null && a.Nome.Contains(filtroNome));
            }

            // Adição do filtro de CPF, se ele foi fornecido
            if (!string.IsNullOrWhiteSpace(filtroCpf))
            {
                query = query.Where(a => a.CPF != null && a.CPF.Contains(filtroCpf));
            }

            // A ordenação, contagem e paginação são feitas sobre a query já filtrada
            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.Nome)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ResultadoPaginado<Aluno>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<Aluno?> GetByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }
        
        public async Task AddAsync(Aluno aluno)
        {
            // Regra de negócio: não permitir CPF duplicado
            if (await _context.Alunos.AnyAsync(a => a.CPF == aluno.CPF))
            {
                throw new InvalidOperationException("Já existe um aluno cadastrado com este CPF.");
            }

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Aluno aluno)
        {
            // Regra de negócio: não permitir CPF duplicado, exceto para editar o próprio aluno
            if (await _context.Alunos.AnyAsync(a => a.CPF == aluno.CPF && a.Id != aluno.Id))
            {
                throw new InvalidOperationException("Já existe outro aluno cadastrado com este CPF.");
            }
            
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
        }
    }
}