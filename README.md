# SpecFix

A [CounterStrikeSharp](https://docs.cssharp.dev/) plugin for Counter-Strike 2 that fixes the **"phantom body"** problem in spectator mode.

When a live player switches to the spectators through the normal team menu, CS2 leaves their old player pawn behind as a persistent dead body. That leftover body is invisible-but-selectable, so while spectating you can get "stuck" watching an empty corpse — complete with its name and avatar in the HUD. SpecFix takes over the spectator switch cycle so the camera **never lands on that phantom body**.

## Features

- Skips the phantom body when cycling spectator targets with left/right click (`spec_next` / `spec_prev`) — no flicker, no HUD glitch, no camera lock.
- Reactive safety net for the free-roam → click-to-lock case (`spec_mode`): if the engine lands the camera on your own leftover body, it is immediately bumped to a live player, while still respecting the target you aimed at.
- **Crash-safe by design.** It never calls `Remove()`, never swaps controller handles, and never teleports the player, so rejoining a team can never crash. It only redirects the observer target, networked the correct way (marking `m_pObserverServices` dirty on `CBasePlayerPawn`).
- Admin command to toggle the fix on the fly.
- Configurable and localizable.

## How it works

The phantom is the dead `CCSPlayerPawn` that persists while its owning controller sits in spectators. SpecFix does not try to delete it (that path crashes on rejoin); instead it controls where the spectator camera is allowed to go:

| Input | Command | Behavior |
| --- | --- | --- |
| Left click | `spec_next` | Plugin picks the next **live** player itself and blocks the engine's own selection. |
| Right click | `spec_prev` | Plugin picks the previous **live** player itself and blocks the engine's own selection. |
| Space (mode) | `spec_mode` | Not blocked. One tick later, if the camera landed on your own phantom body, it is redirected to a live player. |

## Requirements

- Counter-Strike 2 dedicated server with [Metamod:Source](https://www.sourcemm.net/)
- [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp) (built against API `1.0.369`)

## Installation

1. Download the latest release, or build from source (see below).
2. Copy the `SpecFix` folder into `game/csgo/addons/counterstrikesharp/plugins/`.

   The final layout should look like:

   ```
   addons/counterstrikesharp/plugins/SpecFix/
   ├── SpecFix.dll
   └── lang/
       ├── en.json
       └── ru.json
   ```
3. Restart the server or load the plugin with `css_plugins load SpecFix`.

## Configuration

On first load the plugin generates `addons/counterstrikesharp/configs/plugins/SpecFix/SpecFix.json`:

```json
{
  "Enabled": true,
  "DebugLog": true,
  "Version": 2
}
```

| Option | Type | Default | Description |
| --- | --- | --- | --- |
| `Enabled` | bool | `true` | Master switch for the fix. |
| `DebugLog` | bool | `true` | Prints a line to the server console whenever the camera is redirected away from a phantom. |

## Commands

| Command | Permission | Description |
| --- | --- | --- |
| `css_specfix [on\|off]` | `@css/generic` | Toggles the fix. With no argument it flips the current state. |

## Localization

Language files live in `lang/`. `en.json` and `ru.json` are included; add your own by copying one of them and translating the values.

## Building from source

```bash
dotnet build -c Release
```

The build produces a ready-to-upload bundle at `bin/Release/net10.0/bundle/plugins/SpecFix/`. Copy that `SpecFix` folder straight into your server's `plugins/` directory.

## License

MIT

## Credits

- Author: **Nip0s**
- Built with [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp).
