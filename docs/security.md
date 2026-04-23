# DevBoard Security Notes

## Goal

Keep the repository safe to publish on GitHub without exposing credentials, tokens, certificates, or local sensitive configuration.

## Never commit

- real passwords
- real connection strings with production or personal credentials
- API keys
- GitHub tokens
- cloud secrets
- private certificates
- `.env` files with real values
- local secret appsettings files
- publish profiles containing credentials

## Safe patterns

Use these patterns instead:

- `.env.example` with placeholder values
- `appsettings.json` with non-sensitive defaults only
- environment variables for real secrets
- GitHub Actions Secrets for CI/CD configuration
- local secret files excluded by `.gitignore`

## Docker guidance

For local Docker startup:

- keep development defaults non-sensitive
- override values locally with `.env`
- never commit a real `.env`

## GitHub settings to enable

In the GitHub repository UI, enable:

- Secret scanning
- Push protection for secrets
- Dependabot alerts
- Dependabot security updates

## Incident response

If a secret is committed accidentally:

1. rotate the secret immediately
2. revoke the compromised credential
3. remove the secret from repository history
4. verify no active workflow or deployment still uses the old value
5. document what was exposed and what was rotated
