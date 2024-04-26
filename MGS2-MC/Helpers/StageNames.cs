using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    public static class StageNames
    {
        public static class MenuStages
        {
            public static readonly string DevMenu = "select";
            public static readonly string MainMenu = "n_title";
            public static readonly string VRMenu = "mselect";
            public static readonly string SnakeTalesMenu = "tales";

            public static readonly List<string> StageList = new List<string> { DevMenu, MainMenu, VRMenu, SnakeTalesMenu };
        }

        public static class TankerStages
        {
            #region Playable Stages
            public static readonly string AltDeck = "w00a";
            public static readonly string OlgaFight = "w00b";
            public static readonly string NavigationalDeck = "w00c";
            public static readonly string DeckACrewQuarters = "w01a";
            public static readonly string DeckACrewQuartersStarboard = "w01b";
            public static readonly string DeckCCrewQuarters = "w01c";
            public static readonly string DeckDCrewQuarters = "w01d";
            public static readonly string DeckEBridge = "w01e";
            public static readonly string DeckACrewLounge = "w01f";
            public static readonly string EngineRoom = "w02a";
            public static readonly string Deck2Port = "w03a";
            public static readonly string Deck2Starboard = "w03b";
            public static readonly string Hold1 = "w04a";
            public static readonly string Hold2 = "w04b";
            public static readonly string Hold3 = "w04c";

            public static readonly List<string> PlayableStageList = new List<string> { AltDeck, OlgaFight, NavigationalDeck, DeckACrewQuarters, DeckACrewQuartersStarboard, 
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
            public static readonly string SeaDock = "w11a";
            public static readonly string SeaDockBombDisposal = "w11b";
            public static readonly string SeaDockFortune = "w11c";
            public static readonly string StrutARoof = "w12a";
            public static readonly string StrutARoofBomb = "w12c";
            public static readonly string StrutAPumpRoom = "w12b";
            public static readonly string ABConnectingBridge = "w13a";
            public static readonly string ABConnectingBridgeSensorB = "w13b";
            public static readonly string TransformerRoom = "w14a";
            public static readonly string BCConnectingBridge = "w15a";
            public static readonly string BCConnectingBridgeAfterStillman = "w15b";
            public static readonly string DiningHall = "w16a";
            public static readonly string DiningHallAfterStillman = "w16b";
            public static readonly string CDConnectingBridge = "w17a";
            public static readonly string SedimentPool = "w18a";
            public static readonly string DEConnectingBridge = "w19a";
            public static readonly string ParcelRoom = "w20a";
            public static readonly string Heliport = "w20b";
            public static readonly string HeliportBomb = "w20c";
            public static readonly string HeliportPostNinja = "w20d";
            public static readonly string EFConnectingBridge = "w21a";
            public static readonly string Warehouse = "w22a";
            public static readonly string FAConnectingBridge = "w23b";
            public static readonly string Shell1Core = "w24a";
            public static readonly string Shell1CoreB1 = "w24b";
            public static readonly string Shell1CoreB2 = "w24d";
            public static readonly string Shell1CoreHostageRoom = "w24c";
            public static readonly string ShellsConnectingBridge = "w25a";
            public static readonly string ShellsConnectingBridgeDestroyed = "w25b";
            public static readonly string StrutLPerimeter = "w25c";
            public static readonly string KLConnectingBridge = "w25d";
            public static readonly string SewageTreatment = "w28a";
            public static readonly string Shell2Core = "w31a";
            public static readonly string Shell2FiltrationChamber1 = "w31b";
            public static readonly string Shell2FiltrationChamber2 = "w31c";
            public static readonly string Shell2CoreWithEmma = "w31d";
            public static readonly string OilFence = "w32a";
            public static readonly string OilFenceVamp = "w32b";
            public static readonly string Stomach = "w41a";
            public static readonly string Jujenum = "w42a";
            public static readonly string AscendingColon = "w43a";
            public static readonly string Ileum = "w44a";
            public static readonly string SigmoidColon = "w45a";
            public static readonly string Rectum = "w46a";
            public static readonly string ArsenalGear = "w51a";
            public static readonly string FederalHall = "w61a";
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
            public static readonly string AltDec = "a00a";
            public static readonly string NavDeck = "a00b";
            public static readonly string NavDeckUnused = "a00c";
            public static readonly string DeckACrewQuarters = "a01a";
            public static readonly string DeckACrewQuartersStarboard = "a01b";
            public static readonly string DeckCCrewQuarters = "a01c";
            public static readonly string DeckDCrewQuarters = "a01d";
            public static readonly string DeckEBridge = "a01e";
            public static readonly string DeckACrewLounge = "a01f";
            public static readonly string EngineRoom = "a02a";
            public static readonly string Deck2Port = "a03a";
            public static readonly string Deck2Starboard = "a03b";
            public static readonly string Hold1 = "a04a";
            public static readonly string Hold2 = "a04b";
            public static readonly string Hold3 = "a04c";
            public static readonly string SeaDock = "a11a";
            public static readonly string SeaDockBomb = "a11b";
            public static readonly string SeaDockFortune = "a11c";
            public static readonly string StrutARoof = "a12a";
            public static readonly string StrutARoofBomb = "a12c";
            public static readonly string PumpRoom = "a12b";
            public static readonly string ABConnectingBridge = "a13a";
            public static readonly string ABConnectingBridgeSensorB = "a13b";
            public static readonly string TransformerRoom = "a14a";
            public static readonly string BCConnectingBridge = "a15a";
            public static readonly string BCConnectingAfterStillman = "a15b";
            public static readonly List<string> MissionList = new List<string> { };
            public static readonly List<string> PrefixesList = new List<string> { "a1", "a2", "a3", "a4", "a5", "a6", "vs", "sp", "st", "wp"};
            /*
             * VR MISSIONS (Alternate/Snake Tales)
a11a	Strut A Sea Dock
a11b	Strut A Sea Dock (Bomb disposal)
a11c	Strut A Sea Dock (Fortune Fight)
a12a	Strut A Roof
a12c	Strut A Roof (Last Bomb)
a12b	Strut A Pump room
a13a	AB Connecting bridge
a13b	AB Connecting bridge (With Sensor B)
a14a	Strut B Transformer Room
a15a	BC Connecting bridge
a15b	BC Connecting bridge (After Stillman cutscene)
a16a	Strut C Dining Hall
a16b	Strut C Dining Hall (after 'd014p01')
a17a	CD Connecting bridge
a18a	Strut D Sediment Pool
a19a	DE Connecting bridge
a20a	Strut E Parcel room, 1F
a20b	Strut E Heliport
a20c	Strut E Heliport (last bomb)
a20d	Strut E Heliport (After ninja cutscene)
a21a	EF Connecting bridge
a22a	Strut F Warehouse
a23b	FA Connecting bridge
a24a	Shell 1 Core
a24b	Shell 1 Core B1
a24d	Shell 1 Core B2,Computer's Room
a24c	Shell 1 B1 Hall,Hostages Room
a25a	Shell 1,2 Connecting Bridge
a25b	Shell 1,2 Connecting Bridge (Destroyed)
a25c	Strut L perimeter
a25d	KL Connecting Bridge
a28a	Strut L Sewage Treatment Facility
a31a	Shell 2 Core,1F Air Purification Room
a31b	Shell 2 Core,B1 Filtration Chamber NO1
a31c	Shell 2 Core,B1 Filtration Chamber NO2 (Vamp Fight)
a31d	Shell 2 Core,1F Air Purification Room (w/emma)
a32a	Strut L Oil Fence
a32b	Strut L Oil Fence (Vamp Fight)
a41a	Arsenal Gear-Stomach
a42a	Arsenal Gear-Jujenum
a43a	Arsenal Gear-Ascending Colon
a44a	Arsenal Gear-Ileum
a45a	Arsenal Gear-Sigmoid Colon
a46a	Arsenal Gear-Rectum
a51a	Arsenal Gear (After Ray Battle)
a61a	Federal Hall
VR MISSIONS(Sneaking/Eliminate All)
vs01a	Stage One
vs02a	Stage Two
vs03a	Stage Three
vs04a	Stage Four
vs05a	Stage Five
vs06A	Stage Six
vs07a	Stage Seven
vs08a	Stage Eight
vs09A	Stage Nine
vs10A	Stage Ten
VR MISSIONS(Variety)
sp01a	Stage One
sp02a	Stage Two
sp03a	Stage Three
sp04a	Stage Four
sp05a	Stage Five
sp06a	Stage Six
sp07a	Stage Seven
sp08a	Stage Eight
VR MISSIONS(Streaking Mode)
st02a	Stage One
st03a	Stage Two
st04a	Stage Three
st05a	Stage Four
VR MISSIONS(First Person Mode)
sp21	Stage One
sp22	Stage Two
sp23	Stage Three
sp24	Stage Four
sp25	Stage Five
VR MISSIONS(Weapons Mode)
wp01a	Stage One(USP/SOCOM)
wp02a	Stage Two(USP/SOCOM)
wp03a	Stage Three(USP/SOCOM)
wp04a	Stage Four(USP/SOCOM)
wp05a	Stage Five(USP/SOCOM)
wp11a	Stage One(M4/AK74U)
wp12a	Stage Two(M4/AK74U)
wp13a	Stage Three(M4/AK74U)
wp14a	Stage Four(M4/AK74U)
wp15a	Stage Five(M4/AK74U)
wp21a	Stage One(C4/CLAYMORE)
wp22a	Stage Two(C4/CLAYMORE)
wp23a	Stage Three(C4/CLAYMORE)
wp24a	Stage Four(C4/CLAYMORE)
wp25a	Stage Five(C4/CLAYMORE)
wp31a	Stage One(GRENADE)
wp32a	Stage Two(GRENADE)
wp33a	Stage Three(GRENADE)
wp34a	Stage Four(GRENADE)
wp35a	Stage Five(GRENADE)
wp41a	Stage One(PSG-1)
wp42a	Stage Two(PSG-1)
wp43a	Stage Three(PSG-1)
wp44a	Stage Four(PSG-1)
wp45a	Stage Five(PSG-1)
wp51a	Stage One(STINGER)
wp52a	Stage Two(STINGER)
wp53a	Stage Three(STINGER)
wp54a	Stage Four(STINGER)
wp55a	Stage Five(STINGER)
wp61a	Stage One(NIKITA)
wp62a	Stage Two(NIKITA)
wp63a	Stage Three(NIKITA)
wp64a	Stage Four(NIKITA)
wp65a	Stage Five(NIKITA)
wp71a	Stage One(NO WEAPON/HF.BLADE)
wp72a	Stage Two(NO WEAPON/HF.BLADE)
wp73a	Stage Three(NO WEAPON/HF.BLADE)
wp74a	Stage Four(NO WEAPON/HF.BLADE)
wp75a	Stage Five(NO WEAPON/HF.BLADE)
             */
        }
    }
}
