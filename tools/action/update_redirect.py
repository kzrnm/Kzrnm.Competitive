import sys
import os
import pathlib
import yaml
from itertools import chain
from competitive_verifier.documents.front_matter import (
    merge_front_matter,
    split_front_matter,
)
import competitive_verifier.log as log

from logging import getLogger, INFO

log.configure_logging(default_level=INFO)

logger = getLogger("redirect")

REDIRECT_FROM = "redirect_from:\n"


def main():
    jekyll_dir = pathlib.Path(sys.argv[1]).resolve(strict=True)

    os.chdir(pathlib.Path(__file__).resolve(strict=True).parent / "../../Library")

    for p in chain(
        jekyll_dir.glob("**/*.GenericMath.cs.md"),
        jekyll_dir.glob("**/*.GenericMath/**/*.cs.md"),
    ):
        with log.group(p.as_posix()):
            with p.open("rb") as fp:
                front_matter, _ = split_front_matter(fp.read())

            logger.info("documentation_of: %s", front_matter.documentation_of)

            # redirect
            new_redirect_from = front_matter.documentation_of.replace(
                ".GenericMath", ""
            )
            redirect_from = jekyll_dir / f"{new_redirect_from}.md"
            if pathlib.Path(new_redirect_from).exists() or redirect_from.exists():
                logger.info("%s already exists.", new_redirect_from)
            else:
                logger.info("new_redirect_from: %s", new_redirect_from)

                redirect_from.parent.mkdir(parents=True, exist_ok=True)

                with redirect_from.open("w", encoding="utf-8") as fp:
                    fp.write("---\n")
                    yaml.safe_dump({"redirect_to": front_matter.documentation_of}, fp)
                    fp.write("---\n")

            # Not Generic Math
            not_generic_math_doc = jekyll_dir.joinpath(
                f"{front_matter.documentation_of.replace('.GenericMath', '.NotGenericMath')}.md"
            )
            if not_generic_math_doc.exists():
                logger.info("not_generic_math_doc: %s", not_generic_math_doc)

                with not_generic_math_doc.open("r+", encoding="utf-8") as fp:
                    l = fp.read().split("---\n")[-1]
                    if l.strip() == "":
                        fp.write(
                            f"[Generic Math 版](/{front_matter.documentation_of})と同様"
                        )


if __name__ == "__main__":
    main()
