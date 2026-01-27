# Clears Tracker
*(formerly Raid Clears)*

A Blish HUD module that displays your weekly raid, ~~strike~~ raid encounter, fractal, and dungeon clear status in an elegant, customizable overlay.

---

## For support, bug reports, or feature requests, please visit the [Blish HUD Discord](https://discord.gg/FYKN3qh) or open an issue on GitHub.

---

## Overview

Clears Tracker automatically tracks your weekly PvE instance clears using the Guild Wars 2 API. The module displays your progress through raids, strikes, fractals, and dungeons in an easy-to-read grid format that you can customize to match your preferences.

## Requirements

- **Blish HUD** (v1.0.0 or higher)
- **Guild Wars 2 API Token** with the following permissions:
  - `account` (required)
  - `progression` (required)

## Features

### Core Functionality
- **Automatic Tracking** - Fetches clear data directly from the Guild Wars 2 API
- **Multiple Content Types** - Track Raids, Strikes, Fractals, and Dungeons
- **Real-time Updates** - Configurable API polling rate for up-to-date information
- **Weekly Reset Tracking** - Automatically resets clears on weekly reset

### Customization Options
- **Layout Options** - Horizontal, Vertical, Single Row, or Single Column layouts
- **Label Display** - Choose between full names, abbreviations, or numbers
- **Custom Labels** - Customize individual encounter labels
- **Color Customization** - Full color control for cleared/uncleared states and backgrounds
- **Font Size** - Adjustable font sizes for better readability
- **Opacity Controls** - Fine-tune transparency for labels, grids, and backgrounds
- **Visibility Controls** - Show/hide individual wings, encounters, or entire content types

### User Experience
- **Corner Icon** - Quick access with tooltip showing account name and MOTD
- **Notification System** - Visual indicator for new messages and updates
- **Keybind Support** - Assign hotkeys to quickly show/hide panels
- **Screen Clamping** - Optional setting to keep overlays within screen bounds
- **Tooltips** - Hover over encounters for detailed information including damage types and break bars
- **Drag & Drop** - Reposition panels anywhere on your screen (when unlocked)

### Advanced Features
- **Automatic Updates** - Fractal and strike data automatically updates from static hosting
- **Internationalization** - Multi-language support with localization
- **Clear Management** - Manual clear correction tools for edge cases
- **Stylized Boxes** - Optional organic grid box backgrounds

## Installation

1. Install [Blish HUD](https://blishhud.com/)
2. Open Blish HUD and navigate to the **Modules** tab
3. Search for "Clears Tracker" or "Raid Clears"
4. Click **Install**
5. Configure your API token in Blish HUD settings (Account → API Key)
6. The module will automatically start tracking your clears!

## Usage

Once installed, the module will display panels for each enabled content type. Use the corner icon (top-right of screen) to:
- **Left-click** - Toggle visibility of all panels
- **Right-click** - Open context menu with additional options
- **Hover** - View account name and any important messages

Access settings by right-clicking the corner icon and selecting "Open Settings", or through Blish HUD's module settings page.

## Change Log

See [CHANGELOG.md](CHANGELOG.md) for a complete list of changes and version history.

## Credits

**Developers:**
- **Soeed** - Primary developer
- **Abbadon** - Contributor

**Inspiration:**
Heavily inspired by the same feature bundled with Gw2TaCO.

**Translations:**
- French translations by Naru

---

## Links

- [Blish HUD Module Page](https://blishhud.com/modules/?module=Soeed.RaidClears)
- [GitHub Repository](https://github.com/a727891/BlishHud-Raid-Clears)

