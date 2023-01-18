using System;
using Blish_HUD;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using RaidClears.Settings.Enums;

namespace RaidClears.Features.Shared.Services;

public class ApiPollService : IDisposable
{
    protected static int bufferMs = 50;
    protected static int minuteMs = 60000;

    protected bool running = true;
    protected double runningTimer = -20000;
    protected double timeoutValue;

    public event EventHandler<bool> ApiPollingTrigger;

    public ApiPollService(SettingEntry<ApiPollPeriod> apiPollSetting)
    {

        _apiPollSetting = apiPollSetting;

        _apiPollSetting.SettingChanged += OnSettingUpdate;
        SetTimeoutValueInMinutes((int)_apiPollSetting.Value);

    }

    public void Dispose()
    {
        _apiPollSetting.SettingChanged -= OnSettingUpdate;


    }

    public void Update(GameTime gameTime)
    {
        if (running)
        {
            runningTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (runningTimer >= timeoutValue)
            {
                ApiPollingTrigger.Invoke(this, true);
                runningTimer = 0;
            }

        }
    }

    public void Invoke()
    {
        runningTimer = 0;
        ApiPollingTrigger.Invoke(this, true);
    }

    private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod> e) => SetTimeoutValueInMinutes((int)e.NewValue);

    private void SetTimeoutValueInMinutes(int minutes)
    {
        timeoutValue = minutes * minuteMs + bufferMs;
    }


    private readonly SettingEntry<ApiPollPeriod> _apiPollSetting;
}