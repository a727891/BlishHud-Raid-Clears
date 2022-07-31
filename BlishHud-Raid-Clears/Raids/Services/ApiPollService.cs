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
        private static int BUFFER_MS = 50;
        private static int MINUTE_MS = 60000;

        private bool _running = false;
        private double _runningTimer = 0;
        private double _timeoutValue = 0;
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


            }
        }



        private void OnSettingUpdate(object sender, ValueChangedEventArgs<ApiPollPeriod>e)
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