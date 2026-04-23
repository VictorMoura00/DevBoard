# Contributing to DevBoard

## Principles

All contributions must preserve:

- modular monolith boundaries
- SOLID and DRY in a pragmatic way
- small and readable `Program.cs`
- Docker-first local startup
- professional repository hygiene

## Pull request expectations

Every pull request should:

1. keep the project buildable
2. keep Docker startup working
3. include focused changes only
4. follow existing naming and module boundaries
5. avoid speculative abstractions
6. never expose secrets, tokens, passwords, certificates, or local environment files

## Before opening a pull request

Run:

```bash
dotnet build DevBoard.slnx
dotnet test DevBoard.slnx
docker compose config
docker compose build api
```

## Commit and authorship policy

- Do not add AI tools, bots, or assistants as co-authors.
- Do not add generated co-author trailers to commits.
- Human authorship in this repository must remain explicit and clean.

## Commit convention

This repository uses Conventional Commits written in Portuguese.

Format:

```text
tipo(escopo): descricao curta
```

Examples:

```text
feat(api): adiciona endpoint de health check
fix(docker): corrige variavel de ambiente da conexao com postgres
docs(readme): atualiza instrucoes para subir com docker compose
refactor(projects): extrai configuracao para extension methods
test(tasks): adiciona cobertura para conclusao de tarefa
chore(ci): ajusta pipeline de build no github actions
```

Recommended types:

- `feat`
- `fix`
- `docs`
- `refactor`
- `test`
- `chore`
- `build`
- `ci`

Guidelines:

- write the message in Portuguese
- keep the description objective
- use a meaningful scope whenever possible
- avoid vague messages like `ajustes` or `correcao`
- do not include AI or tool co-author metadata

## Branching

Recommended branch naming:

```text
feature/<short-description>
fix/<short-description>
chore/<short-description>
docs/<short-description>
```

## Review guidance

Changes should be easy to review:

- one concern per pull request when possible
- clear summary
- clear validation steps
- no unrelated refactors mixed into feature work

## Secrets policy

Never commit:

- real API keys
- passwords
- access tokens
- connection strings with real credentials
- `.env` files with real values
- certificates or signing keys
- local-only appsettings files with secrets

Use:

- `.env.example` for examples
- environment variables for real secrets
- GitHub Actions Secrets for CI/CD secrets

If a secret is exposed by mistake:

1. rotate it immediately
2. remove it from the repository history
3. document the cleanup in the pull request or incident note
