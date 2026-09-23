import argparse
import json
import sys

import requests
from azure.identity import ClientSecretCredential

from settings import Settings

VALID_ENVIRONMENTS = ("Test", "Prod")


def parse_args(args: list[str]) -> argparse.Namespace:
    parser = argparse.ArgumentParser(prog='client.py')
    parser.add_argument('--env', dest='environment_name', default='Test', choices=VALID_ENVIRONMENTS)
    parser.add_argument('--xml', dest='xml_path', metavar='PATH', required=True)
    return parser.parse_args(args)


def read_settings(environment_name: str) -> Settings:
    with open(f'settings.{environment_name}.json') as config_file:
        return Settings(**json.load(config_file))


def get_access_token(settings: Settings) -> str:
    credential = ClientSecretCredential(settings.TenantId, settings.ClientId, settings.ClientSecret)
    return credential.get_token(settings.Scope).token


def submit(token: str, xml: str, submit_api: str) -> requests.Response:
    headers = {
        'Authorization': f'Bearer {token}',
        'Content-Type': 'application/xml',
    }
    return requests.post(submit_api, data=xml.encode('utf-8'), headers=headers)


def prettify_json(text: str) -> str:
    try:
        return json.dumps(json.loads(text), indent=2)
    except json.JSONDecodeError:
        return text


def run():
    parsed_args = parse_args(sys.argv[1:])

    settings = read_settings(parsed_args.environment_name)
    settings.validate()

    with open(parsed_args.xml_path) as xml_file:
        xml = xml_file.read()

    token = get_access_token(settings)
    response = submit(token, xml, settings.SubmitApi)

    print(f'Status: {response.status_code} {response.reason}')
    print(prettify_json(response.text))

    if response.ok:
        if settings.InsightsUrl:
            print(f'Check your submission at {settings.InsightsUrl}')
    else:
        print(f'Submission failed with status {response.status_code} {response.reason}.', file=sys.stderr)


if __name__ == '__main__':
    run()
