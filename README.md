# DisplaySwitch

Small console utility to toggle a secondary monitor from left to right (and back) relative to the primary monitor on Windows. It uses Win32 display APIs (`EnumDisplayDevices`, `EnumDisplaySettings`, `ChangeDisplaySettingsEx`) to read and move the secondary’s position.

## Build and run

```bash
dotnet build -c Release
dotnet run --project DisplaySwitch/DisplaySwitch.csproj
```

Release binary: `DisplaySwitch/bin/Release/net8.0/DisplaySwitch.exe`

## Behavior

- Detects primary and first secondary display.
- If the secondary is to the left of the primary (X < primary X), it moves it to the right edge of the primary.
- Otherwise, it moves it to the left edge of the primary.
- Handy for people who frequently connect/disconnect a mobile secondary display and want to flip its position quickly.

## Keyboard shortcut to launch the EXE

1) Build in Release: `dotnet build -c Release`.
2) In Explorer, go to `DisplaySwitch/bin/Release/net8.0/`.
3) Right-click `DisplaySwitch.exe` → `Create shortcut`.
4) Right-click the shortcut → `Properties` → `Shortcut` tab.
5) In “Shortcut key”, press your desired combo (e.g., Ctrl+Alt+D). Click OK.
6) Optional: set “Run” to “Minimized” to keep the console out of the way.
7) Move the shortcut to Desktop/Start menu if you prefer—the hotkey will still work.

## Notes

- Windows only (uses `user32.dll`).
- Changes your display layout; use at your own risk.
- Build artifacts (`bin/`, `obj/`) are ignored via `.gitignore`.
