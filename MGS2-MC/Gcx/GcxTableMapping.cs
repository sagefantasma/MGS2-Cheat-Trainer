using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal class GcxTableMapping
    {
        #region MGS2 Items
        //20 natively spawnable items
        public static readonly byte Ration = 0xC1; //can be spawned
        public static readonly byte Scope1 = 0xC2;
        public static readonly byte ColdMeds = 0xC3; //can be spawned
        public static readonly byte Bandage = 0xC4; //can be spawned
        public static readonly byte Pentazemin = 0xC5; //can be spawned
        public static readonly byte BDU = 0xC6;
        public static readonly byte BodyArmor = 0xC7; //can be spawned
        public static readonly byte Stealth = 0xC8;
        public static readonly byte MineDetector = 0xC9; //can be spawned
        public static readonly byte SensorA = 0xCA;
        public static readonly byte SensorB = 0xCB; //can be spawned
        public static readonly byte NVG = 0xCC; //can be spawned
        public static readonly byte Thermals = 0xCD; //can be spawned
        public static readonly byte Scope2 = 0xCE;
        public static readonly byte DigitalCamera = 0xCF; //can be spawned
        public static readonly byte Box1 = 0xD0; //can be spawned
        public static readonly byte Cigs = 0xD1;
        public static readonly byte Card = 0xD2;
        public static readonly byte Shaver = 0xD3; //can be spawned
        public static readonly byte Phone = 0xD4;
        public static readonly byte Camera1 = 0xD5;
        public static readonly byte Box2 = 0xD6; //can be spawned
        public static readonly byte Box3 = 0xD7; //can be spawned
        public static readonly byte WetBox = 0xD8; //can be spawned
        public static readonly byte APSensor = 0xD9;
        public static readonly byte Box4 = 0xDA; //can be spawned
        public static readonly byte Box5 = 0xDB; //can be spawned
        public static readonly byte Unknown = 0xDC;
        public static readonly byte SocomSupp = 0xDD; //can be spawned
        public static readonly byte AkSupp = 0xDE; //can be spawned
        public static readonly byte Camera2 = 0xDF;
        public static readonly byte Bandana = 0xE0;
        public static readonly byte DogTags = 0xE1; //can be spawned
        public static readonly byte MoDisc = 0xE2;
        public static readonly byte UspSupp = 0xE3; //can be spawned
        public static readonly byte InfWig = 0xE4;
        public static readonly byte BlueWig = 0xE5;
        public static readonly byte OrangeWig = 0xE6;
        public static readonly byte ColorWig1 = 0xE7;
        public static readonly byte ColorWig2 = 0xE8;
        #endregion

        #region MGS2 Weapons
        //17 natively spawnable ammo boxes
        //8 natively spawnable guns
        public static readonly byte M9 = 0xC1; //can be spawned(gun & ammo)
        public static readonly byte Usp = 0xC2; //can be spawned(ammo only)
        public static readonly byte Socom = 0xC3; //can be spawned(ammo only)
        public static readonly byte Psg1 = 0xC4; //can be spawned(gun & ammo)
        public static readonly byte Rgb6 = 0xC5; //can be spawned(gun & ammo)
        public static readonly byte Nikita = 0xC6; //can be spawned(gun & ammo)
        public static readonly byte Stinger = 0xC7; //can be spawned(gun & ammo)
        public static readonly byte Claymore = 0xC8; //can be spawned
        public static readonly byte C4 = 0xC9; //can be spawned
        public static readonly byte Chaff = 0xCA; //can be spawned
        public static readonly byte Stun = 0xCB; //can be spawned
        public static readonly byte Dmic1 = 0xCC; //can be spawned
        public static readonly byte HfBlade = 0xCD;
        public static readonly byte Coolant = 0xCE;
        public static readonly byte Aks74u = 0xCF; //can be spawned(gun & ammo)
        public static readonly byte Magazine = 0xD0;
        public static readonly byte Grenade = 0xD1; //can be spawned
        public static readonly byte M4 = 0xD2; //can be spawned(gun & ammo)
        public static readonly byte Psg1t = 0xD3; //can be spawned(gun & ammo)
        public static readonly byte Dmic2 = 0xD4;
        public static readonly byte Book = 0xD5; //can be spawned
        #endregion
    }
}
