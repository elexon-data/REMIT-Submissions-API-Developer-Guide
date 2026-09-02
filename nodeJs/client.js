import { readFile } from "node:fs/promises";
import { ClientSecretCredential } from "@azure/identity";
import config from "./config.js";

const scope = "https://data.dev.elexon.co.uk/account-api-v2/.default";
const submitApi = "https://data.dev.elexon.co.uk/account/v2/remit/submit-api";

const getToken = async () => {
    const credential = new ClientSecretCredential(
        config.tenantId,
        config.clientId,
        config.clientSecret
    );

    const token = await credential.getToken(scope);
    return token.token;
};

const submit = async (token, xml) => {
    return fetch(submitApi, {
        method: "POST",
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/xml",
        },
        body: xml,
    });
};

const prettifyJson = (text) => {
    try {
        return JSON.stringify(JSON.parse(text), null, 2);
    } catch {
        return text;
    }
};

const main = async () => {
    const xml = await readFile(config.xmlFilePath, "utf8");

    const token = await getToken();
    const response = await submit(token, xml);

    console.log(`Status: ${response.status} ${response.statusText}`);
    const responseBody = await response.text();
    console.log("Response:");
    console.log(prettifyJson(responseBody));

    if (!response.ok) {
        console.error(`Submission failed with status ${response.status} ${response.statusText}.`);
        process.exit(1);
    }
};

main().catch((err) => {
    console.error("Error occurred:", err);
    process.exit(1);
});
