# Release notes

User-facing patch notes for Clears Tracker. Newest releases first.

---

## 3.7.0 (2026-02-22)

### Raid Encounters

- **API Tracking** - completion is now read from the game’s weekly achievement (Weekly Raid Encounters). When the module refreshes from the API, your clears update to match.

- **Non-Weekly Highlight** - The raid encounters panel can highlight encounters that won't be daily bounties for the rest of the week. Same as the previously released Raid Wing highlighting

- **Dragonstorm (IBS)** - Dragonstorm is not in the API and will still work by map changes as it has in the past. It is also once again marked as a daily clear.

- **Manage Clears** - The "Manage Clears" setting page has been removed

---

## 3.6.0 (2026-02-18)

### Mentor Progress Popups

- **Choose where popups appear** - A new option in Raid settings lets you move the mentor progress popup to a spot you prefer. Turn on "Reposition mentor progress popup", drag the example popup where you want it, then turn the option off to lock that position. Future mentor progress popups will show there.

### Translations

- The new repositioning option and its example popup are available in **German**, **French**, and **Spanish** (in addition to English).

### Recent Fix (3.5.1)

- **Raid encounter clears** - Completions now register correctly when you go straight from one raid or strike to another without leaving to the lobby in between.

---

## 3.5.0 (2026-02-17)

### Daily Raid Bounties

- **API tracking** - Daily raid bounty completion is now loaded from the game API, so the module shows which bounties you’ve already done.
- **Non-weekly highlight** - Option to highlight raid encounters that will not be daily bounties for the rest of the week, so you can tell them apart from this week’s bounty rotation.
- **Label and visibility fixes** - Custom labels and visibility for bounty encounters now save and load correctly. Existing settings are migrated automatically.
- **Strike daily priority removed** - The old “Daily Bounty” / “Tomorrow” strike sections are gone; daily bounties are now shown through the raid bounty system.

### Raids & Strikes

- **Unified encounter handling** - Raids and strikes now share the same encounter logic, so labels and tooltips behave consistently and stay in sync.

### Other

- **Panel position** - Fixed panel location not saving correctly and updated default positions.
