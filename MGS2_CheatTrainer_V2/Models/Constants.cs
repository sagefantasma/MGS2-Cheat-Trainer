using System;
using System.Collections.Generic;

namespace MGS2_CheatTrainer_V2.Models
{
    public static class Constants
    {
        //REWRITE STATUS: Done?
        public const string Mgs2ProcessName = "METAL GEAR SOLID2";
        internal const string SteamAppId = "2131640";
        internal const string SteamAppIdFileName = "steam_appid.txt";
        public const int MillisecondsInSecond = 1000;
        private const int ItemMinMaxCountDiff = 96;
        private const int WeaponMinMaxCountDiff = 72;

        public enum PlayableCharacter
        {
            Snake,
            Raiden,
            Mgs1Snake,
            TuxedoSnake,
            Pliskin,
            NinjaRaiden,
            NakedRaiden
        }

        public enum Boss
        {
            Olga,
            Fortune,
            Fatman,
            Harrier,
            Vamp,
            VampSnipe,
            Ray1, Ray2, Ray3, Ray4, Ray5, Ray6, Ray7, Ray8, Ray9, Ray10, Ray11, Ray12, Ray13, Ray14, Ray15, Ray16, Ray17, Ray18, Ray19, Ray20, Ray21, Ray22, Ray23, Ray24, Ray25,
            Solidus
        }

        public enum Cheat
        {
            NoBleedDamage, NoBurnDamage, InfiniteAmmo, InfiniteLife, InfiniteOxygen, NoGripDamage, 
            EmmaInfiniteHealth, EmmaInfiniteO2, NoClipWithGravity, NoClipNoGravity,  //Emma health is crashing the game and i cba to fix it
            NoReload, ZoomIn, ZoomOut, DisablePauseButton, //zoom in and out aren't working as expected, and i cant be bothered to fix them right now.
            DisableItemMenuPause, DisableWeaponMenuPause, InfiniteItems, InfiniteKnockout, RemovePlantFilter,
            RemovePlantFog, RemoveTankerFilter, NightTime, MaxStackOnPickup, PauseVrTimer, VrObjectiveAutoComplete,
            VrEnemiesAutoComplete, VrNoHitDamage, VrNoFallDamage, VrInfiniteStrength, VrGripDamage, VrAimStability, //VR Enemies autocomplete is crashing the game
            VrInfiniteAmmo, VrInfiniteItem, VrNoReload, BlackScreen, Letterboxing, GhostMode, TurnOffMusic
        }
        
        public interface IMgs2Object
        {
            public string Name { get; set; }
            public string Shorthand { get; set; }
            public int Index { get; set; }
        }

        public class Item(string name, string shorthand, int index) : IMgs2Object
        {
            public string Name { get; set; } = name;
            public string Shorthand { get; set; } = shorthand;
            public int Index { get; set; } = index;
        }

        public class SpecialItem(string name, string shorthand, int index) : Item(name, shorthand, index)
        {
        }

        public class MaxableItem(string name, string shorthand, int index) : Item(name, shorthand, index)
        {
            public int MaxIndex { get; set; } = index + ItemMinMaxCountDiff;
        }

        public class BooleanItem(string name, string shorthand, int index) : Item(name, shorthand, index)
        {
        }
    
        public class Weapon(string name, string shorthand, int index) : IMgs2Object
        {
            public string Name { get; set; } = name;
            public string Shorthand { get; set; } = shorthand;
            public int Index { get; set; } = index;
        }
        
        public class MaxableWeapon(string name, string shorthand, int index) : Weapon(name, shorthand, index)
        {
            public int MaxIndex { get; set; } = index + WeaponMinMaxCountDiff;
        }

        public class BooleanWeapon(string name, string shorthand, int index) : Weapon(name, shorthand, index)
        {
        }

        public static IMgs2Object DetermineObject(string input)
        {
            try
            {
                string viewName = input.ToLower();
            
                return ItemList.Find(x=> viewName.Contains($"{x.Shorthand}detailview", StringComparison.InvariantCultureIgnoreCase)) ??
                       WeaponList.Find(x => viewName.Contains($"{x.Shorthand}detailview", StringComparison.InvariantCultureIgnoreCase)) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                throw new AggregateException($"{input} is an unknown object", ex);
            }
        }

        #region Item Table

        public static List<IMgs2Object> ItemList =
        [
            new MaxableItem("Ration", "ration", 0), //1 - C2
            new BooleanItem("Scope1", "scope1", 2), //2 - C3
            new MaxableItem("Cold Medicine", "coldmeds", 4), //3 - C4
            new MaxableItem("Bandage", "bandage", 6), //4 - C5
            new MaxableItem("Pentazemin", "pentazemin", 8), //5 - C6
            new BooleanItem("B.D.U.", "bdu", 10), //6 - C7
            new BooleanItem("Body Armor", "bodyarmor", 12), //7 - C8
            new BooleanItem("Stealth", "stealth", 14), //8 - C9
            new BooleanItem("Mine Detector", "minedetector", 16), //9 - CA
            new BooleanItem("Sensor A", "sensora", 18), //10 - CB
            new BooleanItem("Sensor B", "sensorb", 20), //11 - CC
            new BooleanItem("N.V.G.", "nvg", 22), //12 - CD
            new BooleanItem("Thermal Goggles", "thermals", 24), //13 - CE
            new BooleanItem("Scope2", "scope2", 26), //14 - CF
            new BooleanItem("Digital Camera", "digitalcamera", 28), //15 - D0
            new SpecialItem("Box 1", "box1", 30), //16 - D1
            new BooleanItem("Cigarettes", "cigs", 32), //17 - D2
            new SpecialItem("Card", "card", 34), //18 - D3
            new BooleanItem("Shaver", "shaver", 36), //19 - D4
            new BooleanItem("Phone", "phone", 38), //20 - D5
            new BooleanItem("Camera", "camera", 40), //21 - D6
            new SpecialItem("Box 2", "box2", 42), //22 - D7
            new SpecialItem("Box 3", "box3", 44), //23 - D8
            new SpecialItem("Wet Box", "wetbox", 46), //24 - D9
            new BooleanItem("A.P. Sensor", "apsensor", 48), //25 - DA
            new SpecialItem("Box 4", "box4", 50), //26 - DB
            new SpecialItem("Box 5", "box5", 52), //27 - DC
            new BooleanItem("Unknown Item", "unknownitem", 54), //28 razor? - DD
            new BooleanItem("SOCOM Suppressor", "socomsupp", 56), //29 - DE
            new BooleanItem("AK Suppressor", "aksupp", 58), //30 - DF
            new BooleanItem("Cutscene Camera", "cutscenecamera", 60), //31 - E0 cutscene camera, like Dmic has a special cutscene version
            new BooleanItem("Bandana", "bandana", 62), //32 - E1
            new SpecialItem("Dog Tags", "dogtags", 64), //33 - E2
            new BooleanItem("M.O. Disc", "modisc", 66), //34 - E3
            new BooleanItem("U.S.P. Suppressor", "uspsupp", 68), //35 - E4
            new BooleanItem("Infinity Wig", "infinitywig", 70), //36 - E5
            new BooleanItem("Blue Wig", "bluewig", 72), //37 - E6
            new BooleanItem("Orange Wig", "orangewig", 74), //38 - E7
            new BooleanItem("Color Wig 1", "colorwig1", 76), //39 unused item - E8
            new BooleanItem("Color Wig 2", "colorwig2", 78), //40 unused item - E9
        ];
        #endregion

        #region Weapon Table
        public static List<IMgs2Object> WeaponList = [
        new MaxableWeapon("M9", "m9", 0), //1 - C2
        new MaxableWeapon("U.S.P.", "usp", 2), //2 - C3
        new MaxableWeapon("SOCOM", "socom", 4), //3 - C4
        new MaxableWeapon("PSG1", "psg1", 6), //4 - C5
        new MaxableWeapon("RGB-6", "rgb6", 8), //5 - C6
        new MaxableWeapon("Nikita", "nikita", 10), //6 - C7
        new MaxableWeapon("Stinger", "stinger", 12), //7 - C8
        new MaxableWeapon("Claymore", "claymore", 14), //8 - C9
        new MaxableWeapon("C4", "c4", 16), //9 - CA
        new MaxableWeapon("Chaff Grenade", "chaff", 18), //10 - CB
        new MaxableWeapon("Stun Grenade", "stun", 20), //11 - CC
        new BooleanWeapon("Directional Microphone", "dmic", 22), //12 - CD
        new BooleanWeapon("High Frequency Blade", "hfblade", 24), //13 - CE
        new BooleanWeapon("Coolant Spray", "coolant", 26), //14 - CF
        new MaxableWeapon("AKS-74u", "aks74u", 28), //15 - D0
        new MaxableWeapon("Magazine", "magazine", 30), //16 - D1
        new MaxableWeapon("Grenade", "grenade", 32), //17 - D2
        new MaxableWeapon("M4", "m4", 34), //18 - D3
        new MaxableWeapon("PSG1-T", "psg1t", 36), //19 - D4
        new BooleanWeapon("Directional Microphone(Movie)", "dmiczoomed", 38), //20 - D5
        new MaxableWeapon("Book", "book", 40), //21 - D6
        ];
        #endregion
    };
}
