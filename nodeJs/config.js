import dotenv from "dotenv";

const loadSettings = (environmentName) => {
    const envFile = `.env.${environmentName}`;
    dotenv.config({ path: envFile, quiet: true });

    const tenantId = "4203b7a0-7773-4de5-b830-8b263a20426e";
    const clientId = process.env.CLIENT_ID;
    const clientSecret = process.env.CLIENT_SECRET;
    const scope = process.env.SCOPE;
    const submitApi = process.env.SUBMIT_API;
    const insightsUrl = process.env.INSIGHTS_URL;

    if (!clientId) {
        throw new Error(`CLIENT_ID is required in ${envFile}.`);
    }

    if (!clientSecret) {
        throw new Error(`CLIENT_SECRET is required in ${envFile}.`);
    }

    if (!scope) {
        throw new Error(`SCOPE is required in ${envFile}.`);
    }

    if (!submitApi) {
        throw new Error(`SUBMIT_API is required in ${envFile}.`);
    }

    return { tenantId, clientId, clientSecret, scope, submitApi, insightsUrl };
};

export default loadSettings;
