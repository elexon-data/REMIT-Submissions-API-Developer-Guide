# .NET REMIT Submit Client

A minimal .NET console client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

This API is available in two environments, `Test` and `Prod`, each with its own credentials. **Start with `Test`** (the default), and once you've confirmed a submission works there, you can move on to [Using the Prod environment](#using-the-prod-environment).

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

## Configure

1. Copy the settings template:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item appsettings.Test.template.json appsettings.Test.json
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp appsettings.Test.template.json appsettings.Test.json
   ```

2. Edit `appsettings.Test.json` and fill in your Test app registration's credentials (see [Retrieving Credentials](../../README.md#before-you-begin-generate-your-credentials) in the main guide if you don't have these yet):

   ```json
   {
     "ClientId": "your-test-client-id",
     "ClientSecret": "your-test-client-secret",
     "Scope": "https://data.test.elexon.co.uk/account-api-v2/.default",
     "SubmitApi": "https://data.test.elexon.co.uk/account/v2/remit/submit-api",
     "InsightsUrl": "https://bmrs.test.elexon.co.uk/remit"
   }
   ```

   - `ClientId` / `ClientSecret`: your Test app registration's credentials
   - `Scope` / `SubmitApi` / `InsightsUrl`: the Test API's URLs. The template is already pre-filled with the correct values. You shouldn't need to change these

## Run

From the `dotnet/RemitClient` directory:

```bash
dotnet run -- --xml=path/to/your/remit-notification.xml
```

This defaults to the `Test` environment and acquires an access token, reads the given XML file, submits it, and prints the HTTP status code and response body. A successful submission returns a `2xx` status.

## Verify your submission

Once you get a successful response, you can see the submission appear on Insights at [bmrs.test.elexon.co.uk/remit](https://bmrs.test.elexon.co.uk/remit).

Make sure your submission is correct before moving on to Prod.

## Using the Prod environment

Once everything works end-to-end in `Test`, switch to `Prod` the same way, using your Prod app registration's credentials:

1. Copy `appsettings.Prod.template.json` to `appsettings.Prod.json` (instead of `appsettings.Test.template.json` to `appsettings.Test.json`).
2. Fill in your Prod app registration's `ClientId` and `ClientSecret` (see [Retrieving Credentials](../../README.md#before-you-begin-generate-your-credentials) in the main guide if you don't have these yet). The `Scope`, `SubmitApi` and `InsightsUrl` values are already pre-filled for Prod.
3. Run the same command as before, adding `--env=Prod`.

A successful submission will appear at [bmrs.elexon.co.uk/remit](https://bmrs.elexon.co.uk/remit) instead.
