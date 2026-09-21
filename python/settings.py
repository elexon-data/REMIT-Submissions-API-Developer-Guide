from dataclasses import dataclass
import sys


@dataclass
class Settings:
    ClientId: str
    ClientSecret: str
    Scope: str
    SubmitApi: str

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
