# REMIT clients

Elexon's [Insights Solution](https://bmrs.elexon.co.uk/remit) is the platform GB market participants use to publish this REMIT data, and the REMIT Submit API is how you can submit it programmatically instead of through the Elexon Portal UI.

This repo contains example clients showing how to submit a REMIT XML notification to the Elexon REMIT Submit API.

Currently we have clients written in **C#/.NET**, **Node.js**, and **Python**.

## Before you begin: generate your credentials

1. Login to the [Test Insights Solution](https://bmrs.test.elexon.co.uk/login) and go to the [Remit Submit Page](https://bmrs.test.elexon.co.uk/remit/submit).
2. If you haven't already, generate your credentials by clicking 'Request Credentials'
   ![request-credentials](images/request-credentials.png)
3. You will see a pop-up that shows your new client secret. Make sure you copy that, as it won't be shown again.
4. Note down your **Client ID** and **Client secret**. You will use them when calling the API.

## Supported languages

- [C# / .NET](dotnet/RemitClient/README.md)
- [Node.js](nodeJs/README.md)
- [Python](python/README.md)

Each client folder has its own `README.md` with setup and run instructions. All three follow the same flow:

1. Load your `ClientId`, `ClientSecret`, and the path to your REMIT XML file from a local settings file.
2. Request an access token from Microsoft Entra ID using the `client_credentials` grant.
3. `POST` the XML file's contents to the REMIT Submit API.
4. Print the response.

## Writing your REMIT XML

Validate your REMIT notification against the XSD schema before submitting it. Two schema versions are available in [`schemas/`](schemas/):

- [`remit2_0.xsd`](schemas/remit2_0.xsd) — targets `http://bmreports.com/XSD/2.0/remit.xsd`.
- [`remit2_1.xsd`](schemas/remit2_1.xsd) — targets `http://bmreports.com/XSD/2.1/remit.xsd`.

For security reasons, the REMIT Submit API also rejects notifications whose field values contain:

- **Formula injection characters** — e.g. values starting with characters that spreadsheet applications interpret as formulas.
- **Hyperlinks** — e.g. values containing links.
- **Newlines** — line breaks are blocked to guard against log injection.

Make sure your generated XML doesn't include any of the above, or the API will reject the submission.

## Response codes and messages

See [`RESPONSE_CODES.md`](RESPONSE_CODES.md) for the REMIT Submit API's success/error response shapes.

## Auth details

All clients authenticate the same way:

- **Token endpoint:** `POST https://login.microsoftonline.com/4203b7a0-7773-4de5-b830-8b263a20426e/oauth2/v2.0/token`
- **Grant type:** `client_credentials`
- **Scope:** `https://data.dev.elexon.co.uk/account-api-v2/.default`
- **Submit endpoint:** `POST https://data.dev.elexon.co.uk/account/v2/remit/submit-api`

The tenant ID above is fixed for every user, so it's hard-coded in each client rather than something you provide yourself.

## Feedback

We're continuously improving REMIT submission. Help us improve the service by sharing your feedback at insightssupport@elexon.co.uk.

We also welcome contributions and suggestions directly to this repository. Please see our [contributing guidelines](./CONTRIBUTING.md) for more information.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
