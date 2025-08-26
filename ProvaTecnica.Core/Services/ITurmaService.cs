using ProvaTecnica.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public interface ITurmaService
	{
		Task<List<Turma>> GetAllAsync();
		Task<Turma?> GetByIdAsync(int id);
		Task AddAsync(Turma turma);
		Task UpdateAsync(Turma turma);
		Task DeleteAsync(int id);
	}
}