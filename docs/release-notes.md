# Release Notes for Should I Deploy Today?

**Version:** vX.Y.Z (replace with the tag you publish)
**Release date:** 2026-02-06 (update as needed when the release goes out)

## Highlights
- The deployment decision engine keeps evaluating weekends, holidays, and special-data heuristics so every team remains confident before hitting "merge".
- Smart toast notifications keep your workflow in sync—active windows get reminders every 45 minutes while off-hours checks stay gentle and unobtrusive.
- Full-screen/always-on support plus timezone selection ensures the status board can live on a dedicated monitor anywhere in the world.
- Standalone packaging works offline: single self-contained binaries for each runtime share the same deployment safety logic you already trust.

## Testing
- `dotnet test ShouldIDeployApp/ShouldIDeployApp.sln --no-restore --no-build`

## Published artifacts
| Runtime | File |
| --- | --- |
| Windows x64 | ShouldIDeployApp-win-x64.zip |
| macOS x64 | ShouldIDeployApp-osx-x64.zip |
| macOS Arm64 | ShouldIDeployApp-osx-arm64.zip |
| Linux x64 | ShouldIDeployApp-linux-x64.zip |

Each ZIP contains the self-contained single-file executable produced by `dotnet publish -c Release -r <runtime> --self-contained -p:PublishSingleFile=true`.

## Before pushing the release (manual steps)
1. Confirm the version/tag you want to publish and update the **Version** line above plus the release date.
2. Create the corresponding Git tag (e.g., `git tag v2.1.0`) but wait for my go-ahead before pushing it.
3. Double-check the release title/body and drop any additional notes you want to highlight before the workflow runs.
