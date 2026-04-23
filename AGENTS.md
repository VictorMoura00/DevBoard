# DevBoard - Contexto do MVP para agentes de codigo

## Visao geral

DevBoard e um gerenciador de projetos pessoais e open source. A aplicacao deve permitir que o usuario registre projetos, acompanhe tarefas, organize prioridades e receba notificacoes simples sobre eventos relevantes.

O objetivo principal do projeto e servir como portfolio tecnico e ambiente de aprendizado para arquitetura em .NET, com evolucao planejada de um monolito modular para microservicos. O MVP deve nascer pequeno, funcional e facil de rodar localmente. Evite adicionar infraestrutura distribuida antes de existir uma necessidade clara.

## Objetivo do MVP

O MVP deve entregar um fluxo basico, completo e demonstravel:

1. Criar, listar, editar e arquivar projetos.
2. Criar, listar, editar e concluir tarefas vinculadas a projetos.
3. Filtrar tarefas por status, prioridade e projeto.
4. Registrar eventos internos quando tarefas forem criadas, concluidas ou tiverem vencimento alterado.
5. Exibir uma lista simples de notificacoes geradas a partir desses eventos.
6. Rodar localmente com poucos comandos, preferencialmente via Docker Compose para dependencias.

Nesta fase, notificacoes podem ser persistidas e exibidas na interface. Envio real por e-mail, webhook ou mensageria externa deve ficar para uma fase posterior.

## Decisao arquitetural inicial

Comecar como monolito modular.

Nao iniciar com microservicos separados para `projects`, `tasks` e `notifications`. A primeira versao deve manter os dominios no mesmo processo para reduzir complexidade operacional e acelerar a entrega do produto.

A arquitetura deve, entretanto, preservar boundaries claros para permitir a extracao futura do modulo de notificacoes como microservico.

Diretriz:

```text
MVP:
Angular -> DevBoard.Api -> PostgreSQL

Fase posterior:
Angular -> DevBoard.Api -> RabbitMQ -> DevBoard.Notifications.Api

Fase futura opcional:
Angular -> YARP Gateway
              -> Projects.Api
              -> Tasks.Api
              -> Notifications.Api
```

## Stack sugerida

Backend:

- .NET 10
- ASP.NET Core Minimal APIs
- Wolverine para comandos, handlers, eventos internos e futura mensageria
- EF Core
- PostgreSQL
- JWT auth, se houver autenticacao no MVP
- Docker Compose para PostgreSQL
- Health checks basicos

Frontend:

- Angular 21
- Angular Material ou PrimeNG
- RxJS e Signals quando fizer sentido
- Tela inicial ja deve ser o dashboard do produto, nao uma landing page

Infra posterior:

- RabbitMQ
- Wolverine RabbitMQ transport
- Outbox/inbox duravel
- Notifications API separada
- YARP apenas quando houver mais de uma API real
- OpenTelemetry, se o projeto ja tiver fluxo suficiente para justificar observabilidade

## Tecnologias que nao devem entrar no MVP

Evite adicionar no inicio:

- Microservicos separados para todos os dominios.
- YARP apontando para uma unica API.
- RabbitMQ antes de existir um fluxo de notificacao que justifique mensageria externa.
- Kubernetes.
- Service discovery.
- Marten.
- Event sourcing.
- CQRS complexo com modelos separados para toda leitura/escrita.
- Cache distribuido.
- Autenticacao social.
- Multitenancy.

Essas tecnologias podem ser consideradas depois que o MVP estiver funcionando.

## Estrutura sugerida do repositorio

Preferir uma estrutura explicita por modulos:

```text
src/
  DevBoard.Api/
    Program.cs
    Configuration/
    Endpoints/
    Middleware/

  DevBoard.Modules.Projects/
    ProjectsModule.cs
    Endpoints/
    Commands/
    Queries/
    Events/
    Data/
    Models/

  DevBoard.Modules.Tasks/
    TasksModule.cs
    Endpoints/
    Commands/
    Queries/
    Events/
    Data/
    Models/

  DevBoard.Modules.Notifications/
    NotificationsModule.cs
    Endpoints/
    Handlers/
    Events/
    Data/
    Models/

  DevBoard.SharedKernel/
    Errors/
    Messaging/
    Time/
    Results/

  DevBoard.Infrastructure/
    Database/
    Auth/
    Observability/

tests/
  DevBoard.UnitTests/
  DevBoard.IntegrationTests/

frontend/
  devboard-web/

docs/
  architecture.md
  adr/
    0001-start-as-modular-monolith.md
    0002-use-wolverine-for-messaging.md
```

Se a implementacao inicial ficar pequena demais para multiplos projetos `.csproj`, e aceitavel comecar com modulos como pastas dentro de `DevBoard.Api`. Ainda assim, manter separacao clara por dominio.

## Dominios do MVP

### Projects

Responsavel por projetos pessoais ou open source.

Campos iniciais sugeridos:

- `Id`
- `Name`
- `Description`
- `RepositoryUrl`
- `Status`
- `Tags`
- `CreatedAt`
- `UpdatedAt`
- `ArchivedAt`

Status sugeridos:

- `Active`
- `Paused`
- `Completed`
- `Archived`

Endpoints iniciais:

```text
GET    /api/projects
GET    /api/projects/{id}
POST   /api/projects
PUT    /api/projects/{id}
POST   /api/projects/{id}/archive
```

### Tasks

Responsavel por tarefas vinculadas a projetos.

Campos iniciais sugeridos:

- `Id`
- `ProjectId`
- `Title`
- `Description`
- `Status`
- `Priority`
- `DueDate`
- `CreatedAt`
- `UpdatedAt`
- `CompletedAt`

Status sugeridos:

- `Todo`
- `InProgress`
- `Done`
- `Canceled`

Prioridades sugeridas:

- `Low`
- `Medium`
- `High`
- `Critical`

Endpoints iniciais:

```text
GET    /api/tasks
GET    /api/projects/{projectId}/tasks
GET    /api/tasks/{id}
POST   /api/tasks
PUT    /api/tasks/{id}
POST   /api/tasks/{id}/complete
```

### Notifications

No MVP, notificacoes sao registros internos criados por handlers de eventos.

Eventos iniciais que podem gerar notificacoes:

- `TaskCreated`
- `TaskCompleted`
- `TaskDueDateChanged`
- `ProjectArchived`

Campos iniciais sugeridos:

- `Id`
- `Type`
- `Title`
- `Message`
- `IsRead`
- `CreatedAt`
- `ReadAt`
- `RelatedProjectId`
- `RelatedTaskId`

Endpoints iniciais:

```text
GET    /api/notifications
POST   /api/notifications/{id}/read
POST   /api/notifications/read-all
```

## Uso do Wolverine

Usar Wolverine como mediator interno no MVP.

Preferir comandos e handlers para casos de uso com regra de negocio:

```text
CreateProject
UpdateProject
ArchiveProject
CreateTask
UpdateTask
CompleteTask
MarkNotificationAsRead
```

Endpoints Minimal API devem ser finos e delegar trabalho aos handlers via Wolverine.

Eventos de dominio podem ser publicados internamente para desacoplar modulos. Exemplo:

```text
CompleteTask command
  -> atualiza tarefa
  -> publica TaskCompleted
  -> Notifications handler cria notificacao
```

No MVP, nao exigir RabbitMQ. A integracao externa com RabbitMQ deve ser uma evolucao natural quando `Notifications` for extraido para servico separado.

## Persistencia

Usar PostgreSQL.

No MVP, usar um unico banco:

```text
devboard_db
```

Opcoes aceitaveis:

1. Tabelas simples no schema publico, se o projeto ainda estiver pequeno.
2. Schemas por modulo, se a separacao modular ja estiver clara:

```text
projects.*
tasks.*
notifications.*
```

Nao criar um banco separado por modulo no MVP. Reservar banco por servico para a fase de microservicos.

## Autenticacao

Autenticacao e opcional no primeiro corte do MVP.

Se for implementada, manter simples:

- Cadastro e login com e-mail/senha.
- JWT bearer.
- Um usuario dono dos seus projetos e tarefas.

Se autenticacao atrasar o MVP, criar a primeira versao single-user e documentar que auth entra na proxima fase.

## Frontend inicial

A primeira tela deve ser o produto funcionando.

Telas sugeridas:

- Dashboard com resumo de projetos, tarefas abertas e notificacoes.
- Lista de projetos.
- Detalhe do projeto com tarefas.
- Lista ou painel de tarefas.
- Central simples de notificacoes.

Evitar landing page ou telas de marketing. O foco e demonstrar a aplicacao.

## Qualidade minima esperada

O MVP deve conter:

- README com como rodar localmente.
- Docker Compose para PostgreSQL.
- Migrations do EF Core.
- Seed opcional com dados de exemplo.
- Validacao basica de requests.
- ProblemDetails para erros de API.
- Health check basico.
- Testes de unidade para regras principais.
- Testes de integracao para pelo menos um fluxo importante.

Fluxo importante sugerido para teste de integracao:

```text
Criar projeto
Criar tarefa no projeto
Concluir tarefa
Verificar notificacao criada
```

## Ordem de implementacao recomendada

1. Criar solucao .NET e API minima.
2. Configurar PostgreSQL via Docker Compose.
3. Criar modulo Projects com CRUD basico.
4. Criar modulo Tasks com CRUD basico e vinculo com Project.
5. Adicionar Wolverine para comandos e handlers.
6. Criar eventos internos de tarefas.
7. Criar modulo Notifications consumindo eventos internos.
8. Criar frontend Angular com dashboard simples.
9. Adicionar testes basicos.
10. Documentar decisoes arquiteturais em ADRs.

## Criterios de pronto do MVP

O MVP esta pronto quando:

- A aplicacao roda localmente com instrucoes claras.
- O usuario consegue criar um projeto.
- O usuario consegue criar tarefas para esse projeto.
- O usuario consegue concluir uma tarefa.
- A conclusao da tarefa gera uma notificacao visivel.
- O codigo mantem separacao clara entre `Projects`, `Tasks` e `Notifications`.
- Nao ha dependencia obrigatoria de RabbitMQ, YARP ou microservicos para rodar a primeira versao.

## Evolucao planejada pos-MVP

Depois que o MVP estiver estavel:

1. Adicionar RabbitMQ.
2. Configurar Wolverine com transporte RabbitMQ.
3. Adicionar outbox/inbox duravel.
4. Extrair `Notifications` para `DevBoard.Notifications.Api`.
5. Separar banco de notificacoes.
6. Adicionar YARP quando houver pelo menos duas APIs chamadas diretamente pelo frontend.
7. Adicionar observabilidade com logs estruturados, tracing e metricas.
8. Melhorar autenticacao e autorizacao.

## Principio guia

Priorize produto funcionando e boundaries claros.

Este projeto deve demonstrar criterio arquitetural, nao apenas quantidade de ferramentas. Sempre que uma tecnologia nova for adicionada, ela deve resolver um problema real do estado atual da aplicacao ou preparar uma evolucao claramente documentada.
