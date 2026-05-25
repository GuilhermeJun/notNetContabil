# Sistema Contabil API

API REST para gerenciamento de um sistema contabil, desenvolvida em .NET 9 com Minimal APIs, Entity Framework Core e Oracle Database.

## Visao Geral

O projeto organiza os recursos principais de um sistema contabil e comercial:

- Clientes
- Contas contabeis
- Registros contabeis
- Produtos
- Pagamentos
- Vendas
- Itens de venda

A API tambem inclui documentacao OpenAPI com Scalar, health check, logs estruturados com Serilog, endpoint de diagnostico de banco e testes de integracao com xUnit.

## Tecnologias

- .NET 9
- ASP.NET Core Minimal APIs
- Entity Framework Core
- Oracle Entity Framework Core
- Oracle Database
- Scalar/OpenAPI
- Serilog
- Health Checks
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- EF Core InMemory para testes

## Estrutura do Projeto

```text
.
|-- src
|   |-- SistemaContabil.Api              # API, endpoints, configuracao e OpenAPI
|   |-- SistemaContabil.Application      # DTOs e contratos de aplicacao
|   |-- SistemaContabil.Domain           # Entidades, interfaces e servicos de dominio
|   `-- SistemaContabil.Infrastructure   # DbContext, migrations e configuracao EF Core
|-- SistemaContabil.Test                 # Testes xUnit de integracao
|-- docs                                 # Documentacao complementar
`-- SistemaContabil.sln
```

## Configuracao

A API usa a connection string `FiapOracle`.

Em desenvolvimento, configure em `src/SistemaContabil.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "FiapOracle": "Data Source=//host:1521/service;User Id=usuario;Password=senha;"
  }
}
```

Recomendacao: para publicar o projeto no GitHub, nao versionar senhas reais. Prefira variaveis de ambiente, user secrets ou um arquivo local nao commitado.

Exemplo com user secrets:

```bash
dotnet user-secrets set "ConnectionStrings:FiapOracle" "Data Source=//host:1521/service;User Id=usuario;Password=senha;" --project src/SistemaContabil.Api
```

## Como Executar

Restaure, compile e execute a API:

```bash
dotnet restore SistemaContabil.sln
dotnet build SistemaContabil.sln
dotnet run --project src/SistemaContabil.Api
```

Perfis locais configurados:

- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

## URLs Uteis

- API root: `GET /`
- Scalar/OpenAPI: `/scalar/v1`
- Health check: `GET /health`
- Health UI: `GET /health-ui`
- Diagnostico do banco: `GET /diag/database`

O endpoint `/diag/database` ajuda a conferir qual provider, connection string mascarada, usuario/schema Oracle e tabela de clientes estao sendo usados pela API em runtime.

## Endpoints Principais

### Clientes

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/clientes` | Lista todos os clientes |
| GET | `/clientes/ativos` | Lista clientes ativos |
| GET | `/clientes/{id}` | Busca cliente por id com HATEOAS |
| POST | `/clientes` | Cria cliente |
| PUT | `/clientes/{id}` | Atualiza cliente |
| DELETE | `/clientes/{id}` | Remove cliente |

Exemplo de POST:

```http
POST /clientes
Content-Type: application/json

{
  "nome": "Cliente Teste",
  "cpf": "12345678901",
  "email": "cliente@teste.com",
  "senha": "senha123",
  "ativo": "S"
}
```

### Contas

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/contas` | Lista contas |
| GET | `/contas/receitas` | Lista contas de receita |
| GET | `/contas/despesas` | Lista contas de despesa |
| GET | `/contas/{id}` | Busca conta por id |
| POST | `/contas` | Cria conta |
| PUT | `/contas/{id}` | Atualiza conta |
| DELETE | `/contas/{id}` | Remove conta |

### Registros Contabeis

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/registros-contabeis` | Lista registros |
| GET | `/registros-contabeis/{id}` | Busca registro por id |
| POST | `/registros-contabeis` | Cria registro |
| PUT | `/registros-contabeis/{id}` | Atualiza registro |
| DELETE | `/registros-contabeis/{id}` | Remove registro |

### Produtos

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/produtos` | Lista produtos |
| GET | `/produtos/{id}` | Busca produto por id |
| POST | `/produtos` | Cria produto |
| PUT | `/produtos/{id}` | Atualiza produto |
| DELETE | `/produtos/{id}` | Remove produto |

### Pagamentos

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/pagamentos` | Lista pagamentos |
| GET | `/pagamentos/{id}` | Busca pagamento por id |
| POST | `/pagamentos` | Cria pagamento |

### Vendas

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/vendas` | Lista vendas |
| GET | `/vendas/{id}` | Busca venda por id |
| POST | `/vendas` | Cria venda |

### Itens de Venda

| Metodo | Rota | Descricao |
| --- | --- | --- |
| GET | `/itens-venda` | Lista itens de venda |
| GET | `/itens-venda/{id}` | Busca item por id |
| POST | `/itens-venda` | Cria item |
| PUT | `/itens-venda/{id}` | Atualiza item |

## HATEOAS

O endpoint `GET /clientes/{id}` retorna links HATEOAS para facilitar a descoberta das proximas acoes disponiveis.

Exemplo parcial:

```json
{
  "id": 1,
  "nome": "Cliente Teste",
  "_links": [
    {
      "rel": "self",
      "href": "https://localhost:5001/clientes/1",
      "method": "GET"
    },
    {
      "rel": "clientes",
      "href": "https://localhost:5001/clientes",
      "method": "GET"
    },
    {
      "rel": "atualizar",
      "href": "https://localhost:5001/clientes/1",
      "method": "PUT"
    },
    {
      "rel": "excluir",
      "href": "https://localhost:5001/clientes/1",
      "method": "DELETE"
    }
  ]
}
```

## Banco de Dados

O banco principal e Oracle. As entidades usam mapeamento por atributos, por exemplo:

- `Cliente` -> tabela `cliente`
- `Conta` -> tabela `conta_contabil`
- `RegistroContabil` -> tabela `reg_cont`
- `Venda` -> tabela `vendas`
- `Produto` -> tabela `produto`
- `Pagamento` -> tabela `pagamento`
- `ItemVenda` -> tabela `item_venda`

Observacao: no Oracle, nomes nao delimitados costumam aparecer em maiusculo. A tabela `cliente` pode aparecer como `CLIENTE`.

Para confirmar onde a API esta gravando:

```http
GET /diag/database
```

Depois consulte no Oracle usando o schema retornado em `currentSchema`:

```sql
SELECT *
FROM SEU_SCHEMA.CLIENTE
ORDER BY ID_CLIENTE DESC;
```

## Testes

Os testes usam xUnit, `WebApplicationFactory` e banco InMemory para validar a API sem depender do Oracle.

Executar todos os testes:

```bash
dotnet test SistemaContabil.sln
```

Coberturas atuais:

- `GET /clientes/{id}` retorna cliente com links HATEOAS
- `GET /clientes/{id}` retorna `404 NotFound` quando o cliente nao existe

Os testes seguem o padrao AAA:

- Arrange
- Act
- Assert

## Logs e Monitoramento

A aplicacao usa Serilog com saida no console e em arquivo rotativo diario. Tambem possui health check em:

```http
GET /health
```

## Comandos Uteis

```bash
# Restaurar dependencias
dotnet restore SistemaContabil.sln

# Compilar
dotnet build SistemaContabil.sln

# Executar API
dotnet run --project src/SistemaContabil.Api

# Executar testes
dotnet test SistemaContabil.sln

# Criar migration EF Core
dotnet ef migrations add NomeDaMigration --project src/SistemaContabil.Infrastructure --startup-project src/SistemaContabil.Api

# Aplicar migrations
dotnet ef database update --project src/SistemaContabil.Infrastructure --startup-project src/SistemaContabil.Api
```

## Status

Projeto em desenvolvimento academico para pratica de .NET, API REST, Oracle, HATEOAS, testes de integracao e Clean Architecture.
