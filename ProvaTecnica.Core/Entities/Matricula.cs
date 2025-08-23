namespace ProvaTecnica.Core.Entities
{
	public class Matricula : BaseEntity
	{
		public DateTime DataMatricula { get; set; }
		public StatusMatricula Status { get; set; }

		// Chaves Estrangeiras
		public int AlunoId { get; set; }
		public virtual Aluno Aluno { get; set; } = null!;

		public int TurmaId { get; set; }
		public virtual Turma Turma { get; set; } = null!;
	}
}