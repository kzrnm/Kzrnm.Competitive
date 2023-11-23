$prog = "$PSScriptRoot/../Competitive/Program.cs"

if ([char]::IsLower((git ls-files -v $prog)[0])) {
    git update-index --no-assume-unchanged $prog
}
else {
    git update-index --assume-unchanged $prog
}