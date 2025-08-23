using System.ComponentModel.DataAnnotations;

namespace ProvaTecnica.Core.Entities
{
	public class Turma : BaseEntity
	{
		[Required(ErrorMessage = "O nome da turma é obrigatório.")]
		[MaxLength(100)]
		public string Nome { get; set; } = string.Empty;

		[Required(ErrorMessage = "O ano letivo é obrigatório.")]
		public int AnoLetivo { get; set; }
	}
}