using System;
using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Input;
using Blish_HUD.Settings;
using Microsoft.Xna.Framework;
using Settings.Enums;

namespace RaidClears.Raids.Services
{
    public class WingRotationService : IDisposable
    {

        private const int EMBOLDEN_START_TIMESTAMP = 1656315000;
        private const int WEEKLY_SECONDS = 604800;
        private const int NUMBER_OF_WINGS = 7;


        public WingRotationService()
        {


        }

        public (int,int) getHighlightedWingIndices()
        {

            DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;

            var duration = now.ToUnixTimeSeconds() - EMBOLDEN_START_TIMESTAMP;

            var wing = (int) Math.Floor((decimal) (duration / WEEKLY_SECONDS)) % NUMBER_OF_WINGS;

            return (wing, (wing + 1) % NUMBER_OF_WINGS);
            
        }

        public void Dispose()
        {
            

        }

    }
}