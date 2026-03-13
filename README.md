# Recovery Fiscal API

API REST desenvolvida em **.NET 8** para a gestão de processos de recuperação fiscal, concebida com foco em **clareza arquitectural**, **boas práticas de engenharia**, **testabilidade** e **evolução segura** do produto.

> Este projecto foi estruturado para demonstrar capacidade de desenho técnico, organização de solução enterprise e atenção a requisitos comuns em ambientes regulados, como o sector bancário.

---

## Sumário

- [Visão Geral](#visão-geral)
- [Justificação da Arquitectura](#justificação-da-arquitectura)
- [Princípios de Design Adoptados](#princípios-de-design-adoptados)
- [Estrutura da Solução](#estrutura-da-solução)
- [Principais Decisões Técnicas](#principais-decisões-técnicas)
- [API e Contratos](#api-e-contratos)
- [Exemplos de Payload](#exemplos-de-payload)
- [Códigos de Resposta](#códigos-de-resposta)
- [Execução Local](#execução-local)
- [Base de Dados e Migrations](#base-de-dados-e-migrations)
- [Testes](#testes)
- [Melhorias Futuras](#melhorias-futuras)

---

## Visão Geral

O sistema gere **processos de recuperação fiscal**, em que cada processo representa a análise de créditos recuperáveis. A solução expõe uma **API REST** com operações de CRUD, contemplando preocupações relevantes num contexto empresarial:

- modelação clara do domínio;
- separação de responsabilidades;
- validação de entrada;
- paginação, filtragem e ordenação;
- tratamento consistente de erros;
- suporte à evolução incremental;
- containerização para execução previsível.

---

## Justificação da Arquitectura

Foi adoptada uma arquitectura em camadas, com separação entre **Api**, **Application**, **Domain** e **Infrastructure**.

### Motivos para esta decisão

#### 1. Separação clara de responsabilidades

Cada camada possui um papel bem definido:

- **Api**: exposição HTTP, versionamento, middleware e contratos externos;
- **Application**: regras de aplicação, casos de uso, validações e orquestração;
- **Domain**: entidades, enums e conceitos centrais do negócio;
- **Infrastructure**: persistência, Entity Framework Core, repositórios e integrações técnicas.

Esta organização reduz o acoplamento e torna o projecto mais legível, sustentável e preparado para manutenção e evolução.

#### 2. Melhor testabilidade

Ao isolar as regras de negócio e os contratos de persistência, torna-se mais simples validar comportamentos através de **testes unitários** e **testes de integração**.

#### 3. Evolução segura

Num contexto bancário, é comum que novos requisitos surjam com frequência, como autenticação, auditoria detalhada, integração com sistemas internos, mensageria, observabilidade e versionamento adicional. Esta estrutura facilita essas extensões sem comprometer o núcleo da solução.

#### 4. Menor dependência da tecnologia de persistência

A camada de aplicação depende de abstrações, e não da implementação concreta do acesso a dados. Isso preserva o desenho da solução e reduz o impacto caso a estratégia de persistência precise de evoluir.

---

## Princípios de Design Adoptados

### Single Responsibility

Cada componente foi desenhado para cumprir uma responsabilidade principal. Os controladores não concentram regras de negócio, os repositórios não definem regras de validação e o domínio permanece independente da infraestrutura.

### Baixo acoplamento

A camada de aplicação trabalha com **interfaces** e abstrações, o que melhora a flexibilidade e simplifica a execução de testes.

### Coesão

As funcionalidades associadas ao processo de recuperação fiscal foram agrupadas em módulos específicos, tornando a navegação pela solução mais intuitiva e consistente.

### Extensibilidade

A solução já nasce preparada para evoluir com:

- autenticação e autorização;
- auditoria mais rica;
- eventos de domínio;
- filas e mensageria;
- novos endpoints versionados;
- novos módulos de negócio.

---

## Estrutura da Solução

```text
RecoveryFiscal/
├── Directory.Build.props
├── Dockerfile
├── README.md
├── RecoveryFiscal.sln
├── docker-compose.yml
├── src/
│   ├── RecoveryFiscal.Api/
│   │   ├── Controllers/
│   │   │   └── ProcessosRecuperacaoFiscalController.cs
│   │   ├── Middlewares/
│   │   │   ├── CorrelationIdMiddleware.cs
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── Program.cs
│   │   ├── RecoveryFiscal.Api.csproj
│   │   ├── appsettings.Development.json
│   │   └── appsettings.json
│   ├── RecoveryFiscal.Application/
│   │   ├── Common/
│   │   │   ├── Abstractions/
│   │   │   ├── Exceptions/
│   │   │   └── Models/
│   │   ├── ProcessosRecuperacaoFiscal/
│   │   │   ├── Models/
│   │   │   ├── Services/
│   │   │   └── Validators/
│   │   ├── DependencyInjection.cs
│   │   └── RecoveryFiscal.Application.csproj
│   ├── RecoveryFiscal.Domain/
│   │   ├── Common/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── RecoveryFiscal.Domain.csproj
│   └── RecoveryFiscal.Infrastructure/
│       ├── DependencyInjection.cs
│       ├── Persistence/
│       │   ├── AppDbContext.cs
│       │   ├── Configurations/
│       │   ├── DesignTimeDbContextFactory.cs
│       │   ├── Migrations/
│       │   └── Repositories/
│       ├── RecoveryFiscal.Infrastructure.csproj
│       └── Services/
└── tests/
    ├── RecoveryFiscal.IntegrationTests/
    └── RecoveryFiscal.UnitTests/
```

---

## Principais Decisões Técnicas

### .NET 8

Foi escolhida a versão **.NET 8** por oferecer:

- suporte LTS;
- melhor desempenho;
- ecossistema moderno;
- maturidade para APIs empresariais.

### ASP.NET Core Web API

Tecnologia adequada para expor serviços REST com suporte a versionamento, middleware, serialização, Swagger/OpenAPI e integração natural com políticas de validação e tratamento de erros.

### Entity Framework Core + MySQL

O EF Core foi utilizado pela produtividade que oferece na modelação, no acesso a dados e na gestão de migrations. O MySQL foi mantido em conformidade com o requisito do desafio.

### Docker + Docker Compose

A containerização foi adoptada para reduzir diferenças entre ambientes e facilitar a avaliação técnica por terceiros.

### Soft Delete

A remoção lógica foi escolhida para preservar histórico e evitar perda irreversível de registos, prática comum em cenários corporativos e auditáveis.

### Versionamento da API

O versionamento prepara a solução para a evolução do contrato sem quebra imediata para consumidores externos.

### Tratamento centralizado de excepções

A utilização de middleware dedicado melhora a consistência das respostas e reduz a duplicação de código nos controladores.

### Validação com FluentValidation

As validações foram isoladas da camada HTTP, permitindo maior reutilização e testes mais simples.

---

## API e Contratos

### Endpoints principais

- `POST /api/v1/processos-recuperacao-fiscal`
- `GET /api/v1/processos-recuperacao-fiscal/{id}`
- `GET /api/v1/processos-recuperacao-fiscal`
- `PUT /api/v1/processos-recuperacao-fiscal/{id}`
- `PATCH /api/v1/processos-recuperacao-fiscal/{id}`
- `DELETE /api/v1/processos-recuperacao-fiscal/{id}`

### Funcionalidades incluídas

- criação de processo;
- consulta individual;
- listagem com paginação;
- ordenação e filtragem;
- actualização total e parcial;
- remoção lógica;
- respostas padronizadas por status code.

---

## Exemplos de Payload

### Exemplo de criação

```json
{
  "numeroProcesso": "RF-2026-0001",
  "nifCliente": "123456789",
  "nomeCliente": "Empresa Alfa, Lda",
  "tipoCredito": 1,
  "valorOriginalCredito": 150000.0,
  "valorRecuperavelEstimado": 98000.0,
  "dataConstituicaoCredito": "2025-12-10",
  "dataAnalise": "2026-03-10",
  "statusProcesso": 1,
  "prioridade": 3,
  "observacoes": "Processo elegível para recuperação parcial.",
  "ativo": true
}
```

### Exemplo de PATCH

```json
[
  {
    "op": "replace",
    "path": "/observacoes",
    "value": "Observação actualizada"
  },
  {
    "op": "replace",
    "path": "/statusProcesso",
    "value": 2
  }
]
```

---

## Códigos de Resposta

- `201 Created` para criação de recurso;
- `200 OK` para leitura, listagem e actualização;
- `204 No Content` para remoção lógica com sucesso;
- `404 Not Found` quando o recurso não existe;
- `409 Conflict` para conflitos de unicidade;
- `422 Unprocessable Entity` para falhas de validação;
- `500 Internal Server Error` para erros não tratados.

---

## Execução Local

### Subir o ambiente com Docker

```bash
docker compose up -d --build
```

### Swagger

```text
http://localhost:8080/swagger
```

---

## Base de Dados e Migrations

### Aplicar migrations

```bash
dotnet ef database update --project src/RecoveryFiscal.Infrastructure --startup-project src/RecoveryFiscal.Api
```

---

## Testes

### Testes unitários

```bash
dotnet test tests/RecoveryFiscal.UnitTests/RecoveryFiscal.UnitTests.csproj
```

### Testes de integração

```bash
dotnet test tests/RecoveryFiscal.IntegrationTests/RecoveryFiscal.IntegrationTests.csproj
```

### Executar todos os testes

```bash
dotnet test
```

---

## Melhorias Futuras

Para uma versão seguinte, as evoluções mais naturais seriam:

- autenticação e autorização com JWT/OAuth2;
- auditoria detalhada por utilizador e operação;
- observabilidade com logging estruturado e tracing;
- health checks completos para base de dados e dependências externas;
- paginação com metadados mais ricos;
- documentação OpenAPI mais detalhada;
- pipeline de CI/CD;
- cobertura de testes mais abrangente;
- políticas de retry e resiliência;
- segregação entre comandos e consultas, caso o domínio cresça.

---

## Considerações Finais

Este projecto foi construído para demonstrar não apenas a implementação funcional de uma API, mas também uma forma profissional de pensar a solução: com estrutura, previsibilidade, preocupação com manutenção e uma base sólida para evolução.

Num contexto de avaliação técnica, a principal intenção foi evidenciar:

- capacidade de modelar uma solução de forma organizada;
- preocupação com padrões de mercado;
- domínio de boas práticas em .NET;
- sensibilidade para requisitos típicos de ambientes empresariais.
