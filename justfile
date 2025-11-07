# GitHub CLI Extension Build Tasks

# Show available tasks (default)
default:
    @just --list --unsorted

# Variables
solution := "gh-hello.sln"
unit_tests := "tests/Hello.Tests/Hello.Tests.csproj"

set windows-shell := ["powershell.exe", "-NoLogo", "-Command"]

# Restore all project dependencies
restore:
    dotnet restore {{solution}}

# Build the entire solution
build: restore
    dotnet build {{solution}} --no-restore /p:TreatWarningsAsErrors=true

# Build in release mode
build-release: restore
    dotnet build {{solution}} --no-restore --configuration Release /p:TreatWarningsAsErrors=true

# Format code using dotnet format
format:
    dotnet format {{solution}}

# Verify code formatting (CI check)
format-check:
    dotnet format {{solution}} --verify-no-changes

# Run unit tests
test: build
    dotnet test {{unit_tests}} --no-build --verbosity normal

# Run unit tests with coverage
test-coverage: build
    dotnet test {{unit_tests}} --no-build --verbosity normal --logger "xunit;LogFilePath=unit-tests.xml" --collect:"XPlat Code Coverage" --results-directory ./coverage

# Build and run the extension CLI locally
run-gei *args: build
    dotnet run --project src/Hello/Hello.csproj {{args}}

# Watch and auto-rebuild on changes
watch-gei:
    dotnet watch build --project src/Hello/Hello.csproj

# Build self-contained binaries for all platforms (requires PowerShell)
publish:
    pwsh ./publish.ps1

# Build only Linux binaries 
publish-linux:
    #!/usr/bin/env pwsh
    $env:SKIP_WINDOWS = "true"
    $env:SKIP_MACOS = "true"
    ./publish.ps1

# Build only Windows binaries
publish-windows:
    #!/usr/bin/env pwsh
    $env:SKIP_LINUX = "true"
    $env:SKIP_MACOS = "true"
    ./publish.ps1

# Build only macOS binaries
publish-macos:
    #!/usr/bin/env pwsh
    $env:SKIP_WINDOWS = "true"
    $env:SKIP_LINUX = "true"
    ./publish.ps1

# Clean build artifacts
clean:
    dotnet clean {{solution}}
    rm -rf dist/
    rm -rf coverage/

# Full CI pipeline: format check, build, and test
ci: format-check build test

# Full development workflow: format, build, test
dev: format build test

# Install gh CLI extension locally (requires built binaries)
install-extensions: publish-linux
    #!/usr/bin/env bash
    set -euo pipefail
    
    # Create extension directory
    mkdir -p gh-hello
    
    # Copy binaries
    cp ./dist/linux-x64/hello-linux-amd64 ./gh-hello/gh-hello
    
    # Set execute permissions
    chmod +x ./gh-hello/gh-hello
    
    # Install extensions
    cd gh-hello && gh extension install . && cd ..
    
    echo "Extensions installed successfully!"
