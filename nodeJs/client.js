import { readFile } from "node:fs/promises";
import { parseArgs as parseNodeArgs } from "node:util";
import { ClientSecretCredential } from "@azure/identity";
import loadSettings from "./config.js";

const VALID_ENVIRONMENTS = ["Test", "Prod"];

const parseArgs = (args) => {
    const usage = "Usage: npm run client -- [--env=Test|Prod] --xml=<path-to-remit-notification.xml>";

    let values;
    try {
        ({ values } = parseNodeArgs({
            args,
            options: {
                env: { type: "string", default: "Test" },
                xml: { type: "string" },
            },
            strict: true,
        }));
    } catch {
        return { environmentName: null, xmlPath: null, error: usage };
    }

    if (values.xml === undefined) {
        return { environmentName: null, xmlPath: null, error: usage };
    }

    if (!VALID_ENVIRONMENTS.some((env) => env.toLowerCase() === values.env.toLowerCase())) {
        const error = `--env must be one of: ${VALID_ENVIRONMENTS.join(", ")}. Got '${values.env}'.`;
        return { environmentName: null, xmlPath: null, error };
    }

    return { environmentName: values.env, xmlPath: values.xml, error: null };
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
    const { environmentName, xmlPath, error } = parseArgs(process.argv.slice(2));

    if (error) {
        console.error(error);
        process.exit(1);
    }

    const settings = loadSettings(environmentName);

    const xml = await readFile(xmlPath, "utf8");

    const token = await getToken(settings);
    const response = await submit(token, xml, settings.submitApi);

    console.log(`Status: ${response.status} ${response.statusText}`);
    const responseBody = await response.text();
    console.log(prettifyJson(responseBody));

    if (response.ok) {
        if (settings.insightsUrl) {
            console.log(`Check your submission at ${settings.insightsUrl}`);
        }
    } else {
        console.error(`Submission failed with status ${response.status} ${response.statusText}.`);
    }
};

main().catch((err) => {
    console.error("Error occurred:", err);
    process.exit(1);
});
