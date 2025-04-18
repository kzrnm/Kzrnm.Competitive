#/bin/bash

id=$(gh run list -R kzrnm/Kzrnm.Competitive -L 1 -w verify --json databaseId --jq '.[].databaseId')

echo "Wokflow ID=$id"
dst="$(dirname $0)/tmp"
rm -r "$dst"
gh run download "$id" -R kzrnm/Kzrnm.Competitive -D "$dst"
