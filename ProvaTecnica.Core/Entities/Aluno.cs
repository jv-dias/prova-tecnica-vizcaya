using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProvaTecnica.Core.Entities
{
	[Index(nameof(CPF), IsUnique = true)]
	public class Aluno : BaseEntity
	{
		[Required(ErrorMessage = "O nome é obrigatório.")]
		[MaxLength(100)]
		public string? Nome { get; set; } = string.Empty;

		[Required(ErrorMessage = "O CPF é obrigatório.")]
		[MaxLength(14)] // Formato: 123.456.789-00
		public string? CPF { get; set; } = string.Empty;
        
		[Required(ErrorMessage = "O email é obrigatório.")]
		[MaxLength(100)]
		public string? Email { get; set; } = string.Empty;
		
		// Propriedades de endereço que serão preenchidas pelo ViaCEP
		[Required(ErrorMessage = "O CEP é obrigatório.")]
		[MaxLength(9)] // Formato: 12345-678
		public string? CEP { get; set; } = string.Empty;
        
		[Required(ErrorMessage = ("O logradouro é obrigatório."))]
		[MaxLength(200)]
		public string? Logradouro { get; set; } = string.Empty;
        
		[MaxLength(100)]
		public string? Complemento { get; set; } = string.Empty;
        
		[Required(ErrorMessage = "O bairro é obrigatório.")]
		[MaxLength(100)]
		public string? Bairro { get; set; } = string.Empty;
        
		[Required(ErrorMessage = "A cidade é obrigatória.")]
		[JsonPropertyName("localidade")]
		[MaxLength(100)]
		public string? Cidade { get; set; } = string.Empty;
        
		[Required(ErrorMessage = "A UF é obrigatória.")]
		[MaxLength(2)]
		public string? UF { get; set; } = string.Empty;
		
		public bool Ativo { get; set; } = true;
	}
}