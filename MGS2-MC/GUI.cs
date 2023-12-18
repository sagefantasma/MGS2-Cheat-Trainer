using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
            //removing the stats & strings pages for now since they're unfinished
            mgs2TabControl.TabPages.RemoveByKey(tabPageStats.Name);
            mgs2TabControl.TabPages.RemoveByKey(tabPageStrings.Name);
        }

        #region GUI getters
        private short CurrentRationValue()
        {
            return (short)rationCurrentUpDown.Value;
        }

        private short MaxRationValue()
        {
            return (short)rationMaxUpDown.Value;
        }

        private short CurrentBandageValue()
        {
            return (short)bandageCurrentUpDown.Value;
        }

        private short MaxBandageValue()
        {
            return (short)bandageMaxUpDown.Value;
        }

        private short CurrentPentazeminCount()
        {
            return (short)pentazeminCurrentUpDown.Value;
        }

        private short MaxPentazeminCount()
        {
            return (short)pentazeminMaxUpDown.Value;
        }

        private short CurrentDogTagCount()
        {
            return (short)dogTagsCurrentUpDown.Value;
        }

        private short MaxDogTagCount()
        {
            return (short)dogTagsMaxUpDown.Value;
        }

        private short CurrentColdMedsCount()
        {
            return (short)coldMedsCurrentUpDown.Value;
        }

        private short MaxColdMedsCount()
        {
            return (short)coldMedsMaxUpDown.Value;
        }

        private short CardSecurityLevel()
        {
            return (short)cardUpDown.Value;
        }

        private short M9CurrentAmmoCount()
        {
            return (short)m9CurrentUpDown.Value;
        }

        private short M9MaxAmmoCount()
        {
            return (short)m9MaxUpDown.Value;
        }

        private short USPCurrentAmmoCount()
        {
            return (short)uspCurrentUpDown.Value;
        }

        private short USPMaxAmmoCount()
        {
            return (short)uspMaxUpDown.Value;
        }

        private short SOCOMCurrentAmmoCount()
        {
            return (short)socomCurrentUpDown.Value;
        }

        private short SOCOMMaxAmmoCount()
        {
            return (short)socomMaxUpDown.Value;
        }

        private short PSG1CurrentAmmoCount()
        {
            return (short)psg1CurrentUpDown.Value;
        }

        private short PSG1MaxAmmoCount()
        {
            return (short)psg1MaxUpDown.Value;
        }

        private short RGB6CurrentAmmoCount()
        {
            return (short)rgb6CurrentUpDown.Value;
        }

        private short RGB6MaxAmmoCount()
        {
            return (short)rgb6MaxUpDown.Value;
        }

        private short NikitaCurrentAmmoCount()
        {
            return (short)nikitaCurrentUpDown.Value;
        }

        private short NikitaMaxAmmoCount()
        {
            return (short)nikitaMaxUpDown.Value;
        }

        private short StingerCurrentAmmoCount()
        {
            return (short)stingerCurrentUpDown.Value;
        }

        private short StingerMaxAmmoCount()
        {
            return (short)stingerMaxUpDown.Value;
        }

        private short C4CurrentAmmoCount()
        {
            return (short)c4CurrentUpDown.Value;
        }

        private short C4MaxAmmoCount()
        {
            return (short)c4MaxUpDown.Value;
        }

        private short AKCurrentAmmoCount()
        {
            return (short)akCurrentUpDown.Value;
        }

        private short AKMaxAmmoCount()
        {
            return (short)akMaxUpDown.Value;
        }

        private short M4CurrentAmmoCount()
        {
            return (short)m4CurrentUpDown.Value;
        }

        private short M4MaxAmmoCount()
        {
            return (short)m4MaxUpDown.Value;
        }

        private short PSG1TCurrentAmmoCount()
        {
            return (short)psg1TCurrentUpDown.Value;
        }

        private short PSG1TMaxAmmoCount()
        {
            return (short)psg1TMaxUpDown.Value;
        }

        private short ChaffCurrentAmmoCount()
        {
            return (short)chaffCurrentUpDown.Value;
        }

        private short ChaffMaxAmmoCount()
        {
            return (short)chaffMaxUpDown.Value;
        }

        private short StunCurrentAmmoCount()
        {
            return (short)stunCurrentUpDown.Value;
        }

        private short StunMaxAmmoCount()
        {
            return (short)stunMaxUpDown.Value;
        }

        private short GrenadeCurrentAmmoCount()
        {
            return (short)grenadeCurrentUpDown.Value;
        }

        private short GrenadeMaxAmmoCount()
        {
            return (short)grenadeMaxUpDown.Value;
        }

        private short BookCurrentAmmoCount()
        {
            return (short)bookCurrentUpDown.Value;
        }

        private short BookMaxAmmoCount()
        {
            return (short)bookMaxUpDown.Value;
        }

        private short MagazineCurrentAmmoCount()
        {
            return (short)magazineCurrentUpDown.Value;
        }

        private short MagazineMaxAmmoCount()
        {
            return (short)magazineMaxUpDown.Value;
        }

        private short ClaymoreCurrentAmmoCount()
        {
            return (short)claymoreCurrentUpDown.Value;
        }

        private short ClaymoreMaxAmmoCount()
        {
            return (short)claymoreMaxUpDown.Value;
        }

        private bool DMic1Enabled()
        {
            return dmic1CheckBox.Checked;
        }

        private bool DMic2Enabled()
        {
            return dmic2CheckBox.Checked;
        }

        private bool CoolantEnabled()
        {
            return coolantCheckBox.Checked;
        }

        private bool HfBladeEnabled()
        {
            return hfBladeCheckBox.Checked;
        }
        #endregion

        #region Items Button Functions
        private void RationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.ToggleItem(rationCheckBox.Checked);
        }

        private void RationCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateCurrentCount(CurrentRationValue());
        }

        private void RationMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Ration.UpdateMaxCount(MaxRationValue());
        }

        private void BandageCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.ToggleItem(bandageCheckBox.Checked);
        }

        private void BandageCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateCurrentCount(CurrentBandageValue());
        }

        private void BandageMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandage.UpdateMaxCount(MaxBandageValue());
        }

        private void PentazeminCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.ToggleItem(pentazeminCheckBox.Checked);
        }

        private void PentazeminCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateCurrentCount(CurrentPentazeminCount());
        }

        private void PentazeminMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Pentazemin.UpdateMaxCount(MaxPentazeminCount());
        }

        private void CardBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.SetLevel(CardSecurityLevel());
        }

        private void CardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Card.ToggleItem(cardCheckBox.Checked);
        }

        private void Binos1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SnakeScope.ToggleItem(scope1CheckBox.Checked);
        }

        private void Binos2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RaidenScope.ToggleItem(scope2CheckBox.Checked);
        }

        private void Camera1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Camera1.ToggleItem(camera1CheckBox.Checked);
        }

        private void Camera2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Camera2.ToggleItem(camera2CheckBox.Checked);
        }

        private void NvgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.NightVisionGoggles.ToggleItem(nvgCheckBox.Checked);
        }

        private void ThermalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ThermalGoggles.ToggleItem(thermalCheckBox.Checked);
        }

        private void BodyArmorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BodyArmor.ToggleItem(bodyArmorCheckBox.Checked);
        }

        private void MineDetectorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MineDetector.ToggleItem(mineDetectorCheckBox.Checked);
        }

        private void ApSensorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.APSensor.ToggleItem(apSensorCheckBox.Checked);
        }

        private void SensorACheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorA.ToggleItem(sensorACheckBox.Checked);
        }

        private void SensorBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SensorB.ToggleItem(sensorBCheckBox.Checked);
        }

        private void PhoneCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Phone.ToggleItem(phoneCheckBox.Checked);
        }

        private void ColdMedsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ColdMedicine.ToggleItem(coldMedsCheckBox.Checked);
        }

        private void CigarettesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Cigarettes.ToggleItem(cigarettesCheckBox.Checked);
        }

        private void MoDiscCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.MODisc.ToggleItem(moDiscCheckBox.Checked);
        }

        private void SocomSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SocomSuppressor.ToggleItem(socomSupCheckBox.Checked);
        }

        private void UspSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USPSuppressor.ToggleItem(uspSupCheckBox.Checked);
        }

        private void AkSupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKSuppressor.ToggleItem(akSupCheckBox.Checked);
        }

        private void Box1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box1.ToggleItem(box1CheckBox.Checked);
        }

        private void Box2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.ToggleItem(box2CheckBox.Checked);
        }

        private void Box3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.ToggleItem(box3CheckBox.Checked);
        }

        private void Box4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.ToggleItem(box4CheckBox.Checked);
        }

        private void Box5CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.ToggleItem(box5CheckBox.Checked);
        }

        private void WetBoxCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.ToggleItem(wetBoxCheckBox.Checked);
        }

        private void BduCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BDU.ToggleItem(bduCheckBox.Checked);
        }

        private void BduMaskCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            //MGS2UsableObjects.BDU.SetDurability(2);
        }

        private void BandanaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Bandana.ToggleItem(bandanaCheckBox.Checked);
        }

        private void InfinityWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.InfinityWig.ToggleItem(infinityWigCheckBox.Checked);
        }

        private void BlueWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.BlueWig.ToggleItem(blueWigCheckBox.Checked);
        }

        private void OrangeWigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.OrangeWig.ToggleItem(orangeWigCheckBox.Checked);
        }

        private void StealthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stealth.ToggleItem(stealthCheckBox.Checked);
        }

        private void DogTagsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DogTags.ToggleItem(dogTagsCheckBox.Checked);
        }

        private void ShaverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Shaver.ToggleItem(shaverCheckBox.Checked);
        }

        private void ColdMedsCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ColdMedicine.UpdateCurrentCount(CurrentColdMedsCount());
        }

        private void ColdMedsMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ColdMedicine.UpdateMaxCount(MaxColdMedsCount());
        }

        private void box1Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box1.SetDurability((short)box1UpDown.Value);
        }

        private void box2Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box2.SetDurability((short)box2UpDown.Value);
        }

        private void box3Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box3.SetDurability((short)box3UpDown.Value);
        }

        private void box4Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box4.SetDurability((short)box4UpDown.Value);
        }

        private void box5Btn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Box5.SetDurability((short)box5UpDown.Value);
        }

        private void wetBoxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.WetBox.SetDurability((short)wetBoxUpDown.Value);
        }
        #endregion

        #region Weapons Button Functions
        private void AkMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateMaxAmmoCount(AKMaxAmmoCount());
        }

        private void M9CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateCurrentAmmoCount(M9CurrentAmmoCount());
        }

        private void M9MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.UpdateMaxAmmoCount(M9MaxAmmoCount());
        }

        private void SocomCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateCurrentAmmoCount(SOCOMCurrentAmmoCount());
        }

        private void SocomMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.UpdateMaxAmmoCount(SOCOMMaxAmmoCount());
        }

        private void UspCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateCurrentAmmoCount(USPCurrentAmmoCount());
        }

        private void UspMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.UpdateMaxAmmoCount(USPMaxAmmoCount());
        }

        private void ChaffCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateCurrentAmmoCount(ChaffCurrentAmmoCount());
        }

        private void ChaffMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.UpdateMaxAmmoCount(ChaffMaxAmmoCount());
        }

        private void StunCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateCurrentAmmoCount(StunCurrentAmmoCount());
        }

        private void StunMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.UpdateMaxAmmoCount(StunMaxAmmoCount());
        }

        private void GrenadeCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateCurrentAmmoCount(GrenadeCurrentAmmoCount());
        }

        private void GrenadeMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.UpdateMaxAmmoCount(GrenadeMaxAmmoCount());
        }

        private void MagazineCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateCurrentAmmoCount(MagazineCurrentAmmoCount());
        }

        private void MagazineMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.UpdateMaxAmmoCount(MagazineMaxAmmoCount());
        }

        private void AkCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.UpdateCurrentAmmoCount(AKCurrentAmmoCount());
        }

        private void M4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateCurrentAmmoCount(M4CurrentAmmoCount());
        }

        private void M4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.UpdateMaxAmmoCount(M4MaxAmmoCount());
        }

        private void Psg1CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateCurrentAmmoCount(PSG1CurrentAmmoCount());
        }

        private void Psg1MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.UpdateMaxAmmoCount(PSG1MaxAmmoCount());
        }

        private void Psg1TCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateCurrentAmmoCount(PSG1TCurrentAmmoCount());
        }

        private void Psg1TMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.UpdateMaxAmmoCount(PSG1TMaxAmmoCount());
        }

        private void Rgb6CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateCurrentAmmoCount(RGB6CurrentAmmoCount());
        }

        private void Rgb6MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.UpdateMaxAmmoCount(RGB6MaxAmmoCount());
        }

        private void NikitaCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateCurrentAmmoCount(NikitaCurrentAmmoCount());
        }

        private void NikitaMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.UpdateMaxAmmoCount(NikitaMaxAmmoCount());
        }

        private void StingerCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateCurrentAmmoCount(StingerCurrentAmmoCount());
        }

        private void StingerMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.UpdateMaxAmmoCount(StingerMaxAmmoCount());
        }

        private void ClaymoreCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateCurrentAmmoCount(ClaymoreCurrentAmmoCount());
        }

        private void ClaymoreMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.UpdateMaxAmmoCount(ClaymoreMaxAmmoCount());
        }

        private void C4CurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateCurrentAmmoCount(ClaymoreCurrentAmmoCount());
        }

        private void C4MaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.UpdateMaxAmmoCount(ClaymoreMaxAmmoCount());
        }

        private void BookCurrentBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateCurrentAmmoCount(BookCurrentAmmoCount());
        }

        private void BookMaxBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.UpdateMaxAmmoCount(BookMaxAmmoCount());
        }

        private void HfBladeLethalBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToLethal();
        }

        private void HfBladeStunBtn_Click(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.SetToStun();
        }

        private void M9CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M9.ToggleWeapon(m9CheckBox.Checked);
        }

        private void SocomCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.SOCOM.ToggleWeapon(socomCheckBox.Checked);
        }

        private void UspCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.USP.ToggleWeapon(uspCheckBox.Checked);
        }

        private void ChaffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.ChaffGrenade.ToggleWeapon(chaffCheckBox.Checked);
        }

        private void StunCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.StunGrenade.ToggleWeapon(stunCheckBox.Checked);
        }

        private void GrenadeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Grenade.ToggleWeapon(grenadeCheckBox.Checked);
        }

        private void MagazineCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Magazine.ToggleWeapon(magazineCheckBox.Checked);
        }

        private void AkCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.AKS74u.ToggleWeapon(akCheckBox.Checked);
        }

        private void M4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.M4.ToggleWeapon(m4CheckBox.Checked);
        }

        private void Psg1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1.ToggleWeapon(psg1CheckBox.Checked);
        }

        private void Psg1TCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.PSG1T.ToggleWeapon(psg1TCheckBox.Checked);
        }

        private void Rgb6CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.RGB6.ToggleWeapon(rgb6CheckBox.Checked);
        }

        private void NikitaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Nikita.ToggleWeapon(nikitaCheckBox.Checked);
        }

        private void StingerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Stinger.ToggleWeapon(stingerCheckBox.Checked);
        }

        private void ClaymoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Claymore.ToggleWeapon(claymoreCheckBox.Checked);
        }

        private void C4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.C4.ToggleWeapon(c4CheckBox.Checked);
        }

        private void BookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Book.ToggleWeapon(bookCheckBox.Checked);
        }

        private void CoolantCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.Coolant.ToggleWeapon(coolantCheckBox.Checked);
        }

        private void Dmic1CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic1.ToggleWeapon(dmic1CheckBox.Checked);
        }

        private void Dmic2CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.DMic2.ToggleWeapon(dmic2CheckBox.Checked);
        }

        private void HfBladeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MGS2UsableObjects.HighFrequencyBlade.ToggleWeapon(hfBladeCheckBox.Checked);
        }
        #endregion
    }
}
