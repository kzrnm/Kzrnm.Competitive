# verification-helper: PROBLEM https://onlinejudge.u-aizu.ac.jp/courses/lesson/2/ITP1/1/ITP1_1_A
from pathlib import Path
import subprocess


def main():
    thisfile = Path(__file__).resolve()
    subprocess.run([
        'dotnet', 'test',
        str(thisfile.parent / 'Competitive.Library.sln')])


if __name__ == '__main__':
    main()
