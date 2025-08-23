# Diário de Desenvolvimento – Prova Técnica .NET Blazor

**Candidato:** João Victor da Silva Dias

**Data de Início:** 23/08/2025

Este documento relata as decisões, os desafios e o progresso do desenvolvimento do projeto da prova técnica.

---

## Dia 1: 23/08/2025 - Arquitetura, Docker, Banco de Dados e Primeira Migration

**Foco do dia:** Realizar o planejamento completo da arquitetura, configurar o ambiente de desenvolvimento, modelar as entidades, resolver um desafio de integração com o template do .NET Identity, configurar o banco de dados com Docker e materializar o esquema com a primeira migration.

#### Decisões Tomadas:

* **Arquitetura da Solução:** A base do projeto foi definida com uma solução de 2 projetos (`Core` e `Web`) para uma clara separação entre a lógica de negócio e a interface do usuário.
* **Modelagem de Entidades:** As entidades `Aluno`, `Turma` e `Matricula` foram modeladas. Decidi por uma abordagem de auditoria robusta, criando uma `BaseEntity` com campos como `DataCriacao` e `CriadoPorId`, da qual todas as outras entidades herdam.
* **Arquitetura de Usuário:** O desafio mais significativo do dia foi a integração com o template do ASP.NET Identity. A decisão final foi Estender, criando uma classe `ApplicationUser` customizada que herda de `IdentityUser`. Apesar de exigir uma refatoração inicial, esta abordagem alinha o projeto com as boas práticas e permite futuras customizações.
* **Ambiente de Banco de Dados:** Para um ambiente limpo e isolado, optei por usar Docker para o banco de dados. A imagem oficial do `SQL Server Developer Edition` (`mcr.microsoft.com/mssql/server:2022-latest`) foi a escolhida.
* **Configuração de Conexão:** A string de conexão foi configurada para autenticação SQL (usuário `sa`), necessária para a comunicação com o contêiner Docker.

#### Progresso e Implementação:

1.  **Estrutura:** A solução e os projetos `Core` e `Web` foram criados e referenciados.
2.  **Refatoração:** O `DbContext`, o `Program.cs` e as páginas da UI do Identity foram totalmente ajustados para usar a classe `ApplicationUser` customizada.
3.  **Docker:** O contêiner do SQL Server foi iniciado com sucesso com a porta 1433 mapeada.
4.  **Migration:** A primeira migration (`InitialCreate`) foi gerada e, em seguida, aplicada ao banco de dados armazenado no contêiner.

#### Desafios Encontrados:

* O principal desafio foi o forte acoplamento do template do .NET 8 com sua própria implementação do Identity (`ApplicationUser` e `DbContext` no projeto Web). A tentativa inicial de simplificar para o `IdentityUser` genérico causou uma cascata de erros. O problema foi resolvido de forma definitiva ao mover a responsabilidade da classe `ApplicationUser` para o projeto `Core`, alinhando a necessidade do template com a arquitetura estabelecida.

#### Verificação:

* A criação do banco de dados `ProvaTecnicaDb` e de todas as tabelas (`Alunos`, `Turmas`, `AspNetUsers`, etc.) foi confirmada com sucesso utilizando o Database Explorer do Rider.

#### Próximos Passos:

* Com o banco de dados estruturado e conectado, o próximo passo é implementar a lógica de *Seed* (povoamento inicial) para popular as tabelas com dados de teste.