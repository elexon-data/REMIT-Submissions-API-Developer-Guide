# Node.js REMIT Submit Client

A minimal Node.js client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

## Prerequisites

- [Node.js](https://nodejs.org/en/) (version 20 or above)

## Configure

1. Run `npm install` to install the dependencies.

2. Copy `.env.template` to `.env`:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item .env.template .env
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp .env.template .env
   ```

3. Edit `.env` and fill in your values:

   ```
   CLIENT_ID=your-client-id
   CLIENT_SECRET=your-client-secret
   XML_FILE_PATH=path/to/your/remit-notification.xml
   ```

   `.env` is ignored by Git so your credentials will not be committed.

## Run

From the `nodeJs` directory:

```bash
npm run client
```

## What it does

1. Loads settings from `.env`.
2. Acquires an access token from Microsoft Entra ID using `@azure/identity`'s `ClientSecretCredential`.
3. Reads the XML file specified by `XML_FILE_PATH`.
4. `POST`s the XML to `https://data.dev.elexon.co.uk/account/v2/remit/submit-api` with `Authorization: Bearer <token>` and `Content-Type: application/xml`.
5. Prints the HTTP status code and response body, and exits with a non-zero status code if the submission failed.
