import json
import sys

import requests
from azure.identity import ClientSecretCredential

from settings import Settings

# Elexon Products and Services tenant ID. Fixed for all users, so it's a
# constant here rather than something read from settings.json.
TENANT_ID = "4203b7a0-7773-4de5-b830-8b263a20426e"
SCOPE = "https://data.dev.elexon.co.uk/account-api-v2/.default"
SUBMIT_API = "https://data.dev.elexon.co.uk/account/v2/remit/submit-api"


def read_settings() -> Settings:
    with open('settings.json') as config_file:
        return Settings(**json.load(config_file))


def get_access_token(settings: Settings) -> str:
    credential = ClientSecretCredential(TENANT_ID, settings.ClientId, settings.ClientSecret)
    return credential.get_token(SCOPE).token


def submit(token: str, xml: str) -> requests.Response:
    headers = {
        'Authorization': f'Bearer {token}',
        'Content-Type': 'application/xml',
    }
    return requests.post(SUBMIT_API, data=xml.encode('utf-8'), headers=headers)


def prettify_json(text: str) -> str:
    try:
        return json.dumps(json.loads(text), indent=2)
    except json.JSONDecodeError:
        return text


def run():
    settings = read_settings()

    with open(settings.XmlFilePath) as xml_file:
        xml = xml_file.read()

    token = get_access_token(settings)
    response = submit(token, xml)

    print(f'Status: {response.status_code} {response.reason}')
    print('Response:')
    print(prettify_json(response.text))

    if not response.ok:
        sys.exit(f'Submission failed with status {response.status_code} {response.reason}.')


if __name__ == '__main__':
    run()
