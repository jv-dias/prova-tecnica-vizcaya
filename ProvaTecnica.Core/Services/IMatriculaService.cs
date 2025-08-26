using ProvaTecnica.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public interface IMatriculaService
	{
		Task<List<Matricula>> GetAllAsync();
		Task AddAsync(Matricula matricula);
		Task CancelAsync(int matriculaId);
	}
}