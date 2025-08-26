namespace ProvaTecnica.Core.Enums
{
	public enum StatusImportacao
	{
		ProcessadoCriado,    // Novo aluno, salvo com sucesso
		Duplicado,           // CPF duplicado, aguardando ação do usuário
		ErroDeValidacao,     // Falha na validação (ex: campo obrigatório em branco)
		ProcessadoAtualizado,// Aluno existente atualizado com sucesso
		ProcessadoIgnorado	 // Aluno existente, mas sem alterações (ignorado)
	}
}