// Elexon Products and Services tenant ID. Fixed for all users, so it's a
// constant here rather than something read from .env.
const tenantId = "4203b7a0-7773-4de5-b830-8b263a20426e";

const clientId = process.env.CLIENT_ID;
const clientSecret = process.env.CLIENT_SECRET;
const xmlFilePath = process.env.XML_FILE_PATH;

if (!clientId) {
    throw new Error("Invalid configuration value: CLIENT_ID is required");
}

if (!clientSecret) {
    throw new Error("Invalid configuration value: CLIENT_SECRET is required");
}

if (!xmlFilePath) {
    throw new Error("Invalid configuration value: XML_FILE_PATH is required");
}

export default {
    tenantId,
    clientId,
    clientSecret,
    xmlFilePath,
};
