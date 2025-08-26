using ProvaTecnica.Core.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
	public interface ICsvImportService
	{
		Task<List<LinhaCsvAluno>> ProcessarCsvAsync(Stream arquivoStream);
	}
}