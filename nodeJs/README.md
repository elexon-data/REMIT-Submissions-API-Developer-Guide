# Node.js REMIT Submit Client

A minimal Node.js client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

This API is available in two environments, `Test` and `Prod`, each with its own credentials. **Start with `Test`** (the default), and once you've confirmed a submission works there, you can move on to [Using the Prod environment](#using-the-prod-environment).

> Please note that this functionality is currently under development. The production endpoint is not yet live.


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
   INSIGHTS_URL=https://bmrs.test.elexon.co.uk/remit
   ```

   - `CLIENT_ID` / `CLIENT_SECRET`: your Test app registration's credentials
   - `SCOPE` / `SUBMIT_API` / `INSIGHTS_URL`: the Test API's URLs. The template is already pre-filled with the correct values. You shouldn't need to change these

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

1. Copy `.env.Prod.template` to `.env.Prod` (instead of `.env.Test.template` to `.env.Test`).
2. Fill in your Prod app registration's `CLIENT_ID` and `CLIENT_SECRET` (see [Retrieving Credentials](../README.md#before-you-begin-generate-your-credentials) in the main guide if you don't have these yet). The `SCOPE`, `SUBMIT_API` and `INSIGHTS_URL` values are already pre-filled for Prod.
3. Run the same command as before, adding `--env=Prod`.

A successful submission will appear at [bmrs.elexon.co.uk/remit](https://bmrs.elexon.co.uk/remit) instead.
