# Node.js REMIT Submit Client

A minimal Node.js client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

This API is available in two environments, `Test` and `Prod`, each with its own credentials. **Start with `Test`** (the default), and once you've confirmed a submission works there, you can move on to [Using the Prod environment](#using-the-prod-environment).

## Prerequisites

- [Node.js](https://nodejs.org/en/) (version 20 or above)

## Configure

1. Run `npm install` to install the dependencies.

2. Copy the settings template:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item .env.Test.template .env.Test
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp .env.Test.template .env.Test
   ```

3. Edit `.env.Test` and fill in your Test app registration's credentials (see [Retrieving Credentials](../README.md#before-you-begin-generate-your-credentials) in the main guide if you don't have these yet):

   ```
   CLIENT_ID=your-test-client-id
   CLIENT_SECRET=your-test-client-secret
   SCOPE=https://data.test.elexon.co.uk/account-api-v2/.default
   SUBMIT_API=https://data.test.elexon.co.uk/account/v2/remit/submit-api
   ```

   - `CLIENT_ID` / `CLIENT_SECRET`: your Test app registration's credentials
   - `SCOPE` / `SUBMIT_API`: the Test API's URLs. The template is already pre-filled with the correct values. You shouldn't need to change these

   `.env.Test` is ignored by Git so your credentials will not be committed.

## Run

From the `nodeJs` directory:

```bash
npm run client -- --xml=path/to/your/remit-notification.xml
```

This defaults to the `Test` environment and acquires an access token, reads the given XML file, submits it, and prints the HTTP status code and response body. A successful submission returns a `2xx` status.

## Verify your submission

Once you get a successful response, you can see the submission appear on Insights at [bmrs.test.elexon.co.uk/remit](https://bmrs.test.elexon.co.uk/remit).

Make sure your submission is correct before moving on to Prod.

## Using the Prod environment

Once everything works end-to-end in `Test`, switch to `Prod` the same way, using your Prod app registration's credentials:

1. Copy the Prod template:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item .env.Prod.template .env.Prod
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp .env.Prod.template .env.Prod
   ```

2. Edit `.env.Prod` and fill in your Prod app registration's credentials (see [Retrieving Credentials](../README.md#before-you-begin-generate-your-credentials) in the main guide if you don't have these yet):

   ```
   CLIENT_ID=your-prod-client-id
   CLIENT_SECRET=your-prod-client-secret
   SCOPE=https://data.elexon.co.uk/account-api-v2/.default
   SUBMIT_API=https://data.elexon.co.uk/account/v2/remit/submit-api
   ```

3. Run with `--env=Prod`:

   ```bash
   npm run client -- --env=Prod --xml=path/to/your/remit-notification.xml
   ```

   A successful submission will appear at [bmrs.elexon.co.uk/remit](https://bmrs.elexon.co.uk/remit) instead.
