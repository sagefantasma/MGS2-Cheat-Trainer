using System.Collections.Generic;
using System.ComponentModel;

namespace MGS2_CheatTrainer_V2.Models
{
    public class BossVitals
    {
        public List<int>? NestedHealthPointers;
        public List<int>? NestedStaminaPointers;
        public int HealthOffset;
        public int Health;
        public bool HasStamina;
        public int StaminaOffset;
        public int Stamina;
        public Constants.Boss Boss;

        public static BossVitals ParseBossVitals(Constants.Boss boss)
        {
            switch(boss)
            {
                case Constants.Boss.Olga:
                    return Mgs2Boss.Olga;
                case Constants.Boss.Fortune:
                    return Mgs2Boss.Fortune;
                case Constants.Boss.Fatman:
                    return Mgs2Boss.Fatman;
                case Constants.Boss.Harrier:
                    return Mgs2Boss.Harrier;
                case Constants.Boss.Vamp:
                    return Mgs2Boss.Vamp;
                case Constants.Boss.VampSnipe:
                    return Mgs2Boss.VampSniping;
                case Constants.Boss.Solidus:
                    return Mgs2Boss.Solidus;
                case Constants.Boss.Ray1:
                    return Mgs2Boss.Ray1;
                case Constants.Boss.Ray2:
                    return Mgs2Boss.Ray2;
                case Constants.Boss.Ray3:
                    return Mgs2Boss.Ray3;
                case Constants.Boss.Ray4:
                    return Mgs2Boss.Ray4;
                case Constants.Boss.Ray5:
                    return Mgs2Boss.Ray5;
                case Constants.Boss.Ray6:
                    return Mgs2Boss.Ray6;
                case Constants.Boss.Ray7:
                    return Mgs2Boss.Ray7;
                case Constants.Boss.Ray8:
                    return Mgs2Boss.Ray8;
                case Constants.Boss.Ray9:
                    return Mgs2Boss.Ray9;
                case Constants.Boss.Ray10:
                    return Mgs2Boss.Ray10;
                case Constants.Boss.Ray11:
                    return Mgs2Boss.Ray11;
                case Constants.Boss.Ray12:
                    return Mgs2Boss.Ray12;
                case Constants.Boss.Ray13:
                    return Mgs2Boss.Ray13;
                case Constants.Boss.Ray14:
                    return Mgs2Boss.Ray14;
                case Constants.Boss.Ray15:
                    return Mgs2Boss.Ray15;
                case Constants.Boss.Ray16:
                    return Mgs2Boss.Ray16;
                case Constants.Boss.Ray17:
                    return Mgs2Boss.Ray17;
                case Constants.Boss.Ray18:
                    return Mgs2Boss.Ray18;
                case Constants.Boss.Ray19:
                    return Mgs2Boss.Ray19;
                case Constants.Boss.Ray20:
                    return Mgs2Boss.Ray20;
                case Constants.Boss.Ray21:
                    return Mgs2Boss.Ray21;
                case Constants.Boss.Ray22:
                    return Mgs2Boss.Ray22;
                case Constants.Boss.Ray23:
                    return Mgs2Boss.Ray23;
                case Constants.Boss.Ray24:
                    return Mgs2Boss.Ray24;
                case Constants.Boss.Ray25:
                    return Mgs2Boss.Ray25;
                default:
                    throw new InvalidEnumArgumentException("Boss not recognized.");
            }
        }
    }

    public class Mgs2Boss
    {
        public static readonly BossVitals Olga = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.OlgaNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.OlgaNestedPointers, HealthOffset = 0x46A, StaminaOffset = 0x46C,
            Boss = Constants.Boss.Olga
        };
        public static readonly BossVitals Fortune = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.FortuneNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.FortuneNestedPointers,HealthOffset = Mgs2Offset.FortuneHpValue.Start, 
            StaminaOffset = Mgs2Offset.FortuneStaminaValue.Start,
            Boss = Constants.Boss.Fortune
        };
        public static readonly BossVitals Fatman = new BossVitals { HasStamina = true, 
            NestedHealthPointers = Mgs2Pointer.FatmanNestedPointers, NestedStaminaPointers = Mgs2Pointer.FatmanNestedPointers,
            HealthOffset = 0x41E,
            StaminaOffset = 0x2330,
            Boss = Constants.Boss.Fatman
        };
        public static readonly BossVitals Harrier = new BossVitals { HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.HarrierNestedPointers, //it works for tracking, but crashes the game when we modify it KEKW
            HealthOffset = 0x78,
            Boss = Constants.Boss.Harrier
        };
        public static readonly BossVitals Vamp = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.VampNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.VampNestedPointers, 
            HealthOffset = 0xFD0,
            StaminaOffset = 0xFD2,
            Boss = Constants.Boss.Vamp
        };
        public static readonly BossVitals VampSniping = new BossVitals { HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.VampSnipingNestedPointers,
            NestedStaminaPointers = Mgs2Pointer.VampSnipingNestedPointers,
            HealthOffset = 0x22A,
            StaminaOffset = 0x119A,
            Boss = Constants.Boss.VampSnipe
        };
        #region Rays
        public static readonly BossVitals Ray1 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray1NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray1
        };
        public static readonly BossVitals Ray2 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray2NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray2
        };
        public static readonly BossVitals Ray3 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray3NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray3
        };
        public static readonly BossVitals Ray4 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray4NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray4
        };
        public static readonly BossVitals Ray5 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray5NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray5
        };
        public static readonly BossVitals Ray6 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray6NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray6
        };
        public static readonly BossVitals Ray7 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray7NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray7
        };
        public static readonly BossVitals Ray8 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray8NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray8
        };
        public static readonly BossVitals Ray9 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray9NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray9
        };
        public static readonly BossVitals Ray10 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray10NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray10
        };
        public static readonly BossVitals Ray11 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray11NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray11
        };
        public static readonly BossVitals Ray12 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray12NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray12
        };
        public static readonly BossVitals Ray13 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray13NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray13
        };
        public static readonly BossVitals Ray14 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray14NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray14
        };
        public static readonly BossVitals Ray15 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray15NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray15
        };
        public static readonly BossVitals Ray16 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray16NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray16
        };
        public static readonly BossVitals Ray17 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray17NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray17
        };
        public static readonly BossVitals Ray18 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray18NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray18
        };
        public static readonly BossVitals Ray19 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray19NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray19
        };
        public static readonly BossVitals Ray20 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray20NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray20
        };
        public static readonly BossVitals Ray21 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray21NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray21
        };
        public static readonly BossVitals Ray22 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray22NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray22
        };
        public static readonly BossVitals Ray23 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray23NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray23
        };
        public static readonly BossVitals Ray24 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray24NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray24
        };
        public static readonly BossVitals Ray25 = new BossVitals
        {
            HasStamina = false,
            NestedHealthPointers = Mgs2Pointer.Ray25NestedPointers,
            HealthOffset = 0x800,
            Boss = Constants.Boss.Ray25
        };
        #endregion
        public static readonly BossVitals Solidus = new BossVitals
        {
            HasStamina = true,
            NestedHealthPointers = Mgs2Pointer.SolidusNestedPointers, //these crash the game, sadge
            NestedStaminaPointers = Mgs2Pointer.SolidusNestedPointers, //these crash the game, sadge
            HealthOffset = 0x348,
            StaminaOffset = 0x358,
            Boss = Constants.Boss.Solidus
        };
    }
}
