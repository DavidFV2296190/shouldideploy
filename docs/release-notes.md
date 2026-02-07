# Release Notes for Should I Deploy Today?

**Version:** v1.0.0 (replace with the tag you publish)
**Release date:** 2026-02-06

## Highlights
- The deployment decision engine keeps evaluating weekends, holidays, and special-data heuristics so every team remains confident before hitting "merge".
- Smart toast notifications keep your workflow in sync—active windows get reminders every 45 minutes while off-hours checks stay gentle and unobtrusive.
- Full-screen/always-on support plus timezone selection ensures the status board can live on a dedicated monitor anywhere in the world.
- Standalone packaging works offline: single self-contained binaries for each runtime share the same deployment safety logic you already trust.

## Testing (never tested LOL because I don't really care for now... this is vibe code yo!)
- `dotnet test ShouldIDeployApp/ShouldIDeployApp.sln --no-restore --no-build`

## Published artifacts
| Runtime | File |
| --- | --- |
| Windows x64 | ShouldIDeployApp-win-x64.zip |
| macOS x64 | ShouldIDeployApp-osx-x64.zip |
| macOS Arm64 | ShouldIDeployApp-osx-arm64.zip |
| Linux x64 | ShouldIDeployApp-linux-x64.zip |

Each ZIP contains the self-contained single-file executable produced by `dotnet publish -c Release -r <runtime> --self-contained -p:PublishSingleFile=true`.
