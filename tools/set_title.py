import csv
import pathlib
import sys
from competitive_verifier.models import *


def main(input_path: str):
    with pathlib.Path(input_path).open("rb") as fp:
        ver = VerificationInput.model_validate_json(fp.read())
    for path in pathlib.Path(".").glob("**/_title.tsv"):
        with path.open(mode="r", encoding="utf-8") as fp:
            for cols in csv.reader(fp, delimiter="\t"):
                file = path.parent / cols[0]
                ver.files[file].document_attributes["TITLE"] = cols[1]

    with pathlib.Path(input_path).open("w", encoding="utf-8") as fp:
        fp.write(ver.model_dump_json(exclude_none=True))


if __name__ == "__main__":
    main(sys.argv[1])
