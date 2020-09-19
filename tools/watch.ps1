$exePath = "$PSScriptRoot\SourceCodeWatcher\bin\Release\netcoreapp3.1\SourceCodeWatcher.exe"
if (-not (Test-Path $exePath)) {
    dotnet build -c Release "$PSScriptRoot\SourceCodeWatcher"
}

Start-Process $exePath "$PSScriptRoot\..\AtCoderProject\Program.cs"