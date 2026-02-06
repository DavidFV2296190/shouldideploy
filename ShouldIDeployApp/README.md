# Should I Deploy Today? - .NET Desktop Application

A standalone cross-platform .NET desktop application that tells you whether it's safe to deploy to production. No external services required.

Built with [Avalonia UI](https://avaloniaui.net/) for cross-platform support (Windows, macOS, Linux).

## Features

- **Deployment Decision Engine** — Checks day of week, time of day, and special dates (Friday the 13th, Christmas, New Year's) to determine if it's safe to deploy
- **Toast Notifications** — Smart, non-intrusive notifications that keep developers informed without being annoying
- **Full Screen Mode** — Toggle full screen to dedicate a monitor to always showing deployment status
- **Timezone Support** — Select any timezone to check deployment safety for your team's location
- **Completely Standalone** — No internet connection or external services needed

## Notification Strategy

The notification system is designed to be helpful without being intrusive:

| Context | Notification Interval | Rationale |
|---------|----------------------|-----------|
| Work hours (9 AM–6 PM weekdays) | Every 45 minutes | Regular gentle reminders |
| Off hours / weekends | Every 2 hours | Very light touch |
| Status change (safe ↔ unsafe) | Immediate | Critical information |
| Internal status check | Every 5 minutes | Catches changes promptly |

Toast notifications auto-dismiss after 8 seconds and can be manually dismissed at any time.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Quick Start

```bash
cd ShouldIDeployApp

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run the application
dotnet run --project src/ShouldIDeployApp
```

## Running Tests

```bash
cd ShouldIDeployApp
dotnet test
```

## Publishing a Standalone Executable

Create a self-contained single-file executable that requires no .NET runtime:

```bash
# Windows
dotnet publish src/ShouldIDeployApp -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -o publish/win-x64

# macOS (Intel)
dotnet publish src/ShouldIDeployApp -c Release -r osx-x64 --self-contained -p:PublishSingleFile=true -o publish/osx-x64

# macOS (Apple Silicon)
dotnet publish src/ShouldIDeployApp -c Release -r osx-arm64 --self-contained -p:PublishSingleFile=true -o publish/osx-arm64

# Linux
dotnet publish src/ShouldIDeployApp -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true -o publish/linux-x64
```

## Project Structure

```
ShouldIDeployApp/
├── ShouldIDeployApp.sln          # Solution file
├── src/ShouldIDeployApp/         # Main application
│   ├── Models/
│   │   ├── TimeHelper.cs         # Timezone-aware date/time logic
│   │   ├── DeploymentChecker.cs  # Core deployment decision engine
│   │   └── Reasons.cs            # Witty deployment messages
│   ├── Services/
│   │   └── NotificationScheduler.cs  # Smart toast notification scheduler
│   ├── ViewModels/
│   │   └── MainWindowViewModel.cs    # MVVM view model
│   ├── Views/
│   │   ├── MainWindow.axaml         # UI layout (XAML)
│   │   └── MainWindow.axaml.cs      # Window code-behind
│   ├── App.axaml                    # Application definition
│   ├── App.axaml.cs                 # Application startup
│   └── Program.cs                   # Entry point
└── tests/ShouldIDeployApp.Tests/    # Unit tests
    ├── TimeHelperTests.cs
    ├── DeploymentCheckerTests.cs
    └── NotificationSchedulerTests.cs
```
