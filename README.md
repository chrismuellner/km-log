![Server CI](https://github.com/chrismuellner/km-log/workflows/Server%20CI/badge.svg)

# Km Log

## Setup

[Authentication with Microsoft Account](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/standalone-with-microsoft-accounts?view=aspnetcore-3.1)

1. Register a AAD app in the Azure Active Directory > App registrations area of the [Azure portal](https://portal.azure.com)
2. Provide a Name for the app (for example, Blazor Standalone AAD Microsoft Accounts).
3. In Supported account types, select Accounts in any organizational directory.
4. Leave the Redirect URI drop down set to Web and provide the following redirect URI:
    - Dev: `http://localhost/signin-microsoft`
    - Other: `https://<DOMAIN>/signin-microsoft`
5. Clear the Permissions > Grant admin consent to openid and offline_access permissions check box.
6. Select Register.

The next steps are in the registered app.

1. Check `Application (client) ID` in `Overview`.
2. Add Client Secret in `Certificates & Secrets`
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

`docker-compose` for mssql database in `docker-compose.database`

```sh
docker-compose up
```
Manually add email for Microsoft account to database, or add via command line arguments (`"valid@email.com,..."`).