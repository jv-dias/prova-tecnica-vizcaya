using Microsoft.EntityFrameworkCore;
using ProvaTecnica.Core.Data;
using ProvaTecnica.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaTecnica.Core.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly ApplicationDbContext _context;

        public MatriculaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Matricula>> GetAllAsync()
        {
            // Usamos .Include() para carregar os dados do Aluno e da Turma relacionados
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Turma)
                .OrderByDescending(m => m.DataMatricula)
                .ToListAsync();
        }

        public async Task AddAsync(Matricula matricula)
        {
            // Validação: Verifica se o aluno existe e está ativo (requisito da prova)
            var aluno = await _context.Alunos.FindAsync(matricula.AlunoId);
            if (aluno is null || !aluno.Ativo)
            {
                throw new InvalidOperationException("Não é possível matricular um aluno inexistente ou inativo.");
            }

            // Validação: Verifica se o aluno já está matriculado na turma
            var jaExisteMatricula = await _context.Matriculas
                .AnyAsync(m => m.AlunoId == matricula.AlunoId && 
                               m.TurmaId == matricula.TurmaId && 
                               m.Status == StatusMatricula.Ativa);

            if (jaExisteMatricula)
            {
                throw new InvalidOperationException("Este aluno já está matriculado nesta turma.");
            }
            
            // Se todas as validações passaram, define os valores padrão e salva
            matricula.DataMatricula = DateTime.UtcNow;
            matricula.Status = StatusMatricula.Ativa;

            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(int matriculaId)
        {
            var matricula = await _context.Matriculas.FindAsync(matriculaId);
            if (matricula != null)
            {
                matricula.Status = StatusMatricula.Cancelada;
                await _context.SaveChangesAsync();
            }
        }
    }
}