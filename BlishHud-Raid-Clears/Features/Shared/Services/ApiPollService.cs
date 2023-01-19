using System;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Enums;

namespace RaidClears.Features.Shared.Services;

public class ApiPollService : IDisposable
{
    private const int BUFFER_MS = 50;
    private const int MINUTE_MS = 60000;

    private double _runningTimer = -20000;
    private double _timeoutValue;
    
    private readonly SettingEntry<ApiPollPeriod> _apiPollSetting;

    public event EventHandler<bool>? ApiPollingTrigger;

    public ApiPollService(SettingEntry<ApiPollPeriod> apiPollSetting)
    {
        _apiPollSetting = apiPollSetting;
        SetTimeoutValueInMinutes((int)_apiPollSetting.Value);

        _apiPollSetting.SettingChanged += OnSettingUpdate;
    }

    public void Dispose()
    {
        _apiPollSetting.SettingChanged -= OnSettingUpdate;
    }

    public void Update(GameTime gameTime)
    {
        _runningTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

        if (_runningTimer >= _timeoutValue)
        {
            ApiPollingTrigger?.Invoke(this, true);
            _runningTimer = 0;
        }
    }

    public void Invoke()
    {
        _runningTimer = 0;
        ApiPollingTrigger?.Invoke(this, true);
    }

    private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod> e) => SetTimeoutValueInMinutes((int)e.NewValue);

    private void SetTimeoutValueInMinutes(int minutes) => _timeoutValue = minutes * MINUTE_MS + BUFFER_MS;
}