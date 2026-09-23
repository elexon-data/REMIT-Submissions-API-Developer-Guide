from dataclasses import dataclass
import sys

TENANT_ID = "4203b7a0-7773-4de5-b830-8b263a20426e"


@dataclass
class Settings:
    ClientId: str
    ClientSecret: str
    Scope: str
    SubmitApi: str
    InsightsUrl: str

    def validate(self, environment: str) -> None:
        env_file = f'settings.{environment}.json'
        if not self.ClientId:
            sys.exit(f'ClientId is required in {env_file}.')
        if not self.ClientSecret:
            sys.exit(f'ClientSecret is required in {env_file}.')
        if not self.Scope:
            sys.exit(f'Scope is required in {env_file}.')
        if not self.SubmitApi:
            sys.exit(f'SubmitApi is required in {env_file}.')
        if not self.InsightsUrl:
            sys.exit(f'InsightsUrl is required in {env_file}.')
