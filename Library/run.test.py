# verification-helper: STANDALONE
from pathlib import Path
import subprocess


def main():
    subprocess.run([
        'dotnet', 'test', str(Path(__file__).resolve().parent / 'Competitive.Library.sln')],
        check=True)


if __name__ == '__main__':
    main()
