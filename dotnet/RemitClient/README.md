# .NET REMIT Submit Client

A minimal .NET console client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Configure

1. Copy `appsettings.template.json` to `appsettings.json`:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item appsettings.template.json appsettings.json
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp appsettings.template.json appsettings.json
   ```

2. Edit `appsettings.json` and fill in your values:

   ```json
   {
     "ClientId": "your-client-id",
     "ClientSecret": "your-client-secret",
     "XmlFilePath": "path/to/your/remit-notification.xml"
   }
   ```

   `appsettings.json` is ignored by Git so your credentials will not be committed.

   You can also set these values via environment variables (`ClientId`, `ClientSecret`, `XmlFilePath`).

## Run

From the `dotnet/RemitClient` directory:

```bash
dotnet run
```

## What it does

1. Loads settings from `appsettings.json` (and environment variables).
2. Acquires an access token from Microsoft Entra ID using `Azure.Identity.ClientSecretCredential`.
3. Reads the XML file specified by `XmlFilePath`.
4. `POST`s the XML to `https://data.dev.elexon.co.uk/account/v2/remit/submit-api` with `Authorization: Bearer <token>` and `Content-Type: application/xml`.
5. Prints the HTTP status code and response body.
