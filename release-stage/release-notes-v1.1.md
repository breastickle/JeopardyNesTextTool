## What's new in v1.1

### Byte-budget panel
The block tree view is now organized into **Groups**, mirroring the structure of `Config.json`. Selecting a group shows a live capacity panel:

- **Maximum Size** — bytes available in the group's `InsertRange`
- **Actual Size** — bytes the current script will encode to (recomputed live as you edit)
- **Slack** — remaining headroom, with a color-coded state indicator:
  - Green: under 90% used
  - Yellow: 90 – 99% used
  - Orange: 99 – 100% used
  - Red: over budget
- **Per-block contribution** — bytes each block contributes to the group total, in both decimal and hex

Decimal byte counts use thousand separators for readability; hex is preserved in parentheses for ROM-hacker workflows.

### `ConfigBattletoads.json` now ships in source
Previously this config existed only in the 1.0 release zip and was missing from the repository, leaving fork users without a working BattleToads-edition config. It's now committed at the repo root so future builds and forks include it by default.

### Compatibility
- No changes to the script JSON format — `ExtractedScript.json` files from 1.0 work unchanged.
- No changes to ROM read/write behavior — Extract / Insert produce byte-identical results to 1.0 when given the same script and config.

### How to use
Unzip and run `JeopardyNesTextTool.exe`. To work with the BattleToads hack, rename `ConfigBattletoads.json` to `Config.json` (overwriting the 25th Anniversary version), as documented in the README.

---
**Full changelog:** https://github.com/breastickle/JeopardyNesTextTool/compare/1.0...v1.1
