using System.ComponentModel.DataAnnotations;

namespace ProvaTecnica.Core.Entities
{
	public abstract class BaseEntity
	{
		public int Id { get; set; }

		public DateTime DataCriacao { get; set; }

		[MaxLength(450)]
		public string CriadoPorId { get; set; } = string.Empty;

		public DateTime DataUltimaEdicao { get; set; }

		[MaxLength(450)]
		public string EditadoPorId { get; set; } = string.Empty;
	}
}