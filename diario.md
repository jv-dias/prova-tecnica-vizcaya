# Diário de Desenvolvimento – Prova Técnica .NET Blazor

**Candidato:** João Victor Dias

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
* 
---

## Dia 3: 25/08/2025 - Finalização do Módulo de Alunos (Importação CSV) e CRUDS de Turma e Matrícula

**Foco do dia:** Concluir todas as funcionalidades relacionadas à entidade `Aluno` com a implementação da complexa tela de importação via CSV, e em seguida, construir as funcionalidades de negócio para `Turma` e `Matricula`.

#### Progresso e Implementação:

* **Finalização do Módulo de Alunos (Importação CSV):**
    * **Interface Interativa:** Desenvolvi a página `Importar.razor` com um fluxo de trabalho de revisão interativo. A aplicação agora processa o CSV, cria automaticamente os novos alunos válidos e exibe uma tabela de revisão caso seja encontrado algum CPF duplicado na hora da inserção no banco de dados.
    * **Revisão de Conflitos:** A tabela de revisão exibe todas as linhas do arquivo com feedback visual (cores) e status claros. Ações de "Atualizar" ou "Ignorar" estão disponíveis apenas para as linhas com CPF duplicado, permitindo que o usuário resolva as pendências caso a caso.
    * **Tratamento de Erros:** A lógica de validação foi implementada no backend (`CsvImportService`) para identificar registros com dados inválidos (ex: campos obrigatórios vazios), que também são exibidos na tela de revisão com a respectiva mensagem de erro.
    * **Modelo de CSV:** Implementei uma funcionalidade de "Baixar Modelo", que gera um arquivo CSV formatado com delimitador de ponto e vírgula e codificação "UTF-8 with BOM" para garantir a máxima compatibilidade com o Excel.
    * **Backend Robusto:** O `CsvImportService` foi refatorado para suportar este fluxo, realizando uma análise completa do arquivo, criando novos registros em lote e retornando uma lista detalhada para a interface do usuário. Todo o processo de leitura de arquivo é feito de forma assíncrona para garantir a performance.

* **CRUD de Turmas:**
    * **Backend:** Criei a interface `ITurmaService` e sua implementação `TurmaService`, seguindo o padrão estabelecido para o CRUD de Alunos.
    * **Frontend:** Construí a interface do usuário completa para Turmas, incluindo as páginas de `Index`, `Criar` e `Editar`. Adicionei a funcionalidade de exclusão com diálogo de confirmação diretamente na página de listagem para que não seja necessário navegar até a página de edição para uma exclusão.

* **Funcionalidade de Matrículas:**
    * **Backend (`MatriculaService`):** Implementei a lógica de negócio principal no `MatriculaService`. O método `AddAsync` agora valida se um aluno está ativo e se já não possui uma matrícula na turma selecionada, cumprindo os requisitos da prova.
    * **Frontend (Páginas de Matrículas):** Criei as páginas `Index` e `Criar` para Matrículas. A tela de criação exibe nos dropdowns apenas os alunos ativos, aplicando a regra de negócio já na interface.

#### Decisões Tomadas:

* **Fluxo de Importação Interativo:** Decidi abandonar o fluxo de importação em lote por um modelo de revisão interativo, por ser uma solução muito mais robusta e amigável para o usuário final.
* **Padrão de CRUD:** Continuei a replicar o padrão arquitetural de Serviços + Páginas Blazor para as novas funcionalidades, mantendo a consistência do projeto.

#### Desafios Encontrados:

* O maior desafio foi depurar o `CsvHelper` para lidar corretamente com o delimitador, os cabeçalhos e a leitura assíncrona, resultando em uma implementação de serviço de importação explícita e resiliente com o uso de uma `ClassMap`.
* Ajustar a interface do usuário em Blazor para refletir as mudanças de estado de cada linha individualmente na tabela de revisão exigiu um gerenciamento de estado cuidadoso na página `Importar.razor` .

#### Próximos Passos:

* Com todas as funcionalidades de negócio implementadas, a próxima e última grande etapa é reativar e finalizar a configuração de **autenticação e autorização** em todo o sistema.
* Escrever o `README.md` final e preparar o projeto para a entrega.

---

## Dia 4: 26/08/2025 - Finalização da Autenticação e Polimento da Aplicação

**Foco do dia:** Reativar e depurar completamente o sistema de autenticação e autorização, garantindo que todas as regras de acesso funcionem como esperado. Adicionar os toques finais na aplicação para melhorar a apresentação e a experiência do usuário.

#### Progresso e Implementação:

* **Reativação e Correção da Autenticação:**
    * Iniciei o processo de reativação da segurança adicionando o atributo `@attribute [Authorize]` à `Home.razor`. Isso corrigiu o fluxo inicial e garantiu que usuários não autenticados sejam corretamente redirecionados para a página de login.

* **Implementação da Autorização (Roles):**
    * **Navegação:** Apliquei as regras de autorização baseadas em `Roles` no menu principal (`MainLayout.razor`). O link "Alunos" agora é visível para "Admin" e "Professor", enquanto "Turmas" e "Matrículas" são visíveis apenas para o "Admin".
    * **Conteúdo Dinâmico:** A mesma lógica de autorização foi aplicada aos cards do dashboard na `Home.razor`, garantindo que o usuário "Professor" veja apenas as estatísticas de alunos, enquanto o "Admin" vê todas as métricas.

* **Resolução do Bug de Logout:**
    * O desafio mais complexo do dia foi corrigir o botão de Logout, que apresentava erros de *"Antiforgery Token"* e *"Headers are read-only"*.
    * Após várias tentativas de depuração com formulários estáticos e métodos interativos, uma solução temporária foi implementada devido ao prazo de entrega.
    * Criei um **Endpoint Mínimo (Minimal API)** diretamente no `Program.cs` para a rota `/Logout`. Este endpoint lida de forma segura e atômica com a ação de `SignOutAsync` e o redirecionamento, eliminando os conflitos entre os modos de renderização interativo e estático do Blazor.

* **Dashboard na Home Page:**
    * Para enriquecer a apresentação do projeto, implementei o "mini-dashboard" na `Home.razor`.
    * Criei um `DashboardService` para buscar estatísticas do banco de dados de forma eficiente (`Total de Alunos`, `Total de Turmas`, etc.).
    * A Home page agora exibe esses dados em cards interativos que também servem como links para as seções correspondentes.
    * Havia preparado um design mais elaborado, contudo, optei por uma abordagem mais simples e funcional para garantir a entrega dentro do prazo.

#### Decisões Tomadas:

* **Endpoint Mínimo para Logout:** Diante da persistência dos erros de segurança, decidi abandonar as abordagens baseadas em formulários dentro de componentes e optei por um Minimal API. Essa decisão isolou a lógica de logout em um contexto puramente de backend, provando ser uma solução viável.

#### Desafios Encontrados:

* O principal desafio foi, sem dúvida, depurar a interação complexa entre os componentes interativos do Blazor Server, o sistema de Antiforgery e os endpoints estáticos do ASP.NET Core Identity. A resolução exigiu uma compreensão profunda do ciclo de vida da requisição HTTP em contraste com a conexão SignalR do Blazor.

#### Próximos Passos:

* O projeto está **funcional e seguro**.
* As últimas tarefas são a criação do arquivo `README.md` com as instruções de setup.