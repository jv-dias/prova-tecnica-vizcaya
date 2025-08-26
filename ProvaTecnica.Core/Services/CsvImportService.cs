using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data;
using ProvaTecnica.Core.DTOs;
using ProvaTecnica.Core.Entities;
using ProvaTecnica.Core.Enums;
using ProvaTecnica.Core.Mappings;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ProvaTecnica.Core.Services
{
    public class CsvImportService : ICsvImportService
    {
        private readonly ApplicationDbContext _context;

        public CsvImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        //Processa um arquivo CSV de alunos, valida os dados, identifica duplicidades e salva novos registros.
        public async Task<List<LinhaCsvAluno>> ProcessarCsvAsync(Stream arquivoStream)
        {
            // Configuração do CsvHelper para ler arquivos delimitados por ponto e vírgula e ignorar validação de cabeçalho
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null
            };

            using var reader = new StreamReader(arquivoStream);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<AlunoMap>();

            // Lê todos os registros do CSV para uma lista de objetos Aluno
            var alunosDoCsv = await csv.GetRecordsAsync<Aluno>().ToListAsync();
            var resultadoFinal = new List<LinhaCsvAluno>();
            var novosAlunosParaSalvar = new List<Aluno>();

            // Busca todos os CPFs do CSV no banco de dados de uma só vez para otimizar a verificação de duplicidade
            var cpfsDoCsv = alunosDoCsv.Select(a => a.CPF).ToList();
            var alunosExistentesNoDb = await _context.Alunos
                                            .Where(a => a.CPF != null && cpfsDoCsv.Contains(a.CPF))
                                            .ToDictionaryAsync(a => a.CPF!);

            // Processa cada aluno do CSV
            foreach (var alunoDoCsv in alunosDoCsv)
            {
                var linhaResultado = new LinhaCsvAluno { AlunoDoCsv = alunoDoCsv };

                // Validação dos dados do aluno usando DataAnnotations
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(alunoDoCsv);
                if (!Validator.TryValidateObject(alunoDoCsv, validationContext, validationResults, true))
                {
                    linhaResultado.Status = StatusImportacao.ErroDeValidacao;
                    linhaResultado.MensagemErro = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                    resultadoFinal.Add(linhaResultado);
                    continue;
                }

                // Verifica se o aluno já existe no banco pelo CPF
                if (alunoDoCsv.CPF != null && alunosExistentesNoDb.TryGetValue(alunoDoCsv.CPF, out var alunoExistente))
                {
                    linhaResultado.Status = StatusImportacao.Duplicado;
                    linhaResultado.AlunoExistente = alunoExistente;
                }
                else
                {
                    linhaResultado.Status = StatusImportacao.ProcessadoCriado;
                    novosAlunosParaSalvar.Add(alunoDoCsv);
                }
                resultadoFinal.Add(linhaResultado);
            }

            // Salva todos os novos alunos de uma vez, se houver
            if (novosAlunosParaSalvar.Any())
            {
                _context.Alunos.AddRange(novosAlunosParaSalvar);
                await _context.SaveChangesAsync();
            }

            // Retorna o resultado do processamento de cada linha do CSV
            return resultadoFinal;
        }
    }
}