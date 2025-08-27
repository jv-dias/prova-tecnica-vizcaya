using ProvaTecnica.Core.DTOs;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public interface IDashboardService
	{
		Task<DashboardStats> GetStatsAsync();
	}
}