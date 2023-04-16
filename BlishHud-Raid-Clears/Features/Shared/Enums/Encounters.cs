using RaidClears.Features.Shared.Models;

namespace RaidClears.Features.Shared.Enums;

public enum StrikeMissionType
{
    Ibs,
    Eod,
    Priority
}

public static class Encounters
{
    public enum RaidBosses
    {
        ValeGuardian,
        SpiritWoods,
        Gorseval,
        Sabetha,
        
        Slothasor,
        BanditTrio,
        Matthias,
        
        Escort,
        KeepConstruct,
        TwistedCastle,
        Xera,
        
        Cairn,
        MursaatOverseer,
        Samarog,
        Deimos,
        
        SoulessHorror,
        RiverOfSouls,
        StatuesOfGrenth,
        VoiceInTheVoid,
        
        ConjuredAmalgamate,
        TwinLargos,
        Qadim,
        
        Gate,
        Adina,
        Sabir,
        QadimThePeerless,
    }
    
    public enum StrikeMission
    {
        ShiverpeaksPass,
        Fraenir,
        VoiceAndClaw,
        Whisper,
        Boneskinner,
        ColdWar,

        AetherbladeHideout,
        Junkyard,
        Overlook,
        HarvestTemple,
        OldLionsCourt,
    }

    public enum Dungeons
    {
        AscalonianCatacombs,
        CaudecusManor,
        TwilightArbor,
        SorrowsEmbrace,
        CitadelOfFlame,
        HonorOfTheWaves,
        CrucibleOfEternity,
        RuinedCityOfArah,
    }

    public enum DungeonPaths
    {
        AscalonianCatacombsStory,
        AscalonianCatacombsHodgins,
        AscalonianCatacombsDetha,
        AscalonianCatacombsTzark,
        /*  "AC",
              new Path("ac_story","Story", "S"),
              new Path("hodgins","hodgins", "E1"),
              new Path("detha","detha", "E2"),
              new Path("tzark","tzark", "E3"),*/
        CaudecusManorStory,
        CaudecusManorAsura,
        CaudecusManorSeraph,
        CaudecusManorButler,
        /*"CM",
            new Path("cm_story","Story", "S"),
            new Path("asura","asura", "E1"),
            new Path("seraph","seraph", "E2"),
            new Path("butler","butler", "E3"),*/
        TwilightArborStory,
        TwilightArborLeurent,
        TwilightArborVevina,
        TwilightArborAetherPath,
        /*"TA",
            new Path("ta_story","Story", "S"),
            new Path("leurent","leurent (Up)", "Up"),
            new Path("vevina","vevina (Forward)", "Fwd"),
            new Path("aetherpath","aetherpath", "Ae"),*/
        SorrowsEmbraceStory,
        SorrowsEmbraceFergg,
        SorrowsEmbraceRasalov,
        SorrowsEmbraceKoptev,
        /*"SE",
            new Path("se_story","Story", "S"),
            new Path("fergg","fergg", "E1"),
            new Path("rasalov","rasalov", "E2"),
            new Path("koptev","koptev", "E3"),*/
        CitadelOfFlameStory,
        CitadelOfFlameFerrah,
        CitadelOfFlameMagg,
        CitadelOfFlameRhiannon,
        /*"CoF",
            new Path("cof_story","Story", "S"),
            new Path("ferrah","ferrah", "E1"),
            new Path("magg","magg", "E2"),
            new Path("rhiannon","rhiannon", "E3"),*/
        HonorOfTheWavesStory,
        HonorOfTheWavesButcher,
        HonorOfTheWavesPlunderer,
        HonorOfTheWavesZealot,
        /*"HW",
            new Path("hotw_story","Story", "S"),
            new Path("butcher","butcher", "E1"),
            new Path("plunderer","plunderer", "E2"),
            new Path("zealot","zealot", "E3"),*/
        CrucibleOfEternityStory,
        CrucibleOfEternitySubmarine,
        CrucibleOfEternityTeleporter,
        CrucibleOfEternityFrontDoor,
        /*"CoE",
            new Path("coe_story","Story", "S"),
            new Path("submarine","submarine", "E1"),
            new Path("teleporter","teleporter", "E2"),
            new Path("front_door","front_door", "E3"),*/
        RuinedCityOfArahStory,
        RuinedCityOfArahJotun,
        RuinedCityOfArahMursaat,
        RuinedCityOfArahForgotten,
        RuinedCityOfArahSeer,
        /*"Arah",
            //new Path("arah_story","Story", "S"),
            new Path("jotun","jotun", "E1"),
            new Path("mursaat","mursaat", "E2"),
            new Path("forgotten","forgotten", "E3"),
            new Path("seer","seer", "E4"),*/
    }

    public enum Fractal
    {
        //MistlockObservatory,
        AetherbladeFractal,
        AquaticRuinsFractal,
        CaptainMaiTrinBossFractal,
        ChaosFractal,
        CliffsideFractal,
        DeepstoneFractal,
        MoltenBossFractal,
        MoltenFurnaceFractal,
        NightmareFractal,
        ShatteredObservatoryFractal,
        SirensReefFractal,
        SnowblindFractal,
        SunquaPeakFractal,
        SolidOceanFractal,
        SwamplandFractal,
        ThaumanovaReactorFractal,
        TwilightOasisFractal,
        UncategorizedFractal,
        UndergroundFacilityFractal,
        UrbanBattlegroundFractal,
        VolcanicFractal
    }
}

