using System.Collections.Generic;
using System.Linq;

namespace MGS2_CheatTrainer_V2
{
    internal abstract class Snake : PlayableCharacter
    {
        public new static readonly List<Constants.IMgs2Object> UsableObjects =
        [
            Constants.ItemList.First(x => "aksupp".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "apsensor".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "bandage".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "bandana".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "camera1".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "camera2".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "card".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "chaff".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "cigs".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "claymore".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "coldmeds".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "digitalcamera".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "dmic".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "dmiczoomed".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "dogtags".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "grenade".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "m9".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "magazine".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "minedetector".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "modisc".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "pentazemin".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "phone".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "scope1".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "scope2".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "ration".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "sensora".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "sensorb".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "shaver".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "socom".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "socomsupp".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "stealth".Equals(x.Shorthand)),
            Constants.WeaponList.First(x => "stun".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "thermals".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "uspsupp".Equals(x.Shorthand)),
            Constants.ItemList.First(x => "wetbox".Equals(x.Shorthand))
        ];
    }
}
