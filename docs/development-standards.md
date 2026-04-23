# DevBoard Development Standards

## Objetivo

Este documento define os principios de implementacao que devem guiar a evolucao do DevBoard durante o MVP.

O objetivo nao e aumentar cerimonia. O objetivo e manter o codigo simples de entender, facil de alterar e com baixo acoplamento entre modulos.

## Principios obrigatorios

### SOLID

O projeto deve aplicar os principios SOLID de forma pragmatica.

#### Single Responsibility Principle

Cada classe, handler, endpoint mapper, servico ou componente deve ter uma responsabilidade clara.

Exemplos:

- endpoints mapeiam rotas e delegam trabalho
- handlers executam casos de uso
- entidades representam estado e comportamento do dominio
- infraestrutura configura acesso a recursos externos

Evitar classes que:

- validam request
- executam regra de negocio
- persistem dados
- montam resposta HTTP

tudo ao mesmo tempo.

#### Open/Closed Principle

O codigo deve preferir extensao por composicao e pequenas abstracoes, evitando alterar blocos centrais a cada nova funcionalidade.

Exemplos:

- novos handlers de eventos nao devem exigir reescrever modulos existentes
- novas validacoes devem ser adicionadas sem duplicar fluxo inteiro
- novos endpoints devem seguir os mesmos padroes do modulo

#### Liskov Substitution Principle

Quando houver heranca ou contratos compartilhados, implementacoes concretas devem respeitar o comportamento esperado pela abstracao.

Diretriz:

- preferir composicao a heranca quando a heranca nao trouxer ganho real

#### Interface Segregation Principle

Interfaces devem ser pequenas e orientadas ao caso de uso.

Evitar interfaces genericas e inchadas como:

```text
IRepository<T>
IServiceManager
IBaseService
```

Preferir contratos especificos e pequenos quando eles realmente forem necessarios.

#### Dependency Inversion Principle

Modulos de alto nivel nao devem depender diretamente de detalhes de infraestrutura.

Exemplos:

- regras de negocio nao devem conhecer detalhes do EF Core
- handlers nao devem depender de implementacoes concretas quando houver colaboracoes relevantes
- infraestrutura depende do dominio e da aplicacao, nao o contrario

## DRY

O projeto deve evitar duplicacao de regra, fluxo e estrutura acidental.

DRY nao significa criar abstracoes cedo demais. Significa evitar repetir conhecimento no sistema.

### O que deve ser evitado

- validacoes repetidas em varios endpoints
- mapeamentos iguais espalhados em varios arquivos
- tratamento de erro inconsistente entre modulos
- logica duplicada para timestamps, status e transicoes
- consultas parecidas copiadas e adaptadas manualmente

### O que deve ser feito

- extrair helpers ou servicos apenas quando houver repeticao real
- consolidar contratos compartilhados no `SharedKernel` somente quando fizerem sentido para mais de um modulo
- manter configuracoes comuns na camada `Infrastructure`
- criar extensoes de registro e mapeamento para reduzir repeticao no `Program.cs`

## Uso de abstracoes

Abstracoes sao bem-vindas quando reduzem duplicacao, acoplamento e custo de manutencao.

Abstracoes nao devem ser introduzidas apenas para parecer arquiteturalmente sofisticado.

### Quando criar uma abstracao

Criar uma abstracao quando pelo menos uma destas condicoes for verdadeira:

1. Existe comportamento compartilhado entre modulos ou fluxos.
2. Existe colaboracao externa que precisa ser isolada.
3. Existe regra importante que merece um contrato explicito.
4. Existe repeticao concreta em mais de um ponto relevante do sistema.

### Quando nao criar uma abstracao

Nao criar abstracao quando:

1. Existe apenas um uso real e simples.
2. A generalizacao ainda e hipotetica.
3. O resultado seria uma interface vaga demais.
4. A abstracao esconderia a intencao do codigo.

### Regra pratica

Antes de criar uma abstracao, responder:

- isso remove complexidade real?
- isso reduz repeticao real?
- isso melhora testabilidade?
- isso deixa a intencao do codigo mais clara?

Se a resposta for majoritariamente nao, manter a implementacao simples.

## Diretrizes para o DevBoard

### 1. Endpoints finos

Endpoints Minimal API devem:

- receber request
- delegar para command/query/handler
- retornar resposta HTTP apropriada

Endpoints nao devem concentrar regra de negocio.

### 2. Handlers como unidade principal de caso de uso

Cada caso de uso relevante deve viver em um handler pequeno e claro.

Exemplos:

- `CreateProject`
- `UpdateProject`
- `ArchiveProject`
- `CreateTask`
- `UpdateTask`
- `CompleteTask`
- `MarkNotificationAsRead`

### 3. Modulos com boundaries claros

Cada modulo deve evoluir dentro do proprio espaco:

```text
Commands/
Queries/
Events/
Handlers/
Endpoints/
Data/
Models/
```

Evitar acesso cruzado desnecessario entre modulos.

### 4. SharedKernel minimo

O `SharedKernel` deve conter apenas elementos realmente compartilhados.

Exemplos aceitaveis:

- resultados
- erros
- clock
- contratos pequenos de evento

Nao transformar o `SharedKernel` em deposito de utilitarios genericos.

### 5. Infrastructure isolada

`Infrastructure` deve concentrar detalhes tecnicos compartilhados:

- banco de dados
- configuracao
- autenticacao
- observabilidade
- adaptadores externos

Nao mover regra de negocio para `Infrastructure`.

### 6. Configuracoes via extensions

Configuracoes do host devem ser extraidas para extensoes dedicadas e o `Program.cs` deve atuar apenas como ponto de composicao.

Diretrizes:

- `Program.cs` deve permanecer pequeno e legivel
- registros de servicos devem ficar em extensoes de `IServiceCollection`
- configuracao do pipeline deve ficar em extensoes de `WebApplication`
- mapeamento de endpoints deve ser extraido do `Program.cs`
- cada extensao deve agrupar configuracoes por responsabilidade

Exemplo de organizacao:

```text
DevBoard.Api/
  Extensions/
    ServiceCollectionExtensions.cs
    WebApplicationExtensions.cs
```

Objetivos:

- facilitar manutencao
- reduzir crescimento desordenado do bootstrap
- melhorar legibilidade
- simplificar evolucoes futuras

### 7. Nomes explicitos

Classes e arquivos devem expressar claramente a intencao.

Preferir:

- `CreateProjectHandler`
- `ProjectsEndpoints`
- `TaskCompletedHandler`

Evitar:

- `Helper`
- `Manager`
- `Util`
- `BaseService`

### 8. Composicao antes de heranca

Preferir composicao e extensoes a hierarquias profundas.

### 9. Abstracao depois de repeticao real

No MVP, a prioridade e clareza. Generalizacoes prematuras devem ser evitadas.

Se dois ou tres pontos do sistema repetirem a mesma ideia de forma consistente, ai sim vale extrair uma abstracao.

## Sinais de alerta

Durante implementacao e revisao, tratar como alerta:

- classes grandes demais
- handlers com multiplas responsabilidades
- interfaces vagas
- metodos utilitarios genericos demais
- duplicacao de validacao ou mapeamento
- `Program.cs` crescendo sem extracao de extensoes
- modulos acessando detalhes internos uns dos outros

## Regra de decisao

Em caso de duvida:

1. escolher a solucao mais simples que preserve os boundaries
2. evitar abstracao hipotetica
3. extrair apenas o que reduz complexidade real
4. manter o codigo facil de seguir por outro desenvolvedor

## Aplicacao no MVP

Durante o MVP, estas diretrizes significam:

- uma arquitetura modular clara
- endpoints pequenos
- handlers focados
- pouca magia
- pouca heranca
- abstracoes pequenas e justificadas
- codigo preparado para evoluir, sem antecipar microservicos cedo demais
- `Program.cs` pequeno e orientado a composicao
