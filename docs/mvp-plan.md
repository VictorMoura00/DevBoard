# DevBoard MVP Plan

## Estado atual

O projeto ja esta organizado como um monolito modular inicial, com um host web unico e modulos separados por dominio.

```text
DevBoard.Api
DevBoard.Modules.Projects
DevBoard.Modules.Tasks
DevBoard.Modules.Notifications
DevBoard.SharedKernel
DevBoard.Infrastructure
```

Ja existe:

- Separacao inicial por modulos
- `Program.cs` compondo os modulos
- `SharedKernel` e `Infrastructure`
- `health check` e OpenAPI basicos
- Estrutura de solution organizada

Ainda faltam os elementos que tornam o MVP funcional de ponta a ponta:

- Persistencia com PostgreSQL e EF Core
- Endpoints reais
- Comandos e handlers
- Eventos internos
- Notificacoes persistidas
- Frontend Angular
- Testes
- Docker Compose e README operacional

## Objetivo do MVP

O MVP estara pronto quando permitir:

1. Criar, listar, editar e arquivar projetos.
2. Criar, listar, editar e concluir tarefas vinculadas a projetos.
3. Filtrar tarefas por status, prioridade e projeto.
4. Registrar eventos internos para acoes relevantes.
5. Gerar notificacoes internas a partir desses eventos.
6. Exibir tudo isso em uma interface simples.

## Roadmap

## Fase 1 - Base tecnica

Objetivo: preparar a aplicacao para rodar localmente com persistencia real.

Entregas:

- Adicionar `docker-compose` para PostgreSQL
- Adicionar EF Core e provider Npgsql
- Criar `DevBoardDbContext`
- Configurar connection string
- Registrar banco em `DevBoard.Infrastructure`
- Criar migrations iniciais
- Adicionar `health check` do banco
- Criar README inicial com setup local

Resultado esperado:

```text
docker compose up -d
dotnet run --project src/DevBoard.Api
```

A API sobe, conecta ao banco e responde em `/health`.

## Fase 2 - Modulo Projects

Objetivo: entregar o CRUD basico de projetos.

Endpoints:

```text
GET    /api/projects
GET    /api/projects/{id}
POST   /api/projects
PUT    /api/projects/{id}
POST   /api/projects/{id}/archive
```

Estrutura sugerida:

```text
DevBoard.Modules.Projects/
  Endpoints/
  Commands/
  Queries/
  Events/
  Data/
  Models/
  Handlers/
```

Entregas:

- Entidade `Project`
- Mapeamento EF Core
- DTOs de request/response
- Endpoints Minimal API
- Comandos de criar, atualizar e arquivar
- Regras basicas de validacao

Resultado esperado:

O usuario consegue criar, listar, editar e arquivar projetos pela API.

## Fase 3 - Wolverine como mediator interno

Objetivo: deixar endpoints finos e alinhar a arquitetura com o contexto do projeto.

Entregas:

- Adicionar Wolverine
- Configurar Wolverine no host
- Fazer endpoints delegarem para comandos/handlers
- Preparar publicacao de eventos internos

Fluxo esperado:

```text
POST /api/projects
  -> CreateProject
  -> handler
  -> persistencia
```

Resultado esperado:

Regras de negocio deixam de ficar espalhadas nos endpoints.

## Fase 4 - Modulo Tasks

Objetivo: permitir gerenciar tarefas vinculadas a projetos.

Endpoints:

```text
GET    /api/tasks
GET    /api/projects/{projectId}/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
POST   /api/tasks/{id}/complete
```

Filtros minimos:

- `status`
- `priority`
- `projectId`

Entregas:

- Entidade `ProjectTask`
- Relacao com `Project`
- CRUD de tarefas
- Endpoint de conclusao
- Validacoes basicas
- Filtro por status, prioridade e projeto

Eventos internos planejados:

- `TaskCreated`
- `TaskCompleted`
- `TaskDueDateChanged`

Resultado esperado:

O usuario cria tarefas para projetos e consegue conclui-las.

## Fase 5 - Modulo Notifications

Objetivo: transformar eventos internos em notificacoes persistidas.

Endpoints:

```text
GET    /api/notifications
POST   /api/notifications/{id}/read
POST   /api/notifications/read-all
```

Entregas:

- Entidade `Notification`
- Persistencia de notificacoes
- Handlers de eventos internos
- Geracao de notificacoes para eventos principais

Eventos que devem gerar notificacao:

- `TaskCreated`
- `TaskCompleted`
- `TaskDueDateChanged`
- `ProjectArchived`

Resultado esperado:

Ao concluir tarefa, alterar vencimento ou arquivar projeto, uma notificacao aparece na API.

## Fase 6 - Validacao e erros

Objetivo: deixar a API minimamente consistente e previsivel.

Entregas:

- Requests e responses separados das entidades
- Validacao basica de payloads
- Uso consistente de `ProblemDetails`
- Padronizacao de erros de dominio
- Respostas HTTP coerentes

Resposta esperada por tipo de erro:

- `400` para request invalido
- `404` para recurso nao encontrado
- `201` para criacao
- `204` para comandos sem corpo de retorno

## Fase 7 - Testes

Objetivo: garantir confianca no fluxo principal.

Estrutura:

```text
tests/
  DevBoard.UnitTests/
  DevBoard.IntegrationTests/
```

Testes de unidade sugeridos:

- Arquivar projeto altera status e `ArchivedAt`
- Concluir tarefa altera status e `CompletedAt`
- Alterar vencimento publica evento esperado
- Marcar notificacao como lida define `IsRead` e `ReadAt`

Teste de integracao minimo:

```text
Criar projeto
Criar tarefa no projeto
Concluir tarefa
Verificar notificacao criada
```

Resultado esperado:

O fluxo principal do MVP fica protegido por testes.

## Fase 8 - Frontend Angular

Objetivo: entregar uma interface simples e demonstravel.

Estrutura:

```text
frontend/
  devboard-web/
```

Telas do MVP:

- Dashboard
- Lista de projetos
- Detalhe do projeto com tarefas
- Lista/painel de tarefas
- Central de notificacoes

Capacidades minimas:

- Criar e editar projetos
- Criar e editar tarefas
- Concluir tarefa
- Filtrar tarefas
- Visualizar notificacoes
- Marcar notificacoes como lidas

Resultado esperado:

O produto pode ser demonstrado sem depender apenas do Swagger.

## Fase 9 - Acabamento do MVP

Objetivo: deixar o repositorio pronto para uso e apresentacao.

Entregas:

- README completo
- ADRs principais
- Seed opcional com dados de exemplo
- OpenAPI revisado
- Instrucoes de setup local
- Instrucoes para migrations e testes

Importante:

Nesta fase o projeto ainda nao deve depender obrigatoriamente de:

- RabbitMQ
- YARP
- Kubernetes
- Microservicos separados

## Ordem recomendada de implementacao

1. PostgreSQL + EF Core + migrations
2. Projects
3. Wolverine
4. Tasks
5. Eventos internos
6. Notifications
7. Validacao + ProblemDetails
8. Testes
9. Frontend Angular
10. README + ADRs + acabamento

## Criterio de pronto do MVP

O MVP estara pronto quando este fluxo funcionar de ponta a ponta:

```text
Criar projeto
Criar tarefa no projeto
Concluir tarefa
Gerar notificacao automaticamente
Visualizar notificacao na interface
```

E quando o projeto puder ser executado localmente com poucos comandos:

```text
docker compose up -d
dotnet run --project src/DevBoard.Api
dotnet test
```

## Principio guia

Priorizar produto funcionando com boundaries claros.

O foco do DevBoard no MVP e demonstrar criterio arquitetural, boa separacao de responsabilidades e uma base que possa evoluir para RabbitMQ, extracao de servicos e outras capacidades so quando houver necessidade real.
