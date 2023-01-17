using System;
using Blish_HUD.Input;
using Blish_HUD.Settings;


namespace RaidClears.Features.Shared.Services;

public class KeybindHandlerService : IDisposable
{
    public KeybindHandlerService(
        SettingEntry<KeyBinding> keybindSetting,
        SettingEntry<bool> toggleControlSetting
    )
    {

        _keybindSetting          = keybindSetting;
        _toggleControlSetting    = toggleControlSetting;
       
        _keybindSetting.Value.Activated += OnKeybindActivated;

    }

    public void Dispose()
    {
        _keybindSetting.Value.Activated -= OnKeybindActivated;
    }

    protected void OnKeybindActivated(object sender, EventArgs e)
    {
        _toggleControlSetting.Value = !_toggleControlSetting.Value;
    }

    private readonly SettingEntry<KeyBinding> _keybindSetting;
    private readonly SettingEntry<bool> _toggleControlSetting;
}