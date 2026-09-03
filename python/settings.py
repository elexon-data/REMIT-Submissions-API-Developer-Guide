from dataclasses import dataclass
import sys


@dataclass
class Settings:
    ClientId: str
    ClientSecret: str
    XmlFilePath: str

    def __post_init__(self):
        if not self.ClientId:
            sys.exit('Invalid configuration value: ClientId is required')
        if not self.ClientSecret:
            sys.exit('Invalid configuration value: ClientSecret is required')
        if not self.XmlFilePath:
            sys.exit('Invalid configuration value: XmlFilePath is required')
