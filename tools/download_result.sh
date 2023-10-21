set -eu

ROOT=$(dirname $(dirname $(readlink -f "$0")))
cd "$ROOT/Library/.competitive-verifier/"

RUN_ID=$(gh run list -L 1 --branch main --workflow verify --status completed --json databaseId --jq '.[].databaseId')
gh run download --dir tmp $RUN_ID
cp tmp/Linux-verify-files-json/verify_files.json tmp/verify_files.json