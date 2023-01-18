﻿using System;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Enums.Extensions;

public static class StrikeMissionExtensions
{
    public static string GetLabel(this Encounters.StrikeMission value)
    {
        return value switch
        {
            Encounters.StrikeMission.ShiverpeaksPass => Strings.Setting_Strike_SP_Label,
            Encounters.StrikeMission.ColdWar => Strings.Setting_Strike_CW_Label,
            Encounters.StrikeMission.Fraenir => Strings.Setting_Strike_FoJ_Label,
            Encounters.StrikeMission.VoiceAndClaw => Strings.Setting_Strike_VandC_Label,
            Encounters.StrikeMission.Whisper => Strings.Setting_Strike_WoJ_Label,
            Encounters.StrikeMission.Boneskinner => Strings.Setting_Strike_BS_Label,
            Encounters.StrikeMission.AetherbladeHideout => Strings.Setting_Strike_AH_Label,
            Encounters.StrikeMission.Junkyard => Strings.Setting_Strike_XJJ_Label,
            Encounters.StrikeMission.Overlook => Strings.Setting_Strike_KO_Label,
            Encounters.StrikeMission.HarvestTemple => Strings.Setting_Strike_HT_Label,
            Encounters.StrikeMission.OldLionsCourt => Strings.Setting_Strike_OLC_Label,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    
    public static string GetLabelShort(this Encounters.StrikeMission value)
    {
        return value switch
        {
            Encounters.StrikeMission.ShiverpeaksPass => "SP",
            Encounters.StrikeMission.ColdWar => "CW",
            Encounters.StrikeMission.Fraenir => "FoJ",
            Encounters.StrikeMission.VoiceAndClaw => "Bear",
            Encounters.StrikeMission.Whisper => "WoJ",
            Encounters.StrikeMission.Boneskinner => "BS",
            Encounters.StrikeMission.AetherbladeHideout => "AH",
            Encounters.StrikeMission.Junkyard => "XJJ",
            Encounters.StrikeMission.Overlook => "KO",
            Encounters.StrikeMission.HarvestTemple => "HT",
            Encounters.StrikeMission.OldLionsCourt => "OLC",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string GetApiLabel(this Encounters.StrikeMission value)
    {
        return value switch
        {
            Encounters.StrikeMission.ShiverpeaksPass => "shiverpeak_pass",
            Encounters.StrikeMission.ColdWar => "cold_war",
            Encounters.StrikeMission.Fraenir => "fraenir_of_jormag",
            Encounters.StrikeMission.VoiceAndClaw => "voice_and_claw",
            Encounters.StrikeMission.Whisper => "whisper_of_jormag",
            Encounters.StrikeMission.Boneskinner => "boneskinner",
            Encounters.StrikeMission.AetherbladeHideout => "aetherblade_hideout",
            Encounters.StrikeMission.Junkyard => "xunlai_jade_junkyard",
            Encounters.StrikeMission.Overlook => "kaineng_overlook",
            Encounters.StrikeMission.HarvestTemple => "harvest_temple",
            Encounters.StrikeMission.OldLionsCourt => "old_lion_court",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    
    public static StrikeMissionType GetType(this Encounters.StrikeMission value)
    {
        return value switch
        {
            Encounters.StrikeMission.ShiverpeaksPass => StrikeMissionType.Ibs,
            Encounters.StrikeMission.ColdWar => StrikeMissionType.Ibs,
            Encounters.StrikeMission.Fraenir => StrikeMissionType.Ibs,
            Encounters.StrikeMission.VoiceAndClaw => StrikeMissionType.Ibs,
            Encounters.StrikeMission.Whisper => StrikeMissionType.Ibs,
            Encounters.StrikeMission.Boneskinner => StrikeMissionType.Ibs,
            
            Encounters.StrikeMission.AetherbladeHideout => StrikeMissionType.Eod,
            Encounters.StrikeMission.Junkyard => StrikeMissionType.Eod,
            Encounters.StrikeMission.Overlook => StrikeMissionType.Eod,
            Encounters.StrikeMission.HarvestTemple => StrikeMissionType.Eod,
            Encounters.StrikeMission.OldLionsCourt => StrikeMissionType.Eod,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}