import dotenv from "dotenv";

// Elexon Products and Services tenant ID. Fixed for all users, so it's a
// constant here rather than something read from the .env file.
const tenantId = "4203b7a0-7773-4de5-b830-8b263a20426e";

const loadSettings = (environmentName) => {
    const envFile = `.env.${environmentName}`;
    dotenv.config({ path: envFile, quiet: true });

    const clientId = process.env.CLIENT_ID;
    const clientSecret = process.env.CLIENT_SECRET;
    const scope = process.env.SCOPE;
    const submitApi = process.env.SUBMIT_API;

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

    return { tenantId, clientId, clientSecret, scope, submitApi };
};

export default loadSettings;
