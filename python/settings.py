from dataclasses import dataclass, field
import sys


@dataclass
class Settings:
    TenantId: str = field(default="4203b7a0-7773-4de5-b830-8b263a20426e", init=False)
    ClientId: str
    ClientSecret: str
    Scope: str
    SubmitApi: str
    InsightsUrl: str

    def validate(self) -> None:
        if not self.ClientId:
            sys.exit('ClientId is required in the settings file.')
        if not self.ClientSecret:
            sys.exit('ClientSecret is required in the settings file.')
        if not self.Scope:
            sys.exit('Scope is required in the settings file.')
        if not self.SubmitApi:
            sys.exit('SubmitApi is required in the settings file.')
