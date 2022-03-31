# verification-helper: STANDALONE
from pathlib import Path
import subprocess


def main():
    sln_path = str(Path(__file__).resolve().parent / 'Competitive.Library.sln')
    subprocess.run(['dotnet', 'test', sln_path], check=True)


if __name__ == '__main__':
    main()
