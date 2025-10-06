# Test Runner Scripts

## Overview

To avoid the FluentAssertions commercial license warning that clutters test output, use the provided PowerShell scripts.

## Scripts

### Root Level: `run-tests.ps1`
Runs all tests in the solution with filtered output.

```powershell
# From project root
.\run-tests.ps1
```

### Test Project: `RadarrSonarrMcp.Tests\run-tests.ps1`
Runs tests in the test project with filtered output.

```powershell
# From test project directory
cd RadarrSonarrMcp.Tests
.\run-tests.ps1
```

## What Gets Filtered?

The scripts filter out these lines:
- ✗ "Fluent Assertions"
- ✗ "Xceed"
- ✗ "non-commercial use"
- ✗ "commercial use"
- ✗ "mailto:sales@xceed.com"
- ✗ "keep Fluent Assertions"
- ✗ "https://xceed.com"

## Output Comparison

### Before (with warning):
```
[xUnit.net 00:00:00.18]   Finished:    RadarrSonarrMcp.Tests
     Warning:
     The component "Fluent Assertions" is governed by the rules...
     [8 more lines of licensing text]
Test summary: total: 4, failed: 0, succeeded: 4, skipped: 0
```

### After (clean):
```
[xUnit.net 00:00:00.18]   Finished:    RadarrSonarrMcp.Tests
Test summary: total: 4, failed: 0, succeeded: 4, skipped: 0
✓ Tests complete!
```

## Alternative: Use Standard dotnet test

If you prefer seeing the warning or need full output:

```powershell
dotnet test
dotnet test RadarrSonarrMcp.sln
```

## CI/CD Integration

For CI/CD pipelines, you can either:

1. Use the PowerShell script (if Windows-based)
2. Use standard `dotnet test` (warning won't break builds)
3. Set environment variable (though this doesn't currently suppress the warning)

```yaml
# GitHub Actions example
- name: Run tests
  run: dotnet test RadarrSonarrMcp.sln
  # Warning will appear but won't fail the build
```
