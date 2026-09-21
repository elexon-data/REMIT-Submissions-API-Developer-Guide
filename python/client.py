import json
import sys

import requests
from azure.identity import ClientSecretCredential

from settings import Settings

# Elexon Products and Services tenant ID. Fixed for all users, so it's a
# constant here rather than something read from settings.json.
TENANT_ID = "4203b7a0-7773-4de5-b830-8b263a20426e"
VALID_ENVIRONMENTS = ("Test", "Prod")


def parse_args(args: list[str]) -> tuple[str, str | None, list[str]]:
    env_prefix = '--env='
    xml_prefix = '--xml='
    environment_name = 'Test'
    xml_path = None
    unrecognized_args = []

    for arg in args:
        if arg.lower().startswith(env_prefix):
            environment_name = arg[len(env_prefix):]
        elif arg.lower().startswith(xml_prefix):
            xml_path = arg[len(xml_prefix):]
        else:
            unrecognized_args.append(arg)

    return environment_name, xml_path, unrecognized_args


def read_settings(environment_name: str) -> Settings:
    with open(f'settings.{environment_name}.json') as config_file:
        return Settings(**json.load(config_file))


def get_access_token(settings: Settings) -> str:
    credential = ClientSecretCredential(TENANT_ID, settings.ClientId, settings.ClientSecret)
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
    environment_name, xml_path, unrecognized_args = parse_args(sys.argv[1:])

    if xml_path is None or unrecognized_args:
        print(
            'Usage: python client.py [--env=Test|Prod] --xml=<path-to-remit-notification.xml>',
            file=sys.stderr,
        )
        sys.exit(1)

    if environment_name.lower() not in (env.lower() for env in VALID_ENVIRONMENTS):
        print(
            f"--env must be one of: {', '.join(VALID_ENVIRONMENTS)}. Got '{environment_name}'.",
            file=sys.stderr,
        )
        sys.exit(1)

    settings = read_settings(environment_name)
    settings.validate(environment_name)

    with open(xml_path) as xml_file:
        xml = xml_file.read()

    token = get_access_token(settings)
    response = submit(token, xml, settings.SubmitApi)

    print(f'Status: {response.status_code} {response.reason}')
    print(prettify_json(response.text))

    if not response.ok:
        print(f'Submission failed with status {response.status_code} {response.reason}.', file=sys.stderr)
        sys.exit(1)


if __name__ == '__main__':
    run()
