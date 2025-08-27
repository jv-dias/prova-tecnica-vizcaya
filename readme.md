# Sistema de Gestão Acadêmica (Prova Técnica)

Este projeto foi desenvolvido como uma prova técnica para a vaga de Desenvolvedor .NET (Blazor Server). Trata-se de uma aplicação web para a gestão de alunos, turmas e matrículas, construída com Blazor Server e ASP.NET Core Identity.

## Funcionalidades

* **CRUD de Alunos:** Criação, leitura, atualização e exclusão de alunos, com listagem paginada e filtros dinâmicos por nome e CPF.
* **CRUD de Turmas:** Gerenciamento completo de turmas.
* **Gestão de Matrículas:** Funcionalidade para matricular alunos em turmas, com regras de negócio como a validação de alunos ativos.
* **Consumo de API Externa:** Integração com a API ViaCEP para preenchimento automático de endereço a partir do CEP, com cache em memória para otimização de performance.
* **Importação em Lote via CSV:** Uma tela de importação interativa que permite o upload de um arquivo CSV, processa automaticamente novos alunos e apresenta uma tela de revisão para que o usuário resolva conflitos de CPFs duplicados.
* **Autenticação e Autorização:** Sistema de segurança completo baseado em perfis (Roles) "Admin" e "Professor", controlando o acesso a páginas e funcionalidades específicas.
* **Dashboard:** Página inicial com um painel de resumo exibindo estatísticas do sistema.

## Tecnologias Utilizadas

* **Framework:** .NET 8
* **UI:** Blazor Server
* **Banco de Dados:** SQL Server rodando em um contêiner Docker
* **ORM:** Entity Framework Core 8
* **Autenticação/Autorização:** ASP.NET Core Identity
* **Bibliotecas de Terceiros:**
    * **CsvHelper:** Para leitura e parsing de arquivos CSV.
    * **Bogus:** Para geração de dados de teste (Seed).

## Como Executar o Projeto (Setup)

Siga os passos abaixo para configurar e executar a aplicação em seu ambiente de desenvolvimento.

### 1. Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* Uma IDE para desenvolvimento C# (como [Visual Studio](https://visualstudio.microsoft.com/) ou [JetBrains Rider](https://www.jetbrains.com/rider/)) ou um editor de código como o [VS Code](https://code.visualstudio.com/).

### 2. Clonar o Repositório

```bash
git clone <URL_DO_SEU_REPOSITORIO>
cd <NOME_DA_PASTA_DO_PROJETO>
```

### 3. Iniciar o Banco de Dados (Docker)

Execute o comando abaixo no seu terminal para iniciar um contêiner do SQL Server. A aplicação está configurada para se conectar a ele.

```bash
docker run --name sql-server-dev -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Password123" -p 1433:1433 -d [mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)
```

### 4. Configurar a String de Conexão

O projeto já vem com a string de conexão correta para o contêiner Docker. Verifique o arquivo `ProvaTecnica.Web/appsettings.json` e garanta que ela está de acordo com a senha que você definiu no comando Docker.

A senha padrão configurada é `YourStrong@Password123`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProvaTecnicaDb;User Id=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
  }
}
```

### 5. Executar a Aplicação

Você pode executar o projeto através da sua IDE (Visual Studio, Rider) ou via linha de comando. Ao ser executada pela primeira vez, as Migrations do Entity Framework serão aplicadas, e o banco de dados será criado e populado (Seed) automaticamente.

```bash
dotnet run --project ProvaTecnica.Web
```

A aplicação estará disponível em `https://localhost:XXXX` e `http://localhost:YYYY` (as portas serão indicadas no seu terminal).

## Credenciais de Teste

O sistema é populado com dois usuários de teste com diferentes níveis de permissão:

* **Perfil Administrador** (acesso a tudo):
    * **Usuário:** `admin@teste.com`
    * **Senha:** `MeContrata@123`

* **Perfil Professor** (acesso restrito à gestão de Alunos):
    * **Usuário:** `professor@teste.com`
    * **Senha:** `MeContrata@123`

---
*Este projeto foi desenvolvido por João Victor da Silva Dias.*