# Diário de Desenvolvimento – Prova Técnica .NET Blazor

**Candidato:** João Victor da Silva Dias

**Data de Início:** 23/08/2025

Este documento relata as decisões, os desafios e o progresso do desenvolvimento do projeto da prova técnica.

---

## Dia 1: 23/08/2025 - Arquitetura, Docker, Banco de Dados e Primeira Migration

**Foco do dia:** Realizar o planejamento completo da arquitetura, configurar o ambiente de desenvolvimento, modelar as entidades, resolver um desafio de integração com o template do .NET Identity, configurar o banco de dados com Docker e materializar o esquema com a primeira migration.

#### Decisões Tomadas:

* **Arquitetura da Solução:** A base do projeto foi definida com uma solução de 2 projetos (`Core` e `Web`) para uma clara separação entre a lógica de negócio e a interface do usuário.
* **Modelagem de Entidades:** As entidades `Aluno`, `Turma` e `Matricula` foram modeladas. Decidi por uma abordagem de auditoria robusta, criando uma `BaseEntity` com campos como `DataCriacao`, `CriadoPorId`,`DataUltimaEdicao`,`EditadoPorId`; da qual todas as outras entidades herdam.
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

* Com o banco de dados estruturado e conectado, o próximo passo é implementar a lógica de *Seed* para popular as tabelas com dados de teste.
---

## Dia 2: 24/08/2025 - Implementação Completa do CRUD de Alunos e Serviços

**Foco do dia:** Desenvolver toda a funcionalidade de ponta a ponta para a entidade `Aluno`, incluindo a lógica de negócio no backend, a integração com serviços externos e a construção da interface do usuário com todos os requisitos da prova.

#### Progresso e Implementação:

* **Seed de Dados:** Implementei a classe `SeedData` para popular o banco de dados. Para os usuários "Admin" e "Professor", utilizei o `UserManager` para garantir que as senhas fossem armazenadas corretamente com hash. Para a massa de dados de teste, utilizei a biblioteca `Bogus` para gerar 50 alunos com dados realistas em português(Exceto os bairros), o que facilitou os testes de paginação e filtros.

* **Serviço Externo (ViaCEP):** Criei o `ViaCepService`, responsável por consumir a API do ViaCEP. A implementação já inclui um cache em memória de 5 minutos para otimizar chamadas repetidas, atendendo a um requisito da prova. Criei também um DTO (`ViaCepResponse`) para desacoplar a resposta da API do meu modelo de domínio.

* **Serviço de Negócio (`AlunoService`):** Desenvolvi a interface `IAlunoService` e sua implementação `AlunoService`, que centraliza toda a lógica de negócio para o CRUD de Alunos. Todos os métodos de acesso a dados (Criar, Ler, Atualizar, Excluir) estão contidos neste serviço.

* **Interface do Usuário (UI):**
    * **Páginas CRUD:** Criei as três páginas na pasta `Components/Pages/Alunos`: `Index.razor` (Listagem), `Criar.razor` e `Editar.razor`.
    * **Listagem:** A página `Index` foi implementada com chamadas ao `AlunoService` para exibir os dados.
    * **Formulários:** As páginas `Criar` e `Editar` utilizam o componente `EditForm` do Blazor, com validação de dados via `DataAnnotations`. Corrigi bugs de validação e de reset do formulário para garantir o funcionamento correto.
    * **Melhorias de UX:** Implementei diálogos de confirmação (`confirm`) e alerta (`alert`) via JavaScript Interop nos formulários para fornecer um feedback mais claro ao usuário após as ações de salvar ou em caso de erros (como CPF duplicado).

* **Paginação e Filtros:** Refatorei o `AlunoService` e a página `Index.razor` para incluir a funcionalidade completa de paginação e filtros por Nome e CPF, atendendo a todos os requisitos da listagem.
#### Decisões Tomadas:

* **Problemas com Autenticação:** Diante de dificuldades na configuração da autenticação que estavam bloqueando o progresso, tomei a decisão estratégica de desativar temporariamente as regras de autorização. Isso permitiu focar no desenvolvimento e teste de todas as funcionalidades da aplicação. A reativação e o ajuste final da segurança serão feitos em uma etapa posterior.

* **Nomenclatura em Português:** Decidi manter a consistência da nomenclatura em português sempre que possível, como na criação do componente `Paginacao.razor` e da classe de DTO `ResultadoPaginado.cs`.

#### Desafios Encontrados:

* O principal desafio do dia foi diagnosticar e resolver uma série de problemas de configuração e comportamento inesperado relacionados ao novo template do .NET 8, especialmente no que tange à autenticação, autorização e renderização do layout. A decisão de pausar essa frente de trabalho foi crucial para manter a produtividade.
* Encontrei e corrigi um bug sutil no reset de formulários do Blazor, o que me levou a decisão de gerenciar o `EditContext` manualmente.

#### Próximos Passos:

* Terminar a implementação da funcionalidade de importação de Alunos via CSV, que ainda não foi concluída.
* Desenvolver o CRUD para a entidade `Turma`.
* Construir a lógica e a página para a entidade `Matricula`.