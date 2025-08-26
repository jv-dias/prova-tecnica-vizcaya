using ProvaTecnica.Core.Entities;
using ProvaTecnica.Core.Enums;

namespace ProvaTecnica.Core.DTOs
{
	public class LinhaCsvAluno
	{
		public Aluno AlunoDoCsv { get; set; } = new();
		public Aluno? AlunoExistente { get; set; }
		public StatusImportacao Status { get; set; }
		public string MensagemErro { get; set; } = string.Empty;
	}
}