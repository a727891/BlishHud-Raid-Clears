using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Settings.Enums;

namespace RaidClears.Raids.Services
{
    public class ApiPollService : IDisposable
    {
        protected static int BUFFER_MS = 50;
        protected static int MINUTE_MS = 60000;

        protected bool _running = true;
        protected double _runningTimer = 0;
        protected double _timeoutValue = 0;

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
            if (_running)
            {
                _runningTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

                if(_runningTimer >= _timeoutValue)
                {
                    ApiPollingTrigger?.Invoke(this, true);
                    _runningTimer = 0;
                }

            }
        }

        public void Invoke()
        {
            _runningTimer = 0;
            ApiPollingTrigger?.Invoke(this, true);
        }

        private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod> e)
        {
            SetTimeoutValueInMinutes((int)e.NewValue);         
        }

        private void SetTimeoutValueInMinutes(int minutes)
        {
            _timeoutValue = (minutes * MINUTE_MS) + BUFFER_MS;
        }


        private readonly SettingEntry<ApiPollPeriod> _apiPollSetting;
    }
}