using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    public class Stage
    {
        public string Name;
        public string AreaCode;

        public override string ToString()
        {
            return $"Area: {Name} -- Code: {AreaCode}";
        }

        public static Stage Parse(string s)
        {
            if(StageNames.MenuStages.StageList.Any(stageCode => s == stageCode.AreaCode))
            {
                return StageNames.MenuStages.StageList.First(stageCode => s == stageCode.AreaCode);
            }

            if (StageNames.TankerStages.PlayableStageList.Any(stageCode => s == stageCode.AreaCode))
            {
                return StageNames.TankerStages.PlayableStageList.First(stageCode => s == stageCode.AreaCode);
            }

            if (StageNames.PlantStages.PlayableStageList.Any(stageCode => s == stageCode.AreaCode))
            {
                return StageNames.PlantStages.PlayableStageList.First(stageCode => s == stageCode.AreaCode);
            }

            if (StageNames.VRStages.PlayableStageList.Any(stageCode => s == stageCode.AreaCode))
            {
                return StageNames.VRStages.PlayableStageList.First(stageCode => s == stageCode.AreaCode);
            }

            return null;
        }
    }

    public static class StageNames
    {
        public static class MenuStages
        {
            public static readonly Stage DevMenu = new Stage { Name = "DevMenu", AreaCode = "select" };
            public static readonly Stage MainMenu = new Stage { Name = "MainMenu", AreaCode = "n_title" };
            public static readonly Stage VRMenu = new Stage { Name = "VRMenu", AreaCode = "mselect" };
            public static readonly Stage SnakeTalesMenu = new Stage { Name = "SnakeTalesMenu", AreaCode = "tales" };

            public static readonly List<Stage> StageList = new List<Stage> { DevMenu, MainMenu, VRMenu, SnakeTalesMenu };
        }

        public static class TankerStages
        {
            #region Playable Stages
            public static readonly Stage AltDeck = new Stage { Name = "AltDeck", AreaCode = "w00a" };
            public static readonly Stage OlgaFight = new Stage { Name = "OlgaFight", AreaCode = "w00b" };
            public static readonly Stage NavigationalDeck = new Stage { Name = "NavigationalDeck", AreaCode = "w00c" };
            public static readonly Stage DeckACrewQuarters = new Stage { Name = "DeckACrewQuarters", AreaCode = "w01a" };
            public static readonly Stage DeckACrewQuartersStarboard = new Stage { Name = "DeckACrewQuartersStarboard", AreaCode = "w01b" };
            public static readonly Stage DeckCCrewQuarters = new Stage { Name = "DeckCCrewQuarters", AreaCode = "w01c" };
            public static readonly Stage DeckDCrewQuarters = new Stage { Name = "DeckDCrewQuarters", AreaCode = "w01d" };
            public static readonly Stage DeckEBridge = new Stage { Name = "DeckEBridge", AreaCode = "w01e" };
            public static readonly Stage DeckACrewLounge = new Stage { Name = "DeckACrewLounge", AreaCode = "w01f" };
            public static readonly Stage EngineRoom = new Stage { Name = "EngineRoom", AreaCode = "w02a" };
            public static readonly Stage Deck2Port = new Stage { Name = "Deck2Port", AreaCode = "w03a" };
            public static readonly Stage Deck2Starboard = new Stage { Name = "Deck2Starboard", AreaCode = "w03b" };
            public static readonly Stage Hold1 = new Stage { Name = "Hold1", AreaCode = "w04a" };
            public static readonly Stage Hold2 = new Stage { Name = "Hold2", AreaCode = "w04b" };
            public static readonly Stage Hold3 = new Stage { Name = "Hold3", AreaCode = "w04c" };

            public static readonly List<Stage> PlayableStageList = new List<Stage> { AltDeck, OlgaFight, NavigationalDeck, DeckACrewQuarters, DeckACrewQuartersStarboard, 
                DeckACrewLounge, DeckCCrewQuarters, DeckDCrewQuarters, DeckEBridge, EngineRoom, Deck2Port, Deck2Starboard, Hold1, Hold2, Hold3 };
            #endregion

            #region Cutscenes
            public static readonly string TankerOpening = "d00t";
            public static readonly string RussianInvasion = "d01t";
            public static readonly string IdentifyingChoppers = "d04t";
            public static readonly string OlgaCutscenes = "d05t";
            public static readonly string Cutscene05 = "d10t";
            public static readonly string Cutscene06 = "d11t";
            public static readonly string Cutscene07 = "d12t";
            public static readonly string Cutscene08 = "d12t3";
            public static readonly string Cutscene09 = "d12t4";
            public static readonly string Cutscene10 = "d12t4"; //is this accurate?
            public static readonly string Cutscene11 = "d13t";
            public static readonly string Cutscene12 = "d14t";

            public static readonly List<string> CutsceneList = new List<string> { TankerOpening, RussianInvasion, IdentifyingChoppers, OlgaCutscenes, Cutscene05, Cutscene06,
            Cutscene07, Cutscene08, Cutscene09, Cutscene10, Cutscene11, Cutscene12};
            #endregion
        }

        public static class PlantStages
        {
            #region Playable Stages
            public static readonly Stage SeaDock = new Stage { Name = "SeaDock", AreaCode = "w11a" };
            public static readonly Stage SeaDockBombDisposal = new Stage { Name = "SeaDockBombDisposal", AreaCode = "w11b" };
            public static readonly Stage SeaDockFortune = new Stage { Name = "SeaDockFortune", AreaCode = "w11c" };
            public static readonly Stage StrutARoof = new Stage { Name = "StrutARoof", AreaCode = "w12a" };
            public static readonly Stage StrutARoofBomb = new Stage { Name = "StrutARoofBomb", AreaCode = "w12c" };
            public static readonly Stage StrutAPumpRoom = new Stage { Name = "StrutAPumpRoom", AreaCode = "w12b" };
            public static readonly Stage ABConnectingBridge = new Stage { Name = "ABConnectingBridge", AreaCode = "w13a" };
            public static readonly Stage ABConnectingBridgeSensorB = new Stage { Name = "ABConnectingBridgeSensorB", AreaCode = "w13b" };
            public static readonly Stage TransformerRoom = new Stage { Name = "TransformerRoom", AreaCode = "w14a" };
            public static readonly Stage BCConnectingBridge = new Stage { Name = "BCConnectingBridge", AreaCode = "w15a" };
            public static readonly Stage BCConnectingBridgeAfterStillman = new Stage { Name = "BCConnectingBridgeAfterStillman", AreaCode = "w15b" };
            public static readonly Stage DiningHall = new Stage { Name = "DiningHall", AreaCode = "w16a" };
            public static readonly Stage DiningHallAfterStillman = new Stage { Name = "DiningHallAfterStillman", AreaCode = "w16b" };
            public static readonly Stage CDConnectingBridge = new Stage { Name = "CDConnectingBridge", AreaCode = "w17a" };
            public static readonly Stage SedimentPool = new Stage { Name = "SedimentPool", AreaCode = "w18a" };
            public static readonly Stage DEConnectingBridge = new Stage { Name = "DEConnectingBridge", AreaCode = "w19a" };
            public static readonly Stage ParcelRoom = new Stage { Name = "ParcelRoom", AreaCode = "w20a" };
            public static readonly Stage Heliport = new Stage { Name = "Heliport", AreaCode = "w20b" };
            public static readonly Stage HeliportBomb = new Stage { Name = "HeliportBomb", AreaCode = "w20c" };
            public static readonly Stage HeliportPostNinja = new Stage { Name = "HeliportPostNinja", AreaCode = "w20d" };
            public static readonly Stage EFConnectingBridge = new Stage { Name = "EFConnectingBridge", AreaCode = "w21a" };
            public static readonly Stage Warehouse = new Stage { Name = "Warehouse", AreaCode = "w22a" };
            public static readonly Stage FAConnectingBridge = new Stage { Name = "FAConnectingBridge", AreaCode = "w23b" };
            public static readonly Stage Shell1Core = new Stage { Name = "Shell1Core", AreaCode = "w24a" };
            public static readonly Stage Shell1CoreB1 = new Stage { Name = "Shell1CoreB1", AreaCode = "w24b" };
            public static readonly Stage Shell1CoreB2 = new Stage { Name = "Shell1CoreB2", AreaCode = "w24d" };
            public static readonly Stage Shell1CoreHostageRoom = new Stage { Name = "Shell1CoreHostageRoom", AreaCode = "w24c" };
            public static readonly Stage ShellsConnectingBridge = new Stage { Name = "ShellsConnectingBridge", AreaCode = "w25a" };
            public static readonly Stage ShellsConnectingBridgeDestroyed = new Stage { Name = "ShellsConnectingBridgeDestroyed", AreaCode = "w25b" };
            public static readonly Stage StrutLPerimeter = new Stage { Name = "StrutLPerimeter", AreaCode = "w25c" };
            public static readonly Stage KLConnectingBridge = new Stage { Name = "KLConnectingBridge", AreaCode = "w25d" };
            public static readonly Stage SewageTreatment = new Stage { Name = "SewageTreatment", AreaCode = "w28a" };
            public static readonly Stage Shell2Core = new Stage { Name = "Shell2Core", AreaCode = "w31a" };
            public static readonly Stage Shell2FiltrationChamber1 = new Stage { Name = "Shell2FiltrationChamber1", AreaCode = "w31b" };
            public static readonly Stage Shell2FiltrationChamber2 = new Stage { Name = "Shell2FiltrationChamber2", AreaCode = "w31c" };
            public static readonly Stage Shell2CoreWithEmma = new Stage { Name = "Shell2CoreWithEmma", AreaCode = "w31d" };
            public static readonly Stage OilFence = new Stage { Name = "OilFence", AreaCode = "w32a" };
            public static readonly Stage OilFenceVamp = new Stage { Name = "OilFenceVamp", AreaCode = "w32b" };
            public static readonly Stage Stomach = new Stage { Name = "Stomach", AreaCode = "w41a" };
            public static readonly Stage Jujenum = new Stage { Name = "Jujenum", AreaCode = "w42a" };
            public static readonly Stage AscendingColon = new Stage { Name = "AscendingColon", AreaCode = "w43a" };
            public static readonly Stage Ileum = new Stage { Name = "Ileum", AreaCode = "w44a" };
            public static readonly Stage SigmoidColon = new Stage { Name = "SigmoidColon", AreaCode = "w45a" };
            public static readonly Stage Rectum = new Stage { Name = "Rectum", AreaCode = "w46a" };
            public static readonly Stage ArsenalGear = new Stage { Name = "ArsenalGear", AreaCode = "w51a" };
            public static readonly Stage FederalHall = new Stage { Name = "FederalHall", AreaCode = "w61a" };

            public static readonly List<Stage> PlayableStageList = new List<Stage> { SeaDock, SeaDockBombDisposal, SeaDockFortune, StrutARoof, StrutARoofBomb, StrutAPumpRoom,
                ABConnectingBridge, ABConnectingBridgeSensorB, TransformerRoom, BCConnectingBridge, BCConnectingBridgeAfterStillman, DiningHall, DiningHallAfterStillman,
                CDConnectingBridge, SedimentPool, DEConnectingBridge, ParcelRoom, Heliport, HeliportBomb, HeliportPostNinja, EFConnectingBridge, Warehouse, FAConnectingBridge,
                Shell1Core, Shell1CoreB1, Shell1CoreB2, Shell1CoreHostageRoom, ShellsConnectingBridge, ShellsConnectingBridgeDestroyed, StrutLPerimeter, KLConnectingBridge,
                SewageTreatment, Shell2Core, Shell2FiltrationChamber1, Shell2FiltrationChamber2, Shell2CoreWithEmma, OilFence, OilFenceVamp, Stomach, Jujenum,
                AscendingColon, Ileum, SigmoidColon, Rectum, ArsenalGear, FederalHall};
            #endregion

            #region Cutscenes
            public static readonly string PlantBriefing = "museum";
            public static readonly string Website = "webdemo";
            public static readonly string RankScreen = "ending";
            public static readonly string PlantOpening = "d001p01";
            public static readonly string SeaDockCutscene = "d001p02";
            public static readonly string RaidenOnElevator = "d005p01";
            public static readonly string StrutARoofCutscene = "d005p03";
            public static readonly string MeetingVamp = "d010p01";
            public static readonly string ADUD = "d012p01";
            public static readonly string StillmanCutscene = "d014p01";
            public static readonly string FatmanAndNinja = "d021p01";
            public static readonly string HostageCutscene = "d036p03";
            public static readonly string Shell1Cutscene = "d036p05";
            public static readonly string Cutscene14 = "d045p01";
            public static readonly string Cutscene15 = "d046p01";
            public static readonly string Cutscene16 = "d053p01";
            public static readonly string Cutscene17 = "d055p01";
            public static readonly string Cutscene18 = "d063p01";
            public static readonly string Cutscene19 = "d065p02";
            public static readonly string Cutscene20 = "d070p01";
            public static readonly string Cutscene21 = "d070p09";
            public static readonly string Cutscene22 = "d070px9";
            public static readonly string Cutscene23 = "d078p01";
            public static readonly string Cutscene24 = "d080p01";
            public static readonly string Cutscene25 = "d080p06";
            public static readonly string Cutscene26 = "d080p07";
            public static readonly string Cutscene27 = "d080p08";
            public static readonly string Cutscene28 = "d082p01";
            #endregion
        }

        public static class VRStages
        {
            #region Alternate Missions & SnakeTales
            public static readonly Stage AltDeck = new Stage { Name = "AlternateAltDeck", AreaCode = "a00a" };
            public static readonly Stage NavDeck = new Stage { Name = "AlternateNavDeck", AreaCode = "a00b" };
            public static readonly Stage NavDeckUnused = new Stage { Name = "AlternateNavDeckUnused", AreaCode = "a00c" };
            public static readonly Stage DeckACrewQuarters = new Stage { Name = "AlternateDeckACrewQuarters", AreaCode = "a01a" };
            public static readonly Stage DeckACrewQuartersStarboard = new Stage { Name = "AlternateDeckACrewQuartersStarboard", AreaCode = "a01b" };
            public static readonly Stage DeckCCrewQuarters = new Stage { Name = "AlternateDeckCCrewQuarters", AreaCode = "a01c" };
            public static readonly Stage DeckDCrewQuarters = new Stage { Name = "AlternateDeckDCrewQuarters", AreaCode = "a01d" };
            public static readonly Stage DeckEBridge = new Stage { Name = "AlternateDeckEBridge", AreaCode = "a01e" };
            public static readonly Stage DeckACrewLounge = new Stage { Name = "AlternateDeckACrewLounge", AreaCode = "a01f" };
            public static readonly Stage EngineRoom = new Stage { Name = "AlternateEngineRoom", AreaCode = "a02a" };
            public static readonly Stage Deck2Port = new Stage { Name = "AlternateDeck2Port", AreaCode = "a03a" };
            public static readonly Stage Deck2Starboard = new Stage { Name = "AlternateDeck2Starboard", AreaCode = "a03b" };
            public static readonly Stage Hold1 = new Stage { Name = "AlternateHold1", AreaCode = "a04a" };
            public static readonly Stage Hold2 = new Stage { Name = "AlternateHold2", AreaCode = "a04b" };
            public static readonly Stage Hold3 = new Stage { Name = "AlternateHold3", AreaCode = "a04c" };
            public static readonly Stage SeaDock = new Stage { Name = "AlternateSeaDock", AreaCode = "a11a" };
            public static readonly Stage SeaDockBomb = new Stage { Name = "AlternateSeaDockBomb", AreaCode = "a11b" };
            public static readonly Stage SeaDockFortune = new Stage { Name = "AlternateSeaDockFortune", AreaCode = "a11c" };
            public static readonly Stage StrutARoof = new Stage { Name = "AlternateStrutARoof", AreaCode = "a12a" };
            public static readonly Stage StrutARoofBomb = new Stage { Name = "AlternateStrutARoofBomb", AreaCode = "a12c" };
            public static readonly Stage PumpRoom = new Stage { Name = "AlternatePumpRoom", AreaCode = "a12b" };
            public static readonly Stage ABConnectingBridge = new Stage { Name = "AlternateABConnectingBridge", AreaCode = "a13a" };
            public static readonly Stage ABConnectingBridgeSensorB = new Stage { Name = "AlternateABConnectingBridgeSensorB", AreaCode = "a13b" };
            public static readonly Stage TransformerRoom = new Stage { Name = "AlternateTransformerRoom", AreaCode = "a14a" };
            public static readonly Stage BCConnectingBridge = new Stage { Name = "AlternateBCConnectingBridge", AreaCode = "a15a" };
            public static readonly Stage BCConnectingAfterStillman = new Stage { Name = "AlternateBCConnectingAfterStillman", AreaCode = "a15b" };
            public static readonly Stage DiningHall = new Stage { Name = "AlternateDiningHall", AreaCode = "a16a" };
            public static readonly Stage DiningHallAfterCutscene = new Stage { Name = "AlternateDiningHallAfterCutscene", AreaCode = "a16b" };
            public static readonly Stage CDConnectingBridge = new Stage { Name = "AlternateCDConnectingBridge", AreaCode = "a17a" };
            public static readonly Stage SedimentPool = new Stage { Name = "AlternateSedimentPool", AreaCode = "a18a" };
            public static readonly Stage DEConnectingBridge = new Stage { Name = "AlternateDEConnectingBridge", AreaCode = "a19a" };
            public static readonly Stage ParcelRoom = new Stage { Name = "AlternateParcelRoom", AreaCode = "a20a" };
            public static readonly Stage Heliport = new Stage { Name = "AlternateHeliport", AreaCode = "a20b" };
            public static readonly Stage HeliportBomb = new Stage { Name = "AlternateHeliportBomb", AreaCode = "a20c" };
            public static readonly Stage HeliportNinja = new Stage { Name = "AlternateHeliportNinja", AreaCode = "a20d" };
            public static readonly Stage EFConnectingBridge = new Stage { Name = "AlternateEFConnectingBridge", AreaCode = "a21a" };
            public static readonly Stage Warehouse = new Stage { Name = "AlternateWarehouse", AreaCode = "a22a" };
            public static readonly Stage FAConnectingBridge = new Stage { Name = "AlternateFAConnectingBridge", AreaCode = "a23b" };
            public static readonly Stage Shell1 = new Stage { Name = "AlternateShell1", AreaCode = "a24a" };
            public static readonly Stage Shell1B1 = new Stage { Name = "AlternateShell1B1", AreaCode = "a24b" };
            public static readonly Stage Shell1B2 = new Stage { Name = "AlternateShell1B2", AreaCode = "a24d" };
            public static readonly Stage Shell1HostageRoom = new Stage { Name = "AlternateShell1HostageRoom", AreaCode = "a24c" };
            public static readonly Stage Shell12ConnectingBridge = new Stage { Name = "AlternateShell12ConnectingBridge", AreaCode = "a25a" };
            public static readonly Stage Shell12ConnectingBridgeDestroyed = new Stage { Name = "AlternateShell12ConnectingBridgeDestroyed", AreaCode = "a25b" };
            public static readonly Stage StrutLPerimeter = new Stage { Name = "AlternateStrutLPerimeter", AreaCode = "a25c" };
            public static readonly Stage KLConnectingBridge = new Stage { Name = "AlternateKLConnectingBridge", AreaCode = "a25d" };
            public static readonly Stage SewageTreatment = new Stage { Name = "AlternateSewageTreatment", AreaCode = "a28a" };
            public static readonly Stage Shell2 = new Stage { Name = "AlternateShell2", AreaCode = "a31a" };
            public static readonly Stage Shell2B1 = new Stage { Name = "AlternateShell2B1", AreaCode = "a31b" };
            public static readonly Stage Shell2B1VampFight = new Stage { Name = "AlternateShell2B1VampFight", AreaCode = "a31c" };
            public static readonly Stage Shell2WithEmma = new Stage { Name = "AlternateShell2WithEmma", AreaCode = "a31d" };
            public static readonly Stage OilFence = new Stage { Name = "AlternateOilFence", AreaCode = "a32a" };
            public static readonly Stage OilFenceVamp = new Stage { Name = "AlternateOilFenceVamp", AreaCode = "a32b" };
            public static readonly Stage Stomach = new Stage { Name = "AlternateStomach", AreaCode = "a41a" };
            public static readonly Stage Jujenum = new Stage { Name = "AlternateJujenum", AreaCode = "a42a" };
            public static readonly Stage AscendingColon = new Stage { Name = "AlternateAscendingColon", AreaCode = "a43a" };
            public static readonly Stage Ileum = new Stage { Name = "AlternateIleum", AreaCode = "a44a" };
            public static readonly Stage SigmoidColon = new Stage { Name = "AlternateSigmoidColon", AreaCode = "a45a" };
            public static readonly Stage Rectum = new Stage { Name = "AlternateRectum", AreaCode = "a46a" };
            public static readonly Stage ArsenalGear = new Stage { Name = "AlternateArsenalGear", AreaCode = "a51a" };
            public static readonly Stage FederalHall = new Stage { Name = "AlternateFederalHall", AreaCode = "a61a" };
            #endregion
            
            #region Sneaking/Eliminate All Missions
            public static readonly Stage Sneaking01 = new Stage { Name = "Sneaking01", AreaCode = "vs01a" };
            public static readonly Stage Sneaking02 = new Stage { Name = "Sneaking02", AreaCode = "vs02a" };
            public static readonly Stage Sneaking03 = new Stage { Name = "Sneaking03", AreaCode = "vs03a" };
            public static readonly Stage Sneaking04 = new Stage { Name = "Sneaking04", AreaCode = "vs04a" };
            public static readonly Stage Sneaking05 = new Stage { Name = "Sneaking05", AreaCode = "vs05a" };
            public static readonly Stage Sneaking06 = new Stage { Name = "Sneaking06", AreaCode = "vs06a" };
            public static readonly Stage Sneaking07 = new Stage { Name = "Sneaking07", AreaCode = "vs07a" };
            public static readonly Stage Sneaking08 = new Stage { Name = "Sneaking08", AreaCode = "vs08a" };
            public static readonly Stage Sneaking09 = new Stage { Name = "Sneaking09", AreaCode = "vs09a" };
            public static readonly Stage Sneaking10 = new Stage { Name = "Sneaking10", AreaCode = "vs10a" };
            #endregion

            #region Variety Missions
            public static readonly Stage Variety01 = new Stage { Name = "Variety01", AreaCode = "sp01a" };
            public static readonly Stage Variety02 = new Stage { Name = "Variety02", AreaCode = "sp02a" };
            public static readonly Stage Variety03 = new Stage { Name = "Variety03", AreaCode = "sp03a" } ;
            public static readonly Stage Variety04 = new Stage { Name = "Variety04", AreaCode = "sp04a" };
            public static readonly Stage Variety05 = new Stage { Name = "Variety05", AreaCode = "sp05a" };
            public static readonly Stage Variety06 = new Stage { Name = "Variety06", AreaCode = "sp06a" };
            public static readonly Stage Variety07 = new Stage { Name = "Variety07", AreaCode = "sp07a" };
            public static readonly Stage Variety08 = new Stage { Name = "Variety08", AreaCode = "sp08a" };
            #endregion

            #region Streaking Missions
            public static readonly Stage Streaking01 = new Stage { Name = "Streaking01", AreaCode = "st02a" };
            public static readonly Stage Streaking02 = new Stage { Name = "Streaking02", AreaCode = "st03a" };
            public static readonly Stage Streaking03 = new Stage { Name = "Streaking03", AreaCode = "st04a" };
            public static readonly Stage Streaking04 = new Stage { Name = "Streaking04", AreaCode = "st05a" };
            #endregion

            #region First Person Missions
            public static readonly Stage FirstPerson01 = new Stage { Name = "FirstPerson01", AreaCode = "sp21" };
            public static readonly Stage FirstPerson02 = new Stage { Name = "FirstPerson02", AreaCode = "sp22" };
            public static readonly Stage FirstPerson03 = new Stage { Name = "FirstPerson03", AreaCode = "sp23" };
            public static readonly Stage FirstPerson04 = new Stage { Name = "FirstPerson04", AreaCode = "sp24" };
            public static readonly Stage FirstPerson05 = new Stage { Name = "FirstPerson05", AreaCode = "sp25" };
            #endregion

            #region Weapons Missions
            public static readonly Stage Socom01 = new Stage { Name = "Socom01", AreaCode = "wp01a" };
            public static readonly Stage Socom02 = new Stage { Name = "Socom02", AreaCode = "wp02a" };
            public static readonly Stage Socom03 = new Stage { Name = "Socom03", AreaCode = "wp03a" };
            public static readonly Stage Socom04 = new Stage { Name = "Socom04", AreaCode = "wp04a" };
            public static readonly Stage Socom05 = new Stage { Name = "Socom05", AreaCode = "wp05a" };
            public static readonly Stage M401 = new Stage { Name = "M401", AreaCode = "wp11a" };
            public static readonly Stage M402 = new Stage { Name = "M402", AreaCode = "wp12a" };
            public static readonly Stage M403 = new Stage { Name = "M403", AreaCode = "wp13a" };
            public static readonly Stage M404 = new Stage { Name = "M404", AreaCode = "wp14a" };
            public static readonly Stage M405 = new Stage { Name = "M405", AreaCode = "wp15a" };
            public static readonly Stage C401 = new Stage { Name = "C401", AreaCode = "wp21a" };
            public static readonly Stage C402 = new Stage { Name = "C402", AreaCode = "wp22a" };
            public static readonly Stage C403 = new Stage { Name = "C403", AreaCode = "wp23a" };
            public static readonly Stage C404 = new Stage { Name = "C404", AreaCode = "wp24a" };
            public static readonly Stage C405 = new Stage { Name = "C405", AreaCode = "wp25a" };
            public static readonly Stage Grenade01 = new Stage { Name = "Grenade01", AreaCode = "wp31a" };
            public static readonly Stage Grenade02 = new Stage { Name = "Grenade02", AreaCode = "wp32a" };
            public static readonly Stage Grenade03 = new Stage { Name = "Grenade03", AreaCode = "wp33a" };
            public static readonly Stage Grenade04 = new Stage { Name = "Grenade04", AreaCode = "wp34a" };
            public static readonly Stage Grenade05 = new Stage { Name = "Grenade05", AreaCode = "wp35a" };
            public static readonly Stage PSG101 = new Stage { Name = "PSG101", AreaCode = "wp41a" };
            public static readonly Stage PSG102 = new Stage { Name = "PSG102", AreaCode = "wp42a" };
            public static readonly Stage PSG103 = new Stage { Name = "PSG103", AreaCode = "wp43a" };
            public static readonly Stage PSG104 = new Stage { Name = "PSG104", AreaCode = "wp44a" };
            public static readonly Stage PSG105 = new Stage { Name = "PSG105", AreaCode = "wp45a" };
            public static readonly Stage Stinger01 = new Stage { Name = "Stinger01", AreaCode = "wp51a" };
            public static readonly Stage Stinger02 = new Stage { Name = "Stinger02", AreaCode = "wp52a" };
            public static readonly Stage Stinger03 = new Stage { Name = "Stinger03", AreaCode = "wp53a" };
            public static readonly Stage Stinger04 = new Stage { Name = "Stinger04", AreaCode = "wp54a" };
            public static readonly Stage Stinger05 = new Stage { Name = "Stinger05", AreaCode = "wp55a" };
            public static readonly Stage Nikita01 = new Stage { Name = "Nikita01", AreaCode = "wp61a" };
            public static readonly Stage Nikita02 = new Stage { Name = "Nikita02", AreaCode = "wp62a" };
            public static readonly Stage Nikita03 = new Stage { Name = "Nikita03", AreaCode = "wp63a" };
            public static readonly Stage Nikita04 = new Stage { Name = "Nikita04", AreaCode = "wp64a" };
            public static readonly Stage Nikita05 = new Stage { Name = "Nikita05", AreaCode = "wp65a" };
            public static readonly Stage NoWeapon01 = new Stage { Name = "NoWeapon01", AreaCode = "wp71a" };
            public static readonly Stage NoWeapon02 = new Stage { Name = "NoWeapon02", AreaCode = "wp72a" };
            public static readonly Stage NoWeapon03 = new Stage { Name = "NoWeapon03", AreaCode = "wp73a" };
            public static readonly Stage NoWeapon04 = new Stage { Name = "NoWeapon04", AreaCode = "wp74a" };
            public static readonly Stage NoWeapon05 = new Stage { Name = "NoWeapon05", AreaCode = "wp75a" };
            #endregion

            public static readonly List<Stage> PlayableStageList = new List<Stage> { AltDeck, NavDeck, NavDeckUnused, DeckACrewQuarters, DeckACrewQuartersStarboard,
                DeckACrewLounge, DeckCCrewQuarters, DeckDCrewQuarters, DeckEBridge, EngineRoom, Deck2Port, Deck2Starboard, Hold1, Hold2, Hold3, SeaDock, SeaDockBomb,
                SeaDockFortune, StrutARoof, StrutARoofBomb, PumpRoom, ABConnectingBridge, ABConnectingBridgeSensorB, TransformerRoom, BCConnectingBridge,
                BCConnectingAfterStillman, DiningHall, DiningHallAfterCutscene, CDConnectingBridge, SedimentPool, DEConnectingBridge, ParcelRoom, Heliport,
                HeliportBomb, HeliportNinja, EFConnectingBridge, Warehouse, FAConnectingBridge, Shell1, Shell1B1, Shell1B2, Shell1HostageRoom, Shell12ConnectingBridge,
                Shell12ConnectingBridgeDestroyed, StrutLPerimeter, KLConnectingBridge, SewageTreatment, Shell2, Shell2B1, Shell2B1VampFight, Shell2WithEmma, OilFence,
                OilFenceVamp, Stomach, Jujenum, AscendingColon, Ileum, SigmoidColon, Rectum, ArsenalGear, FederalHall, Sneaking01, Sneaking02, Sneaking03, Sneaking04,
                Sneaking05, Sneaking06, Sneaking07, Sneaking08, Sneaking09, Sneaking10, Variety01, Variety02, Variety03, Variety04, Variety05, Variety06, Variety07,
                Variety08, Streaking01, Streaking02, Streaking03, Streaking04, FirstPerson01, FirstPerson02, FirstPerson03, FirstPerson04, FirstPerson05, Socom01,
                Socom02, Socom03, Socom04, Socom05, M401, M402, M403, M404, M405, C401, C402, C403, C404, C405, Grenade01, Grenade02, Grenade03, Grenade04, Grenade05,
                PSG101, PSG102, PSG103, PSG104, PSG105, Stinger01, Stinger02, Stinger03, Stinger04, Stinger05, Nikita01, Nikita02, Nikita03, Nikita04, Nikita05,
                NoWeapon01, NoWeapon02, NoWeapon03, NoWeapon04, NoWeapon05};
            public static readonly List<string> PrefixesList = new List<string> { "a1", "a2", "a3", "a4", "a5", "a6", "vs", "sp", "st", "wp"};
        }
    }
}
