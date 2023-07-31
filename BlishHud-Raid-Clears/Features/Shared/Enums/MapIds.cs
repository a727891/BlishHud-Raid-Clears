using RaidClears.Features.Shared.Enums;
using RaidClears.Features.Shared.Models;
using RaidClears.Features.Strikes.Services;
using System.Collections.Generic;

namespace RaidClears.Features.Shared.Enums;

public static class MapIds
{

    /**
    * The Icebrood Saga
    * 0   Cold War  https://api.guildwars2.com/v2/maps?ids=1374,1376&lang=en
    * 1   Fraenir of Jormag https://api.guildwars2.com/v2/maps?ids=1341,1344&lang=en
    * 2   Shiverpeaks Pass https://api.guildwars2.com/v2/maps?ids=1331,1332&lang=en
    * 3   Voice of the Fallen and Claw of the Fallen https://api.guildwars2.com/v2/maps?ids=1340,1346&lang=en
    * 4   Whisper of Jormag https://api.guildwars2.com/v2/maps?ids=1357,1359&lang=en
    * 5   Boneskinner https://api.guildwars2.com/v2/maps?ids=1339,1351&lang=en
    * End of Dragons
    * 0	Aetherblade Hideout https://api.guildwars2.com/v2/maps?ids=1432&lang=en
    * 1	Xunlai Jade Junkyard https://api.guildwars2.com/v2/maps?ids=1450&lang=en
    * 2	Kaineng Overlook https://api.guildwars2.com/v2/maps?ids=1451&lang=en
    * 3	Harvest Temple https://api.guildwars2.com/v2/maps?ids=1437&lang=en
    * 4	Old Lion's Court https://api.guildwars2.com/v2/maps?ids=1485&lang=en
    **/
    public enum StrikeMaps
    {
        ShiverpeaksPass=1331,
        ShiverpeaksPassPublic=1332,
        Fraenir=1341,
        FraenirPublic=1344,
        VoiceAndClaw=1346,
        VoiceAndClawPublic=1340,
        Whisper=1359,
        WhisperPublic=1357,
        Boneskinner=1339,
        BoneskinnerPublic=1351,
        ColdWar = 1374,
        ColdWarPublic = 1376,

        AetherbladeHideout = 1432,
        Junkyard = 1450,
        Overlook = 1451,
        HarvestTemple = 1437,
        OldLionsCourt = 1485,

        DragonStorm=1409,
        DragonStormPublic=1411
    }

    public enum FractalMaps
    {
        //MistlockObservatory = 872,
        AetherbladeFractal = 956,
        AquaticRuinsFractal = 951,
        CaptainMaiTrinBossFractal = 960,
        ChaosFractal = 1164,
        CliffsideFractal = 952,
        DeepstoneFractal = 1290,
        MoltenBoss = 959,
        MoltenFurnace = 955,
        Nightmare = 1177,
        ShatteredObservatory = 1205,
        SirensReef = 1309,
        SilentSurf = 1500,
        SnowblindFractal = 948,
        SunquaPeak = 1384,
        SolidOceanFractal = 958,
        SwamplandFractal = 949,
        ThaumanovaReactor = 957,
        TwilightOasis = 1267,
        UncategorizedFractal = 947,
        UndergroundFacilityFractal = 953,
        UrbanBattlegroundFractal = 950,
        VolcanicFractal = 954,
    }

}       

