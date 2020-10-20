![Server CI](https://github.com/chrismuellner/km-log/workflows/Server%20CI/badge.svg)

# Km Log

## Setup

[Authentication with Microsoft Account](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-microsoft-accounts?view=aspnetcore-3.1)

1. Register a AAD app in the Azure Active Directory > App registrations area of the [Azure portal](https://portal.azure.com).
2. Provide a Name for the app (for example, `kmlog-<username>`).
3. In Supported account types, select `Accounts in any organizational directory (Any Azure AD directory - Multitenant) and personal Microsoft accounts (e.g. Skype, Xbox)`.
4. Leave the Redirect URI drop down set to Web and provide the following redirect URI:
    - Dev: `http://localhost/signin-microsoft`
    - Other: `https://<DOMAIN>/signin-microsoft`
6. Select Register. (the following steps are in the newly registered app)
7. Check `Application (client) ID` in `Overview`.
2. Add Client Secret in `Certificates & Secrets`.
3. Store `Client Id` and `Client Secret` in `appsettings.json` or `secrets.json` ([app secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows)) of `KmLog.Server.WebApi` in the following format:

```json
{
    "Azure": {
        "ClientId": "<CLIENT_ID>",
        "ClientSecret": "<CLIENT_SECRET>"
    }
}
```

## Development

Use the `docker-compose` in `KmLog.Server\docker-compose.database` as mssql database.

```sh
docker-compose up
```

Add emails of authorized Microsoft account(s):
- Manually insert into database
```sql
insert into [User]
  (Id, Email, Role)
values
  (newid(), 'valid@email.com', 0)
```
- Add comma-separated list via command line arguments: (`"valid@email.com,..."`)