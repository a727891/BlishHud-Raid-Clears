# Change Log

All notable changes to Clears Tracker will be documented in this file.

## 3.6.0 (2026-02-18)
* **Mentor progress popup repositioning** – New setting to show a draggable example popup so you can choose where mentor progress popups appear; disable the setting to lock the position
* Localized mentor progress repositioning setting and example popup title (DE, FR, ES)

## 3.5.1 (2026-02-18)
* Fixed raid encounter completion trigger when map changes from raid-to-raid without leaving a raid map
## 3.5.0 (2026-02-17)
* **Daily raid bounty tracking via API** – Bounty completion state is now fetched from the API and reflected in the UI
* **Unified raids and strikes** – Shared `BossEncounter` model and `IEncounter`/`IExpansion` APIs; removed duplicate RaidEncounter/StrikeMission types; raid and strike labels and tooltips use a single encounter-based flow
* **Daily raid bounties fixes** – Custom labels and visibility for bounty encounters no longer persist `priority_`/`tomorrow_` in settings JSON; storage uses normalized keys; migration merges legacy prefixed keys into base ids; strike missions in bounties use strike labelables and update when labels change
* **Removed old strike daily priority** – Replaced by raid bounties; removed strike Priority/Tomorrow sections, DailyBounty/DailyBountyTomorrow strike groups, and related settings
* **Non-weekly highlight** – Option to highlight raid encounters that will not be daily bounties for the rest of the week (distinct from this week’s bounty rotation)
* **Panel location** – Fixed panel save location and updated default positions

## 3.4.1 (2026-02-12)
* Fixed DRB `priority_` ids
## 3.4.0 (2026-02-11)
* Added DailyBounties
* Added Raid Mentory tracking
* Dragging Raids/Strikes moves the other in real time when anchored

## 3.3.1 (2026-01-25)
* Added corner icon tooltip with account name and MOTD support
* Added corner icon notification dot for new messages
* Added raid boss damage type and break bar tooltips
* Added fractal CM selection settings
* Improved internationalization (i18n) support
* Fixed localization issues for fractal maps and raid wings
* Fixed UTF-8 encoding issues in dynamic labels

## 3.3.0 (2026-01-23)
* Renamed "Strikes" to "Raid Encounters" throughout the module
* Added config file API versioning and unified file names
* Moved most strings to resource files for better localization
* Added open color picker button in settings
* Added Janthir Wilds raid support
* Improved French translations (Thanks Naru!)
* Made settings window buttons wider to accommodate longer translations

## 3.2.0 (2024-12-14)
* Added fancy tooltips with encounter information
* Added label customization for encounters
* Added more fine-grain visibility controls
* Added stylized boxes option
* Added raid wing visibility controls

## 3.0.0 (2024-05-24)
* Updated for May 2024 game patch
* Added Lonely Tower fractal
* Added "Keep overlays on screen" setting to prevent dragging windows outside screen bounds
* Adjusted Fractal Recs and Fractal DailyTier with known data
* Automatic Fractal and Strike updates - data now downloaded from BlishHUD static hosting on module load
* Automatic Fractal Instabilities updates from static hosting
* Reduced module size by pushing image assets to static hosting

## 2.6.1 (2024-05-22)
* Version bump for public repo release
* Added screen-clamp setting

## 2.6.0 (2024-05-21)
* Updated for Soto fractal
* Instabilities are loaded from bhud-static
* Fractal data is loaded from bhud-static

## 2.5.0
* (No changelog entry)

## 2.4.0 (2024-01-11)
* Added Fractal CM with Instabilities

---

## 2.3.0 (2023-08-24)
* Added SotO Strike missions

## 2.2.3 (2023-08-22)
* Added Soto fix patch for blish corner icon offset

## 2.2.2 (2023-07-30)
* Added Dragonstorm to IBS strikes list

## 2.2.1 (2023-07-12)
* Finalized Silent Surf fractal changes
* Added strike and fractal clear management setting page

## 2.2.0 (2023-06-28)
* Added Silent Surf support

## 2.1.0 (2023-04-20)
* (No changelog entry)

## 2.0.5 (2023-03-19)
* Added Wing 8 for april fools joke

## 2.0.4 (2023-03-01)
* Fixed daily strike index using Anets day of year index calculation

## 2.0.3 (2023-02-26)
* Fixed race condition

## 2.0.2 (2023-02-13)
* Fixed keybinds not activating

## 2.0.1 (2023-02-12)
* Version bump for 2.0.0 full release

## 2.0.0 (2023-02-01)
* Complete rewrite
* Added strike functionality

## 1.4.1 (2023-01-11)
* Settings are expanded by default

## 1.4.0 (2022-12-31)
* Added color customization

## 1.3.0 (2022-10-23)
* Added Call of the Mists and Emboldened

## 1.2.0 (2022-07-31)
* Added Dungeon tracking

## 1.1.2 (2022-07-22)
* Fixed Opacity settings not being applied at module load
* Fixed FontSize scaling by changing label widths and limiting available sizes
* Added ApiPollPeriod setting to increase or decrease polling rate

## 1.1.1 (2022-07-21)
* Added IsInGame and MapOpen filter checks to hide the panel properly

## 1.1.0 (2022-07-21)
* Rewrote using Eksofa's SDK style repo
