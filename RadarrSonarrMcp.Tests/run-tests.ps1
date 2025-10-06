# Run tests with cleaner output (filters out FluentAssertions warning)
# Usage: .\run-tests.ps1

Write-Host "Running tests..." -ForegroundColor Cyan
Write-Host ""

$output = dotnet test 2>&1 | Out-String

# Filter out the FluentAssertions warning lines
$filteredOutput = $output -split "`n" | Where-Object {
    $_ -notmatch "Fluent Assertions" -and
    $_ -notmatch "Xceed" -and 
    $_ -notmatch "non-commercial use" -and
    $_ -notmatch "commercial use" -and
    $_ -notmatch "mailto:sales@xceed.com" -and
    $_ -notmatch "keep Fluent Assertions" -and
    $_ -notmatch "https://xceed.com"
} | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne "" }

$filteredOutput | ForEach-Object { Write-Host $_ }

Write-Host ""
