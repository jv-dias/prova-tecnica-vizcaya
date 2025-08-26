using CsvHelper.Configuration;
using ProvaTecnica.Core.Entities;

namespace ProvaTecnica.Core.Mappings
{
	public class AlunoMap : ClassMap<Aluno>
	{
		public AlunoMap()
		{
			// Mapeia apenas as propriedades que esperamos do CSV
			Map(m => m.Nome);
			Map(m => m.CPF);
			Map(m => m.Email);
			Map(m => m.Ativo);
			Map(m => m.CEP);
			Map(m => m.Logradouro);
			Map(m => m.Complemento);
			Map(m => m.Bairro);
			Map(m => m.Cidade);
			Map(m => m.UF);
		}
	}
}