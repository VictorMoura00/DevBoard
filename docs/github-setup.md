# GitHub Repository Setup

This document captures the repository rules that should be enabled on GitHub for DevBoard.

Repository owner:

- [VictorMoura00](https://github.com/VictorMoura00)

## Goals

- keep the repository professional
- reduce the chance of broken mainline code
- preserve clear authorship
- keep Docker startup easy and verified

## Files already configured in the repository

The repository includes:

- `CODEOWNERS`
- pull request template
- issue templates
- GitHub Actions CI workflow
- `CONTRIBUTING.md`
- `README.md`
- `docker-compose.yml`
- `global.json`
- `.editorconfig`

## Manual GitHub settings to enable

These settings must be configured in the GitHub repository UI.

## Security settings

Enable at least:

- Secret scanning
- Push protection for secrets
- Dependabot alerts
- Dependabot security updates

Repository guidance is documented in:

- `docs/security.md`

## Default branch

Use:

- `main`

## Branch protection for `main`

Enable at least:

- require a pull request before merging
- require status checks to pass before merging
- require branches to be up to date before merging
- require conversation resolution before merging
- block force pushes
- block branch deletion

Recommended required check:

- `CI / build-test-and-docker`

## Merge strategy

Recommended:

- allow squash merge
- disable merge commits if you want a cleaner history
- keep linear history enabled when possible

## CODEOWNERS

Repository ownership is set to:

- `@VictorMoura00`

## Authorship policy

This repository should not include AI assistant or tool co-author trailers in commits.

Examples that should not appear in commits:

- `Co-authored-by: ...`
- assistant bot authorship
- tool-generated co-author metadata

## Commit message standard

Use Conventional Commits in Portuguese.

Format:

```text
tipo(escopo): descricao curta
```

Examples:

```text
feat(projects): adiciona estrutura inicial do modulo de projetos
fix(api): corrige rota de documentacao
docs(standards): define padrao de abstractions e extensions
```

Recommended enforcement:

- document the standard in `CONTRIBUTING.md`
- require pull requests for changes to `main`
- reject vague commit history during review

## CI expectations

The CI workflow validates:

- restore
- build
- test
- Docker Compose configuration
- Docker image build

This helps guarantee that the project remains easy to start with Docker.
