#!/bin/bash
set -eu

ROOT=$(dirname $(dirname $(readlink -f "$0")))
cd "$ROOT/Library/.competitive-verifier/"

RUN_ID=$(gh run list -L 1 --branch main --workflow verify --status completed --json databaseId --jq '.[].databaseId')
TMPDIR="$PWD/tmp/"
if [ -d "$TMPDIR" ]; then
   echo -n "$TMPDIR が存在します。消して続けますか？[y/n]:"
read ANS

case $ANS in
  [Yy]* )
    rm -rf "$TMPDIR"
    ;;
  * )
    exit 1
    ;;
esac
fi
gh run download --dir "$TMPDIR" $RUN_ID
cp "$TMPDIR/Linux-verify-files-json/verify_files.json" "$TMPDIR/verify_files.json"
