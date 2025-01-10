namespace MGS2_MC
{
    public class Constants
    {
        public const string MGS2_PROCESS_NAME = "METAL GEAR SOLID2";
        internal const string SteamAppId = "2131640";
        internal const string SteamAppIdFileName = "steam_appid.txt";
        public const int MillisecondsInSecond = 1000;

        public enum PlayableCharacter
        {
            Snake,
            Raiden,
            MGS1Snake,
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

        #region Item Table
        public const int RATION = 0; //1 - C2
        public const int BROKEN_SCOPE = 2; //2 - C3 verify
        public const int COLD_MEDICINE = 4; //3 - C4
        public const int BANDAGE = 6; //4 - C5
        public const int PENTAZEMIN = 8; //5 - C6
        public const int BDU = 10; //6 - C7
        public const int BODY_ARMOR = 12; //7 - C8
        public const int STEALTH = 14; //8 - C9
        public const int MINE_DETECTOR = 16; //9 - CA
        public const int SENSOR_A = 18; //10 - CB
        public const int SENSOR_B = 20; //11 - CC
        public const int NVG = 22; //12 - CD
        public const int THERMAL_GOGGLES = 24; //13 - CE
        public const int SCOPE = 26; //14 verify - CF
        public const int DIGITAL_CAMERA = 28; //15 - D0
        public const int BOX_1 = 30; //16 - D1
        public const int CIGARETTES = 32; //17 - D2
        public const int CARD = 34; //18 - D3
        public const int SHAVER = 36; //19 - D4
        public const int PHONE = 38; //20 - D5
        public const int CAMERA = 40; //21 - D6
        public const int BOX_2 = 42; //22 - D7
        public const int BOX_3 = 44; //23 - D8
        public const int WET_BOX = 46; //24 - D9
        public const int AP_SENSOR = 48; //25 - DA
        public const int BOX_4 = 50; //26 - DB
        public const int BOX_5 = 52; //27 - DC
        public const int UNKNOWN_ITEM = 54; //28 razor? - DD
        public const int SOCOM_SUPPRESSOR = 56; //29 - DE
        public const int AK_SUPPRESSOR = 58; //30 - DF
        public const int BROKEN_CAMERA = 60; //31 - E0 cutscene camera, like Dmic has a special cutscene version
        public const int BANDANA = 62; //32 - E1
        public const int DOG_TAGS = 64; //33 - E2
        public const int MO_DISC = 66; //34 - E3
        public const int USP_SUPPRESSOR = 68; //35 - E4
        public const int INFINITY_WIG = 70; //36 - E5
        public const int BLUE_WIG = 72; //37 - E6
        public const int ORANGE_WIG = 74; //38 - E7
        public const int COLOR_WIG_1 = 76; //39 unused item - E8
        public const int COLOR_WIG_2 = 78; //40 unused item - E9
        #endregion

        #region Weapon Table
        public const int M9 = 0; //1 - C2
        public const int USP = 2; //2 - C3
        public const int SOCOM = 4; //3 - C4
        public const int PSG1 = 6; //4 - C5
        public const int RGB6 = 8; //5 - C6
        public const int NIKITA = 10; //6 - C7
        public const int STINGER = 12; //7 - C8
        public const int CLAYMORE = 14; //8 - C9
        public const int C4 = 16; //9 - CA
        public const int CHAFF_GRENADE = 18; //10 - CB
        public const int STUN_GRENADE = 20; //11 - CC
        public const int D_MIC = 22; //12 - CD
        public const int HIGH_FREQUENCY_BLADE = 24; //13 - CE
        public const int COOLANT = 26; //14 - CF
        public const int AKS74U = 28; //15 - D0
        public const int MAGAZINE = 30; //16 - D1
        public const int GRENADE = 32; //17 - D2
        public const int M4 = 34; //18 - D3
        public const int PSG1T = 36; //19 - D4
        public const int D_MIC_ZOOMED = 38; //20 - D5
        public const int BOOK = 40; //21 - D6
        #endregion
    };
}
