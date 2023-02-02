namespace RaidClears.Features.Shared.Enums.Extensions;

public static class MapIdsExtensions
{

    public static string GetApiLabel(this MapIds.StrikeMaps value)
    {
        return value switch
        {
            MapIds.StrikeMaps.ColdWar =>                Encounters.StrikeMission.ColdWar.GetApiLabel(),
            MapIds.StrikeMaps.ColdWarPublic =>          Encounters.StrikeMission.ColdWar.GetApiLabel(),
            MapIds.StrikeMaps.Fraenir =>                Encounters.StrikeMission.Fraenir.GetApiLabel(),
            MapIds.StrikeMaps.FraenirPublic =>          Encounters.StrikeMission.Fraenir.GetApiLabel(),
            MapIds.StrikeMaps.ShiverpeaksPass =>        Encounters.StrikeMission.ShiverpeaksPass.GetApiLabel(),
            MapIds.StrikeMaps.ShiverpeaksPassPublic =>  Encounters.StrikeMission.ShiverpeaksPass.GetApiLabel(),
            MapIds.StrikeMaps.VoiceAndClaw =>           Encounters.StrikeMission.VoiceAndClaw.GetApiLabel(),
            MapIds.StrikeMaps.VoiceAndClawPublic =>     Encounters.StrikeMission.VoiceAndClaw.GetApiLabel(),
            MapIds.StrikeMaps.Whisper =>                Encounters.StrikeMission.Whisper.GetApiLabel(),
            MapIds.StrikeMaps.WhisperPublic =>          Encounters.StrikeMission.Whisper.GetApiLabel(),
            MapIds.StrikeMaps.Boneskinner =>            Encounters.StrikeMission.Boneskinner.GetApiLabel(),
            MapIds.StrikeMaps.BoneskinnerPublic =>      Encounters.StrikeMission.Boneskinner.GetApiLabel(),

            MapIds.StrikeMaps.AetherbladeHideout =>     Encounters.StrikeMission.AetherbladeHideout.GetApiLabel(),
            MapIds.StrikeMaps.Junkyard =>               Encounters.StrikeMission.Junkyard.GetApiLabel(),
            MapIds.StrikeMaps.Overlook =>               Encounters.StrikeMission.Overlook.GetApiLabel(),
            MapIds.StrikeMaps.HarvestTemple =>          Encounters.StrikeMission.HarvestTemple.GetApiLabel(),
            MapIds.StrikeMaps.OldLionsCourt =>          Encounters.StrikeMission.OldLionsCourt.GetApiLabel(),
            _ => "Not a valid Strike Instance Map"
        };
    }

    public static string GetLabel(this MapIds.StrikeMaps value)
    {
        return value switch
        {
            MapIds.StrikeMaps.ColdWar => Encounters.StrikeMission.ColdWar.GetLabel(),
            MapIds.StrikeMaps.ColdWarPublic => Encounters.StrikeMission.ColdWar.GetLabel(),
            MapIds.StrikeMaps.Fraenir => Encounters.StrikeMission.Fraenir.GetLabel(),
            MapIds.StrikeMaps.FraenirPublic => Encounters.StrikeMission.Fraenir.GetLabel(),
            MapIds.StrikeMaps.ShiverpeaksPass => Encounters.StrikeMission.ShiverpeaksPass.GetLabel(),
            MapIds.StrikeMaps.ShiverpeaksPassPublic => Encounters.StrikeMission.ShiverpeaksPass.GetLabel(),
            MapIds.StrikeMaps.VoiceAndClaw => Encounters.StrikeMission.VoiceAndClaw.GetLabel(),
            MapIds.StrikeMaps.VoiceAndClawPublic => Encounters.StrikeMission.VoiceAndClaw.GetLabel(),
            MapIds.StrikeMaps.Whisper => Encounters.StrikeMission.Whisper.GetLabel(),
            MapIds.StrikeMaps.WhisperPublic => Encounters.StrikeMission.Whisper.GetLabel(),
            MapIds.StrikeMaps.Boneskinner => Encounters.StrikeMission.Boneskinner.GetLabel(),
            MapIds.StrikeMaps.BoneskinnerPublic => Encounters.StrikeMission.Boneskinner.GetLabel(),

            MapIds.StrikeMaps.AetherbladeHideout => Encounters.StrikeMission.AetherbladeHideout.GetLabel(),
            MapIds.StrikeMaps.Junkyard => Encounters.StrikeMission.Junkyard.GetLabel(),
            MapIds.StrikeMaps.Overlook => Encounters.StrikeMission.Overlook.GetLabel(),
            MapIds.StrikeMaps.HarvestTemple => Encounters.StrikeMission.HarvestTemple.GetLabel(),
            MapIds.StrikeMaps.OldLionsCourt => Encounters.StrikeMission.OldLionsCourt.GetLabel(),
            _ => "Not a valid Strike Instance Map"
        };
    }

}