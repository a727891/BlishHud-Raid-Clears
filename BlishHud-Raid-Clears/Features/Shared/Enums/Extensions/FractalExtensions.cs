﻿using System;
using System.Collections.Generic;
using RaidClears.Features.Raids.Models;
using RaidClears.Localization;

namespace RaidClears.Features.Shared.Enums.Extensions;


public static class FractalExtensions
{
    public static string GetLabel(this Encounters.Fractal value)
    {
        return value switch
        {
            //Encounters.Fractal.MistlockObservatory => "mistlock_observatory",
            Encounters.Fractal.AetherbladeFractal => "Aetherblade Retreat",
            Encounters.Fractal.AquaticRuinsFractal => "Aquatic Ruins",
            Encounters.Fractal.CaptainMaiTrinBossFractal => "Captain Mai Trin Boss",
            Encounters.Fractal.ChaosFractal => "Chaos",
            Encounters.Fractal.CliffsideFractal => "Cliffside",
            Encounters.Fractal.DeepstoneFractal => "Deepstone",
            Encounters.Fractal.MoltenBossFractal => "Molten Boss",
            Encounters.Fractal.MoltenFurnaceFractal => "Molten Furnace",
            Encounters.Fractal.NightmareFractal => "Nightmare",
            Encounters.Fractal.ShatteredObservatoryFractal => "Shattered Observatory",
            Encounters.Fractal.SirensReefFractal => "Siren's Reef",
            Encounters.Fractal.SilentSurfFractal => "Silent Surf",
            Encounters.Fractal.SnowblindFractal => "Snowblind",
            Encounters.Fractal.SunquaPeakFractal => "Sunqua Peak",
            Encounters.Fractal.SolidOceanFractal => "Solid Ocean",
            Encounters.Fractal.SwamplandFractal => "Swampland",
            Encounters.Fractal.ThaumanovaReactorFractal => "Thaumanova Reactor",
            Encounters.Fractal.TwilightOasisFractal => "Twilight Oasis",
            Encounters.Fractal.UncategorizedFractal => "Uncategorized",
            Encounters.Fractal.UndergroundFacilityFractal => "Underground Facility",
            Encounters.Fractal.UrbanBattlegroundFractal => "Urban Battleground",
            Encounters.Fractal.VolcanicFractal => "Volcanic",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    
    public static string GetLabelShort(this Encounters.Fractal value)
    {
        return value switch
        {
            //Encounters.Fractal.MistlockObservatory => "mistlock_observatory",
            Encounters.Fractal.AetherbladeFractal => "Aeth",
            Encounters.Fractal.AquaticRuinsFractal => "Aqua",
            Encounters.Fractal.CaptainMaiTrinBossFractal => "Mai",
            Encounters.Fractal.ChaosFractal => "Chaos",
            Encounters.Fractal.CliffsideFractal => "Clff",
            Encounters.Fractal.DeepstoneFractal => "Deep",
            Encounters.Fractal.MoltenBossFractal => "MBoss",
            Encounters.Fractal.MoltenFurnaceFractal => "MFurn",
            Encounters.Fractal.NightmareFractal => "Night",
            Encounters.Fractal.ShatteredObservatoryFractal => "S Obs",
            Encounters.Fractal.SirensReefFractal => "Siren",
            Encounters.Fractal.SilentSurfFractal => "Surf",
            Encounters.Fractal.SnowblindFractal => "Snow",
            Encounters.Fractal.SunquaPeakFractal => "Sun",
            Encounters.Fractal.SolidOceanFractal => "S Ocn",
            Encounters.Fractal.SwamplandFractal => "Swmp",
            Encounters.Fractal.ThaumanovaReactorFractal => "Thau",
            Encounters.Fractal.TwilightOasisFractal => "Twil",
            Encounters.Fractal.UncategorizedFractal => "Uncat",
            Encounters.Fractal.UndergroundFacilityFractal => "U Fac",
            Encounters.Fractal.UrbanBattlegroundFractal => "Urban",
            Encounters.Fractal.VolcanicFractal => "Vol",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static string GetApiLabel(this Encounters.Fractal value)
    {
        return value switch
        {
            //Encounters.Fractal.MistlockObservatory => "mistlock_observatory",
            Encounters.Fractal.AetherbladeFractal => "aetherblade",
            Encounters.Fractal.AquaticRuinsFractal => "aquatic_ruins",
            Encounters.Fractal.CaptainMaiTrinBossFractal => "mai_trin_boss",
            Encounters.Fractal.ChaosFractal => "chaos",
            Encounters.Fractal.CliffsideFractal => "cliffside",
            Encounters.Fractal.DeepstoneFractal => "deepstone",
            Encounters.Fractal.MoltenBossFractal => "molten_boss",
            Encounters.Fractal.MoltenFurnaceFractal => "molten_furnace",
            Encounters.Fractal.NightmareFractal => "nightmare",
            Encounters.Fractal.ShatteredObservatoryFractal => "shattered_observatory",
            Encounters.Fractal.SirensReefFractal => "sirens_reef",
            Encounters.Fractal.SilentSurfFractal => "silent_surf",
            Encounters.Fractal.SnowblindFractal => "snowblind",
            Encounters.Fractal.SunquaPeakFractal => "sunqua_peak",
            Encounters.Fractal.SolidOceanFractal => "solid_ocean",
            Encounters.Fractal.SwamplandFractal => "swampland",
            Encounters.Fractal.ThaumanovaReactorFractal => "thaumanova_reactor",
            Encounters.Fractal.TwilightOasisFractal => "twilight_oasis",
            Encounters.Fractal.UncategorizedFractal => "uncategorized",
            Encounters.Fractal.UndergroundFacilityFractal => "underground_facility",
            Encounters.Fractal.UrbanBattlegroundFractal => "urban_battleground",
            Encounters.Fractal.VolcanicFractal => "volcanic",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

/*    public static List<int> GetScales(this Encounters.Fractal value)
    {
        return value switch
        {
            //Encounters.Fractal.MistlockObservatory => new List<int>() { },
            Encounters.Fractal.AetherbladeFractal => new List<int>() { 14, 46, 65, 71, 96 },
            Encounters.Fractal.AquaticRuinsFractal => new List<int>() { 7, 26, 61, 76 },
            Encounters.Fractal.CaptainMaiTrinBossFractal => new List<int>() { 18, 42, 72, 95 },
            Encounters.Fractal.ChaosFractal => new List<int>() { 13, 30, 38, 63, 88, 97 },
            Encounters.Fractal.CliffsideFractal => new List<int>() { 6, 21, 47, 69, 94 },
            Encounters.Fractal.DeepstoneFractal => new List<int>() { 11, 33, 67, 84 },
            Encounters.Fractal.MoltenBossFractal => new List<int>() { 10, 40, 70, 90 },
            Encounters.Fractal.MoltenFurnaceFractal => new List<int>() { 9, 22, 39, 58, 83 },
            Encounters.Fractal.NightmareFractal => new List<int>() { 23, 48, 73, 98 },
            Encounters.Fractal.ShatteredObservatoryFractal => new List<int>() { 24, 49, 74, 99 },
            Encounters.Fractal.SirensReefFractal => new List<int>() { 12, 37, 54, 78 },
            Encounters.Fractal.SilentSurfFractal => new List<int>() { 12, 37, 54, 78 },
            Encounters.Fractal.SnowblindFractal => new List<int>() { 3, 27, 51, 68, 86, 93 },
            Encounters.Fractal.SunquaPeakFractal => new List<int>() { 25, 50, 75, 100 },
            Encounters.Fractal.SolidOceanFractal => new List<int>() { 20, 35, 45, 60, 80 },
            Encounters.Fractal.SwamplandFractal => new List<int>() { 5, 17, 32, 56, 77, 89 },
            Encounters.Fractal.ThaumanovaReactorFractal => new List<int>() { 15, 34, 43, 55, 64, 82 },
            Encounters.Fractal.TwilightOasisFractal => new List<int>() { 16, 41, 59, 87 },
            Encounters.Fractal.UncategorizedFractal => new List<int>() { 2, 36, 44, 62, 79, 91 },
            Encounters.Fractal.UndergroundFacilityFractal => new List<int>() { 8, 29, 53, 81 },
            Encounters.Fractal.UrbanBattlegroundFractal => new List<int>() { 4, 31, 57, 66, 85 },
            Encounters.Fractal.VolcanicFractal => new List<int>() { 1, 19, 28, 52, 92 },
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }*/

    public static Encounters.Fractal GetFractalForScale(int scale)
    {
        return scale switch
        {
            1 => Encounters.Fractal.VolcanicFractal,
            2 => Encounters.Fractal.UncategorizedFractal,
            3 => Encounters.Fractal.SnowblindFractal,
            4 => Encounters.Fractal.UrbanBattlegroundFractal,
            5 => Encounters.Fractal.SwamplandFractal,
            6 => Encounters.Fractal.CliffsideFractal,
            7 => Encounters.Fractal.AquaticRuinsFractal,
            8 => Encounters.Fractal.UndergroundFacilityFractal,
            9 => Encounters.Fractal.MoltenFurnaceFractal,
            10 => Encounters.Fractal.MoltenBossFractal,
            11 => Encounters.Fractal.DeepstoneFractal,
            12 => Encounters.Fractal.SirensReefFractal,
            13 => Encounters.Fractal.ChaosFractal,
            14 => Encounters.Fractal.AetherbladeFractal,
            15 => Encounters.Fractal.ThaumanovaReactorFractal,
            16 => Encounters.Fractal.TwilightOasisFractal,
            17 => Encounters.Fractal.MoltenFurnaceFractal,
            18 => Encounters.Fractal.CaptainMaiTrinBossFractal,
            19 => Encounters.Fractal.VolcanicFractal,
            20 => Encounters.Fractal.SolidOceanFractal,
            21 => Encounters.Fractal.CliffsideFractal,
            22 => Encounters.Fractal.NightmareFractal,
            23 => Encounters.Fractal.ShatteredObservatoryFractal,
            24 => Encounters.Fractal.SunquaPeakFractal,
            25 => Encounters.Fractal.SilentSurfFractal,
            26 => Encounters.Fractal.AquaticRuinsFractal,
            27 => Encounters.Fractal.SnowblindFractal,
            28 => Encounters.Fractal.VolcanicFractal,
            29 => Encounters.Fractal.UndergroundFacilityFractal,
            30 => Encounters.Fractal.ChaosFractal,
            31 => Encounters.Fractal.UrbanBattlegroundFractal,
            32 => Encounters.Fractal.SwamplandFractal,
            33 => Encounters.Fractal.DeepstoneFractal,
            34 => Encounters.Fractal.ThaumanovaReactorFractal,
            35 => Encounters.Fractal.SolidOceanFractal,
            36 => Encounters.Fractal.UncategorizedFractal,
            37 => Encounters.Fractal.SirensReefFractal,
            38 => Encounters.Fractal.ChaosFractal,
            39 => Encounters.Fractal.MoltenFurnaceFractal,
            40 => Encounters.Fractal.MoltenBossFractal,
            41 => Encounters.Fractal.TwilightOasisFractal,
            42 => Encounters.Fractal.CaptainMaiTrinBossFractal,
            43 => Encounters.Fractal.ThaumanovaReactorFractal,
            44 => Encounters.Fractal.SolidOceanFractal,
            45 => Encounters.Fractal.AetherbladeFractal,
            46 => Encounters.Fractal.CliffsideFractal,
            47 => Encounters.Fractal.NightmareFractal,
            48 => Encounters.Fractal.ShatteredObservatoryFractal,
            49 => Encounters.Fractal.SunquaPeakFractal,
            50 => Encounters.Fractal.SilentSurfFractal,
            51 => Encounters.Fractal.SnowblindFractal,
            52 => Encounters.Fractal.VolcanicFractal,
            53 => Encounters.Fractal.UndergroundFacilityFractal,
            54 => Encounters.Fractal.SirensReefFractal,
            55 => Encounters.Fractal.ThaumanovaReactorFractal,
            56 => Encounters.Fractal.SwamplandFractal,
            57 => Encounters.Fractal.UrbanBattlegroundFractal,
            58 => Encounters.Fractal.MoltenFurnaceFractal,
            59 => Encounters.Fractal.TwilightOasisFractal,
            60 => Encounters.Fractal.SolidOceanFractal,
            61 => Encounters.Fractal.AquaticRuinsFractal,
            62 => Encounters.Fractal.UncategorizedFractal,
            63 => Encounters.Fractal.ChaosFractal,
            64 => Encounters.Fractal.ThaumanovaReactorFractal,
            65 => Encounters.Fractal.AetherbladeFractal,
            66 => Encounters.Fractal.UrbanBattlegroundFractal,
            67 => Encounters.Fractal.DeepstoneFractal,
            68 => Encounters.Fractal.CliffsideFractal,
            69 => Encounters.Fractal.MoltenBossFractal,
            70 => Encounters.Fractal.AetherbladeFractal,
            71 => Encounters.Fractal.CaptainMaiTrinBossFractal,
            72 => Encounters.Fractal.NightmareFractal,
            73 => Encounters.Fractal.ShatteredObservatoryFractal,
            74 => Encounters.Fractal.SunquaPeakFractal,
            75 => Encounters.Fractal.SilentSurfFractal,
            76 => Encounters.Fractal.AquaticRuinsFractal,
            77 => Encounters.Fractal.SwamplandFractal,
            78 => Encounters.Fractal.SirensReefFractal,
            79 => Encounters.Fractal.UncategorizedFractal,
            80 => Encounters.Fractal.SolidOceanFractal,
            81 => Encounters.Fractal.UndergroundFacilityFractal,
            82 => Encounters.Fractal.ThaumanovaReactorFractal,
            83 => Encounters.Fractal.MoltenFurnaceFractal,
            84 => Encounters.Fractal.DeepstoneFractal,
            85 => Encounters.Fractal.UrbanBattlegroundFractal,
            86 => Encounters.Fractal.SnowblindFractal,
            87 => Encounters.Fractal.TwilightOasisFractal,
            88 => Encounters.Fractal.ChaosFractal,
            89 => Encounters.Fractal.SwamplandFractal,
            90 => Encounters.Fractal.MoltenBossFractal,
            91 => Encounters.Fractal.UncategorizedFractal,
            92 => Encounters.Fractal.VolcanicFractal,
            93 => Encounters.Fractal.SnowblindFractal,
            94 => Encounters.Fractal.CliffsideFractal,
            95 => Encounters.Fractal.CaptainMaiTrinBossFractal,
            96 => Encounters.Fractal.AetherbladeFractal,
            97 => Encounters.Fractal.NightmareFractal,
            98 => Encounters.Fractal.ShatteredObservatoryFractal,
            99 => Encounters.Fractal.SunquaPeakFractal,
            100 => Encounters.Fractal.SilentSurfFractal,
            _ => throw new ArgumentOutOfRangeException("fractal for scale", scale, null)
        };
    }

}