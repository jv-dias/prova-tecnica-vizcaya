using System.ComponentModel.DataAnnotations;

namespace ProvaTecnica.Core.Entities
{
	public class Matricula : BaseEntity
	{
		public DateTime DataMatricula { get; set; }
		public StatusMatricula Status { get; set; }

		// Chaves Estrangeiras
		// Validações para garantir que um aluno e uma turma sejam selecionados
		[Range(1, int.MaxValue, ErrorMessage = "Por favor, selecione um aluno.")]
		public int AlunoId { get; set; }
		public virtual Aluno Aluno { get; set; } = null!;
		
		[Range(1, int.MaxValue, ErrorMessage = "Por favor, selecione uma turma.")]
		public int TurmaId { get; set; }
		public virtual Turma Turma { get; set; } = null!;
	}
}