URLS="
https://www.ioi-jp.org/camp/2014/2014-sp-tasks/2014-sp-d1-data.zip
https://www.ioi-jp.org/camp/2014/2014-sp-tasks/2014-sp-d4-data.zip
"

echo $URLS | xargs -n 1 -P 4 wget

find $(dirname $0) -name "*.zip" | xargs -n 1 -P 4 unzip