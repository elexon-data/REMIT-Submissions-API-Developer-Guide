# Python REMIT Submit Client

A minimal Python client that authenticates with Microsoft Entra ID and submits a REMIT XML notification to the Elexon REMIT Submit API.

## Prerequisites

- [Python](https://www.python.org/downloads/) (version 3.11 or above is recommended)

## Configure

1. From the `python` directory, create a virtual environment and install the dependencies:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   python -m venv .venv
   ./.venv/Scripts/activate
   pip install -r requirements.txt
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   python -m venv .venv
   source ./.venv/Scripts/activate
   pip install -r requirements.txt
   ```

2. Copy `settings.template.json` to `settings.json`:

   ```powershell
   # PowerShell (Windows/macOS/Linux)
   Copy-Item settings.template.json settings.json
   ```

   ```bash
   # bash/zsh (macOS/Linux) or Git Bash on Windows
   cp settings.template.json settings.json
   ```

3. Edit `settings.json` and fill in your values:

   ```json
   {
     "ClientId": "your-client-id",
     "ClientSecret": "your-client-secret",
     "XmlFilePath": "path/to/your/remit-notification.xml"
   }
   ```

   `settings.json` is ignored by Git so your credentials will not be committed.

## Run

From the `python` directory, with the virtual environment activated:

```bash
python client.py
```

## What it does

1. Loads settings from `settings.json`.
2. Acquires an access token from Microsoft Entra ID using `azure-identity`'s `ClientSecretCredential`.
3. Reads the XML file specified by `XmlFilePath`.
4. `POST`s the XML to `https://data.dev.elexon.co.uk/account/v2/remit/submit-api` with `Authorization: Bearer <token>` and `Content-Type: application/xml`.
5. Prints the HTTP status code and response body, and exits with a non-zero status code if the submission failed.
