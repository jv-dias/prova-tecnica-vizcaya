using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data;
using ProvaTecnica.Core.DTOs;
using ProvaTecnica.Core.Entities;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public class DashboardService : IDashboardService
	{
		private readonly ApplicationDbContext _context;

		public DashboardService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<DashboardStats> GetStatsAsync()
		{
			var stats = new DashboardStats
			{
				TotalAlunos = await _context.Alunos.CountAsync(),
				TotalTurmas = await _context.Turmas.CountAsync(),
				TotalMatriculasAtivas = await _context.Matriculas
					.CountAsync(m => m.Status == StatusMatricula.Ativa)
			};
			return stats;
		}
	}
}