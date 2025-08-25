using ProvaTecnica.Core.Entities;
using ProvaTecnica.Core.Shared;

namespace ProvaTecnica.Core.Services
{
	public interface IAlunoService
	{
		//Regra de negócio: Suporte à Paginação e Filtros
		Task<ResultadoPaginado<Aluno>> GetAllAsync(int pageNumber, int pageSize, string? filtroNome, string? filtroCpf);
		Task<Aluno?> GetByIdAsync(int id);
		Task AddAsync(Aluno aluno);
		Task UpdateAsync(Aluno aluno);
		Task DeleteAsync(int id);
	}
}