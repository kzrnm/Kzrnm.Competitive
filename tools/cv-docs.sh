#!/bin/bash

ROOT=$(dirname $(dirname $(readlink -f "$0")))
cd "$ROOT/Library/"

competitive-verifier docs .competitive-verifier/tmp/Result-*/result.json --verify-json .competitive-verifier/tmp/verify_files.json --destination .competitive-verifier/tmp/_jekyll/
python3 "$ROOT/tools/update_redirect.py" "$ROOT/Library/.competitive-verifier/tmp/_jekyll/"