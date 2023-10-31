import sys
import os
import json
from urllib.request import urlopen
from typing import NamedTuple


class Version(NamedTuple):
    major: int
    minor: int
    patch: int

    @classmethod
    def parse(cls, s: str) -> "Version":
        splited = dict(enumerate(int(p.strip()) for p in s.split(".")))
        return Version(splited.get(0), splited.get(1), splited.get(2))

    def date_num(self) -> int:
        return self.major * 100 + self.minor // 100


def main(prev: Version):
    with urlopen(
        "https://api.nuget.org/v3-flatcontainer/kzrnm.competitive/index.json"
    ) as req:
        latest = Version.parse(json.load(req)["versions"][-1])
    print(f"prev={prev.date_num()}", file=sys.stderr)
    print(f"latest={latest.date_num()}", file=sys.stderr)
    print(f"is-new-month={prev.date_num()<latest.date_num()}".lower())


if __name__ == "__main__":
    main(Version.parse(sys.argv[1]))
