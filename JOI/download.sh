URLS="
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-banner.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-dragon.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-joitter-in-1-10.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-joitter-in-11-17.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-joitter-in-18-20.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day1-joitter-out.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day2-guess.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day2-keycards.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day2-shiritori-in-1-18.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day2-shiritori-in-19-20.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day2-shiritori-out.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day3-deciphering.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day3-report.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day3-ufo-in.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day3-ufo-out.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day4-apples.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day4-bookshelf.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day4-ioi.zip
https://www.ioi-jp.org/camp/2011/2011-sp-tasks/Day4-orienteering.zip
"

# echo $URLS | xargs -n 1 -P 4 wget

find $(dirname $0) -name "*.zip" | xargs -n 1 -P 4 unzip