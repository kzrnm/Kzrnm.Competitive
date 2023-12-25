#!/bin/bash
set -eu

ROOT=$(dirname $(dirname $(readlink -f "$0")))
competitive-verifier merge-result "$ROOT"/Library/.competitive-verifier/tmp/Result-Linux-*/result.json > "$ROOT/Library/.competitive-verifier/prev-result.json"
gzip "$ROOT/Library/.competitive-verifier/prev-result.json"
