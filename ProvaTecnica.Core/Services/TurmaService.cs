using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data;
using ProvaTecnica.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public class TurmaService : ITurmaService
	{
		private readonly ApplicationDbContext _context;

		public TurmaService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Turma>> GetAllAsync()
		{
			return await _context.Turmas.OrderBy(t => t.Nome).ToListAsync();
		}

		public async Task<Turma?> GetByIdAsync(int id)
		{
			return await _context.Turmas.FindAsync(id);
		}

		public async Task AddAsync(Turma turma)
		{
			_context.Turmas.Add(turma);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Turma turma)
		{
			_context.Entry(turma).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var turma = await _context.Turmas.FindAsync(id);
			if (turma != null)
			{
				_context.Turmas.Remove(turma);
				await _context.SaveChangesAsync();
			}
		}
	}
}