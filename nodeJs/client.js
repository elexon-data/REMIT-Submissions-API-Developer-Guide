import { readFile } from "node:fs/promises";
import { ClientSecretCredential } from "@azure/identity";
import loadSettings from "./config.js";

const VALID_ENVIRONMENTS = ["Test", "Prod"];

const parseArgs = (args) => {
    const envPrefix = "--env=";
    const xmlPrefix = "--xml=";
    let environmentName = "Test";
    let xmlPath = null;
    const unrecognizedArgs = [];

    for (const arg of args) {
        if (arg.toLowerCase().startsWith(envPrefix)) {
            environmentName = arg.slice(envPrefix.length);
        } else if (arg.toLowerCase().startsWith(xmlPrefix)) {
            xmlPath = arg.slice(xmlPrefix.length);
        } else {
            unrecognizedArgs.push(arg);
        }
    }

    return { environmentName, xmlPath, unrecognizedArgs };
};

const getToken = async (settings) => {
    const credential = new ClientSecretCredential(
        settings.tenantId,
        settings.clientId,
        settings.clientSecret
    );

    const token = await credential.getToken(settings.scope);
    return token.token;
};

const submit = async (token, xml, submitApi) => {
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
    const { environmentName, xmlPath, unrecognizedArgs } = parseArgs(process.argv.slice(2));

    if (xmlPath === null || unrecognizedArgs.length > 0) {
        console.error("Usage: node client.js [--env=Test|Prod] --xml=<path-to-remit-notification.xml>");
        process.exit(1);
    }

    if (!VALID_ENVIRONMENTS.some((env) => env.toLowerCase() === environmentName.toLowerCase())) {
        console.error(`--env must be one of: ${VALID_ENVIRONMENTS.join(", ")}. Got '${environmentName}'.`);
        process.exit(1);
    }

    const settings = loadSettings(environmentName);

    const xml = await readFile(xmlPath, "utf8");

    const token = await getToken(settings);
    const response = await submit(token, xml, settings.submitApi);

    console.log(`Status: ${response.status} ${response.statusText}`);
    const responseBody = await response.text();
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
