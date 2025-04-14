using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public struct MGS2Resource
    {
        //TODO: MAY NEED TO ADD TRIs as well, sadge
        //TODO: should all of these have ctxr's associated with them?
        public static OldResource ItemBox = new OldResource("itembox",
           kms: "", cmdl: "", ctxr: "", tri: "assets/tri/us/itembox.tri,us/stage/XXXX/cache/00883186.tri,cache/00883186.tri\r\r\n");
        public static OldResource ColdMedsLabel = new OldResource("cold_medicine_label", 
            kms: "assets/kms/us/cold_medicine_label_stage_a03b.kms,us/stage/XXXX/cache/00f971fb.kms,cache/00f971fb.kms\r\r\n", 
            cmdl: "assets/kms/us/cold_medicine_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f971fb.cmdl,eu/stage/XXXX/cache/00f971fb.cmdl\r\r\n",
            ctxr: "textures/flatlist/coldmedicine_tx_alp.bmp.ctxr,stage/XXXX/cache/coldmedicine_tx_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/00ed17f6.ctxr\r\r\n", tri: ""); 
        public static OldResource ThermalGogglesLabel = new OldResource("tgl_label", 
            kms: "assets/kms/us/tgl_label_stage_a03b.kms,us/stage/XXXX/cache/006968d1.kms,cache/006968d1.kms\r\r\n",
            cmdl: "assets/kms/us/tgl_label_stage_a03b.cmdl,us/stage/XXXX/cache/006968d1.cmdl,eu/stage/XXXX/cache/006968d1.cmdl\r\r\n",
            ctxr: "", tri: ""); 
        public static OldResource GoggleIbox = new OldResource("goggle_ibox_stage", 
            kms: "assets/kms/us/goggle_ibox_stage_a00c.kms,us/stage/XXXX/cache/00706bd2.kms,cache/00706bd2.kms\r\r\n",
            cmdl: "assets/kms/us/goggle_ibox_stage_a00c.cmdl,us/stage/XXXX/cache/00706bd2.cmdl,eu/stage/XXXX/cache/00706bd2.cmdl\r\r\n",
            ctxr: "textures/flatlist/ibox_all4.bmp.ctxr,stage/XXXX/cache/ibox_all4.bmp.ctxr,eu/stage/XXXX/cache/00706bd2/008e6a6a.ctxr\r\r\n",
            tri: "assets/tri/us/goggle_ibox.tri,us/stage/XXXX/cache/00706bd2.tri,cache/00706bd2.tri\r\r\n"); 
        public static OldResource GoggleSh = new OldResource("goggle_ibox_sh", 
            kms: "assets/kms/us/goggle_ibox_sh_stage_a00c.kms,us/stage/XXXX/cache/00eac2fd.kms,cache/00eac2fd.kms\r\r\n",
            cmdl: "assets/kms/us/goggle_ibox_sh_stage_a00c.cmdl,us/stage/XXXX/cache/00eac2fd.cmdl,eu/stage/XXXX/cache/00eac2fd.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource AKAmmoLabel = new OldResource("aks_amo_label", 
            kms: "assets/kms/us/aks_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003ce0e9.kms,cache/003ce0e9.kms\r\r\n",
            cmdl: "assets/kms/us/aks_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003ce0e9.cmdl,eu/stage/XXXX/cache/003ce0e9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource AKWeaponLabel = new OldResource("ak_label", 
            kms: "assets/kms/us/ak_label_stage_a03b.kms,us/stage/XXXX/cache/00f53890.kms,cache/00f53890.kms\r\r\n",
            cmdl: "assets/kms/us/ak_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f53890.cmdl,eu/stage/XXXX/cache/00f53890.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource MagazineIbox = new OldResource("magazine_ibox", 
            kms: "assets/kms/us/magazine_ibox_stage_a03b.kms,us/stage/XXXX/cache/00ca7cd0.kms,cache/00ca7cd0.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00ca7cd0.cmdl,eu/stage/XXXX/cache/00ca7cd0.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource MagazineLabel = new OldResource("magazine_label", 
            kms: "assets/kms/us/magazine_label_stage_a03b.kms,us/stage/XXXX/cache/007ee425.kms,cache/007ee425.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_label_stage_a03b.cmdl,us/stage/XXXX/cache/007ee425.cmdl,eu/stage/XXXX/cache/007ee425.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource MagazineSh = new OldResource("magazine_sh", 
            kms: "assets/kms/us/magazine_sh_stage_a03b.kms,us/stage/XXXX/cache/009e33e1.kms,cache/009e33e1.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009e33e1.cmdl,eu/stage/XXXX/cache/009e33e1.cmdl\r\r\n",
            ctxr: "", tri: ""   );
        public static OldResource C4Label = new OldResource("cfr_label", 
            kms: "assets/kms/us/cfr_label_stage_a03b.kms,us/stage/XXXX/cache/00586251.kms,cache/00586251.kms\r\r\n",
            cmdl: "assets/kms/us/cfr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00586251.cmdl,eu/stage/XXXX/cache/00586251.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource ClaymoreLabel = new OldResource("clm_label",
            kms: "assets/kms/us/clm_label_stage_a03b.kms,us/stage/XXXX/cache/00589111.kms,cache/00589111.kms\r\r\n",
            cmdl: "assets/kms/us/clm_label_stage_a03b.cmdl,us/stage/XXXX/cache/00589111.cmdl,eu/stage/XXXX/cache/00589111.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DmicLabel = new OldResource("dmp_label",
            kms: "assets/kms/us/dmp_label_stage_a03b.kms,us/stage/XXXX/cache/005999d1.kms,cache/005999d1.kms\r\r\n",
            cmdl: "assets/kms/us/dmp_label_stage_a03b.cmdl,us/stage/XXXX/cache/005999d1.cmdl,eu/stage/XXXX/cache/005999d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource GrenadeLabel = new OldResource("gre_label",
            kms: "assets/kms/us/gre_label_stage_a03b.kms,us/stage/XXXX/cache/005cbf11.kms,cache/005cbf11.kms\r\r\n",
            cmdl: "assets/kms/us/gre_label_stage_a03b.cmdl,us/stage/XXXX/cache/005cbf11.cmdl,eu/stage/XXXX/cache/005cbf11.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource M4AmmoLabel = new OldResource("m4_amo_label",
            kms: "assets/kms/us/m4_amo_label_stage_a03b.kms,us/stage/XXXX/cache/0036d0ed.kms,cache/0036d0ed.kms\r\r\n",
            cmdl: "assets/kms/us/m4_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/0036d0ed.cmdl,eu/stage/XXXX/cache/0036d0ed.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource M4WeaponLabel = new OldResource("m4_label",
            kms: "assets/kms/us/m4_label_stage_a03b.kms,us/stage/XXXX/cache/00f58ad0.kms,cache/00f58ad0.kms\r\r\n",
            cmdl: "assets/kms/us/m4_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f58ad0.cmdl,eu/stage/XXXX/cache/00f58ad0.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource NikitaAmmoLabel = new OldResource("nkt_amo_label",
            kms: "assets/kms/us/nkt_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003db0ed.kms,cache/003db0ed.kms\r\r\n",
            cmdl: "assets/kms/us/nkt_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003db0ed.cmdl,eu/stage/XXXX/cache/003db0ed.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource NikitaWeaponLabel = new OldResource("nkt_label",
            kms: "assets/kms/us/nkt_label_stage_a03b.kms,us/stage/XXXX/cache/00638ad1.kms,cache/00638ad1.kms\r\r\n",
            cmdl: "assets/kms/us/nkt_label_stage_a03b.cmdl,us/stage/XXXX/cache/00638ad1.cmdl,eu/stage/XXXX/cache/00638ad1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource PSG1AmmoLabel = new OldResource("psg_amo_label",
            kms: "assets/kms/us/psg_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003dd4b9.kms,cache/003dd4b9.kms\r\r\n",
            cmdl: "assets/kms/us/psg_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003dd4b9.cmdl,eu/stage/XXXX/cache/003dd4b9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource PSG1WeaponLabel = new OldResource("psg_label",
            kms: "assets/kms/us/psg_label_stage_a03b.kms,us/stage/XXXX/cache/0065c791.kms,cache/0065c791.kms\r\r\n",
            cmdl: "assets/kms/us/psg_label_stage_a03b.cmdl,us/stage/XXXX/cache/0065c791.cmdl,eu/stage/XXXX/cache/0065c791.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource PSG1TAmmoLabel = new OldResource("psgt_amo_label",
            kms: "assets/kms/us/psgt_amo_label_stage_a03b.kms,us/stage/XXXX/cache/001dfe6e.kms,cache/001dfe6e.kms\r\r\n",
            cmdl: "assets/kms/us/psgt_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/001dfe6e.cmdl,eu/stage/XXXX/cache/001dfe6e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource PSG1TWeaponLabel = new OldResource("psg_t_label",
            kms: "assets/kms/us/psg_t_label_stage_a03b.kms,us/stage/XXXX/cache/00bc2c9f.kms,cache/00bc2c9f.kms\r\r\n",
            cmdl: "assets/kms/us/psg_t_label_stage_a03b.cmdl,us/stage/XXXX/cache/00bc2c9f.cmdl,eu/stage/XXXX/cache/00bc2c9f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RGB6AmmoLabel = new OldResource("rgb_amo_label",
            kms: "assets/kms/us/rgb_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003deea5.kms,cache/003deea5.kms\r\r\n",
            cmdl: "assets/kms/us/rgb_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003deea5.cmdl,eu/stage/XXXX/cache/003deea5.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RGB6WeaponLabel = new OldResource("rgb_label",
            kms: "assets/kms/us/rgb_label_stage_a03b.kms,us/stage/XXXX/cache/00676651.kms,cache/00676651.kms\r\r\n",
            cmdl: "assets/kms/us/rgb_label_stage_a03b.cmdl,us/stage/XXXX/cache/00676651.cmdl,eu/stage/XXXX/cache/00676651.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource StingerAmmoLabel = new OldResource("stg_amo_label",
            kms: "assets/kms/us/stg_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003e0539.kms,cache/003e0539.kms\r\r\n",
            cmdl: "assets/kms/us/stg_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003e0539.cmdl,eu/stage/XXXX/cache/003e0539.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource StingerWeaponLabel = new OldResource("stg_label",
            kms: "assets/kms/us/stg_label_stage_a03b.kms,us/stage/XXXX/cache/0068cf91.kms,cache/0068cf91.kms\r\r\n",
            cmdl: "assets/kms/us/stg_label_stage_a03b.cmdl,us/stage/XXXX/cache/0068cf91.cmdl,eu/stage/XXXX/cache/0068cf91.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource BodyArmorLabel = new OldResource("bam_label",
            kms: "assets/kms/us/bam_label_stage_a03b.kms,us/stage/XXXX/cache/00573911.kms,cache/00573911.kms\r\r\n",
            cmdl: "assets/kms/us/bam_label_stage_a03b.cmdl,us/stage/XXXX/cache/00573911.cmdl,eu/stage/XXXX/cache/00573911.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DigitalCameraIbox = new OldResource("digital_camera_ibox",
            kms: "assets/kms/us/digital_camera_ibox_stage_a03b.kms,us/stage/XXXX/cache/001cd720.kms,cache/001cd720.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/001cd720.cmdl,eu/stage/XXXX/cache/001cd720.cmdl\r\r\n",
            ctxr: "textures/flatlist/degital_camera.bmp.ctxr,stage/XXXX/cache/degital_camera.bmp.ctxr,eu/stage/XXXX/cache/00883186/00fb2060.ctxr\r\r\n",
            tri: "");
        public static OldResource DigitalCameraLabel = new OldResource("digital_camera_label",
            kms: "assets/kms/us/digital_camera_label_stage_a03b.kms,us/stage/XXXX/cache/00ca2e0f.kms,cache/00ca2e0f.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_label_stage_a03b.cmdl,us/stage/XXXX/cache/00ca2e0f.cmdl,eu/stage/XXXX/cache/00ca2e0f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DigitalCameraSh = new OldResource("digital_camera_sh",
            kms: "assets/kms/us/digital_camera_sh_stage_a03b.kms,us/stage/XXXX/cache/00320878.kms,cache/00320878.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00320878.cmdl,eu/stage/XXXX/cache/00320878.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource PentazeminLabel = new OldResource("dzp2_label",
            kms: "assets/kms/us/dzp2_label_stage_a03b.kms,us/stage/XXXX/cache/00efa25d.kms,cache/00efa25d.kms\r\r\n",
            cmdl: "assets/kms/us/dzp2_label_stage_a03b.cmdl,us/stage/XXXX/cache/00efa25d.cmdl,eu/stage/XXXX/cache/00efa25d.cmdl\r\r\n",
            ctxr: "textures/flatlist/dzp2_tx_alp.bmp.ctxr,stage/XXXX/cache/dzp2_tx_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/0062bc3f.ctxr\r\r\n",
            tri: "");
        public static OldResource SensorBLabel = new OldResource("bsn_b_label",
            kms: "assets/kms/us/bsn_b_label_stage_a03b.kms,us/stage/XXXX/cache/00c327e7.kms,cache/00c327e7.kms\r\r\n",
            cmdl: "assets/kms/us/bsn_b_label_stage_a03b.cmdl,us/stage/XXXX/cache/00c327e7.cmdl,eu/stage/XXXX/cache/00c327e7.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource SocomSuppressorLabel = new OldResource("scm_sp_label",
            kms: "assets/kms/us/scm_sp_label_stage_a03b.kms,us/stage/XXXX/cache/00f504ea.kms,cache/00f504ea.kms\r\r\n",
            cmdl: "assets/kms/us/scm_sp_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f504ea.cmdl,eu/stage/XXXX/cache/00f504ea.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource MineDetectorLabel = new OldResource("mnd_label",
            kms: "assets/kms/us/mnd_label_stage_a03b.kms,us/stage/XXXX/cache/00629ed1.kms,cache/00629ed1.kms\r\r\n",
            cmdl: "assets/kms/us/mnd_label_stage_a03b.cmdl,us/stage/XXXX/cache/00629ed1.cmdl,eu/stage/XXXX/cache/00629ed1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource NVGLabel = new OldResource("ngl_label",
            kms: "assets/kms/us/ngl_label_stage_a03b.kms,us/stage/XXXX/cache/006368d1.kms,cache/006368d1.kms\r\r\n",
            cmdl: "assets/kms/us/ngl_label_stage_a03b.cmdl,us/stage/XXXX/cache/006368d1.cmdl,eu/stage/XXXX/cache/006368d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RifleAmmoIbox = new OldResource("rifle_amo_ibox",
            kms: "assets/kms/us/rifle_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/0012ff3a.kms,cache/0012ff3a.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0012ff3a.cmdl,eu/stage/XXXX/cache/0012ff3a.cmdl\r\r\n",
            ctxr: "", tri: ""); //TODO: there are at least 3 of these referenced in w22a. what is going on with this
        public static OldResource RifleAmmoSh = new OldResource("handgun_amo_ibox",
            kms: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/009e9447.kms,cache/009e9447.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009e9447.cmdl,eu/stage/XXXX/cache/009e9447.cmdl\r\r\n",
            ctxr: "", tri: ""); //This is NOT a mistake on my part, this is Konami/Bluepoint's doing. This is correct to the game files
        public static OldResource RifleIbox = new OldResource("rifle_ibox_stage",
            kms: "assets/kms/us/rifle_ibox_stage_a03b.kms,us/stage/XXXX/cache/00d26236.kms,cache/00d26236.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00d26236.cmdl,eu/stage/XXXX/cache/00d26236.cmdl\r\r\n",
            ctxr: "textures/flatlist/rifle_ibox.bmp_94bca08db682d231ae1d48d2b6385598.ctxr,stage/XXXX/cache/rifle_ibox.bmp_94bca08db682d231ae1d48d2b6385598.ctxr,eu/stage/XXXX/cache/00883186/00d26236.ctxr\r\r\n",
            tri: "");
        public static OldResource RifleSh = new OldResource("rifle_ibox_sh",
            kms: "assets/kms/us/rifle_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/001cf3f9.kms,cache/001cf3f9.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/001cf3f9.cmdl,eu/stage/XXXX/cache/001cf3f9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource LauncherAmmoIbox = new OldResource("launcher_amo_ibox_stage",
            kms: "assets/kms/us/launcher_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/00eb0f44.kms,cache/00eb0f44.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00eb0f44.cmdl,eu/stage/XXXX/cache/00eb0f44.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource LauncherAmmoSh = new OldResource("launcher_amo_ibox_sh",
            kms: "assets/kms/us/launcher_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00a4004f.kms,cache/00a4004f.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00a4004f.cmdl,eu/stage/XXXX/cache/00a4004f.cmdl\r\r\n",
            ctxr: "textures/flatlist/launcher_ammo_side.bmp.ctxr,stage/XXXX/cache/launcher_ammo_side.bmp.ctxr,eu/stage/XXXX/cache/00883186/00ca4e21.ctxr\r\r\n", tri: "");
        public static OldResource LauncherIbox = new OldResource("launcher_ibox_stage",
            kms: "assets/kms/us/launcher_ibox_stage_a03b.kms,us/stage/XXXX/cache/005362e4.kms,cache/005362e4.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/005362e4.cmdl,eu/stage/XXXX/cache/005362e4.cmdl\r\r\n",
            ctxr: "textures/flatlist/launcher_ibox.bmp.ctxr,stage/XXXX/cache/launcher_ibox.bmp.ctxr,eu/stage/XXXX/cache/00883186/005362e4.ctxr\r\r\n",
            tri: "");
        public static OldResource LauncherSh = new OldResource("launcher_ibox_sh",
            kms: "assets/kms/us/launcher_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0073b479.kms,cache/0073b479.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0073b479.cmdl,eu/stage/XXXX/cache/0073b479.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource Box2Ibox = new OldResource("box2_ibox",
            kms: "assets/kms/us/box2_ibox_stage_a03b.kms,us/stage/XXXX/cache/008bacc2.kms,cache/008bacc2.kms\r\r\n",
            cmdl: "assets/kms/us/box2_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/008bacc2.cmdl,eu/stage/XXXX/cache/008bacc2.cmdl\r\r\n",
            ctxr: "textures/flatlist/ibox2_tx_all_alp.bmp.ctxr,stage/XXXX/cache/ibox2_tx_all_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/0051c0fb.ctxr\r\r\n",
            tri: "");
        public static OldResource Box2Sh = new OldResource("box2_ibox_sh",
            kms: "assets/kms/us/box2_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0062d09e.kms,cache/0062d09e.kms\r\r\n",
            cmdl: "assets/kms/us/box2_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0062d09e.cmdl,eu/stage/XXXX/cache/0062d09e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource CbxLabel = new OldResource("cbx_label",
            kms: "assets/kms/us/cbx_label_stage_a03b.kms,us/stage/XXXX/cache/005843d1.kms,cache/005843d1.kms\r\r\n",
            cmdl: "assets/kms/us/cbx_label_stage_a03b.cmdl,us/stage/XXXX/cache/005843d1.cmdl,eu/stage/XXXX/cache/005843d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DetectorIbox = new OldResource("detector_ibox_stage",
            kms: "assets/kms/us/detector_ibox_stage_a03b.kms,us/stage/XXXX/cache/0033475f.kms,cache/0033475f.kms\r\r\n",
            cmdl: "assets/kms/us/detector_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0033475f.cmdl,eu/stage/XXXX/cache/0033475f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DetectorSh = new OldResource("detector_ibox_sh",
            kms: "assets/kms/us/detector_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00b1246b.kms,cache/00b1246b.kms\r\r\n",
            cmdl: "assets/kms/us/detector_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00b1246b.cmdl,eu/stage/XXXX/cache/00b1246b.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource DmicIbox = new OldResource("box_ibox_stage",
            kms: "assets/kms/us/box_ibox_stage_a03b.kms,us/stage/XXXX/cache/0029430e.kms,cache/0029430e.kms\r\r\n",
            cmdl: "assets/kms/us/box_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0029430e.cmdl,eu/stage/XXXX/cache/0029430e.cmdl\r\r\n",
            ctxr: "textures/flatlist/dmp_ibx_label_alp.bmp.ctxr,stage/XXXX/cache/dmp_ibx_label_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/00055db2.ctxr\r\r\n",
            tri: "");
        public static OldResource DmicSh = new OldResource("box_ibox_sh",
            kms: "assets/kms/us/box_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00889f69.kms,cache/00889f69.kms\r\r\n",
            cmdl: "assets/kms/us/box_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00889f69.cmdl,eu/stage/XXXX/cache/00889f69.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RationIbox = new OldResource("ration_ibox_stage",
            kms: "assets/kms/us/ration_ibox_stage_a03b.kms,us/stage/XXXX/cache/00bd7cce.kms,cache/00bd7cce.kms\r\r\n",
            cmdl: "assets/kms/us/ration_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00bd7cce.cmdl,eu/stage/XXXX/cache/00bd7cce.cmdl\r\r\n",
            ctxr: "textures/flatlist/ration_box.bmp.ctxr,stage/XXXX/cache/ration_box.bmp.ctxr,eu/stage/XXXX/cache/00883186/00b5d18b.ctxr\r\r\n", tri: "");
        public static OldResource M9AmmoLabel = new OldResource("m92_amo_label",
            kms: "assets/kms/us/m92_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003d86e5.kms,cache/003d86e5.kms\r\r\n",
            cmdl: "assets/kms/us/m92_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003d86e5.cmdl,eu/stage/XXXX/cache/003d86e5.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource M9WeaponLabel = new OldResource("m92_label",
            kms: "assets/kms/us/m92_label_stage_a03b.kms,us/stage/XXXX/cache/0060ea51.kms,cache/0060ea51.kms\r\r\n",
            cmdl: "assets/kms/us/m92_label_stage_a03b.cmdl,us/stage/XXXX/cache/0060ea51.cmdl,eu/stage/XXXX/cache/0060ea51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource StunLabel = new OldResource("sgr_label",
            kms: "assets/kms/us/sgr_label_stage_a03b.kms,us/stage/XXXX/cache/00686a51.kms,cache/00686a51.kms\r\r\n",
            cmdl: "assets/kms/us/sgr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00686a51.cmdl,eu/stage/XXXX/cache/00686a51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource BandageLabel = new OldResource("sbs_label",
            kms: "assets/kms/us/sbs_label_stage_a03b.kms,us/stage/XXXX/cache/00684291.kms,cache/00684291.kms\r\r\n",
            cmdl: "assets/kms/us/sbs_label_stage_a03b.cmdl,us/stage/XXXX/cache/00684291.cmdl,eu/stage/XXXX/cache/00684291.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource ShaverLabel = new OldResource("shv_label",
            kms: "assets/kms/us/shv_label_stage_a03b.kms,us/stage/XXXX/cache/00687351.kms,cache/00687351.kms\r\r\n",
            cmdl: "assets/kms/us/shv_label_stage_a03b.cmdl,us/stage/XXXX/cache/00687351.cmdl,eu/stage/XXXX/cache/00687351.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource MedicineIbox = new OldResource("medicine_ibox_stage",
            kms: "assets/kms/us/medicine_ibox_stage_a03b.kms,us/stage/XXXX/cache/00b37ec5.kms,cache/00b37ec5.kms\r\r\n",
            cmdl: "assets/kms/us/medicine_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00b37ec5.cmdl,eu/stage/XXXX/cache/00b37ec5.cmdl\r\r\n",
            ctxr: "textures/flatlist/medicine_box.bmp.ctxr,stage/XXXX/cache/medicine_box.bmp.ctxr,eu/stage/XXXX/cache/00883186/006d819b.ctxr\r\r\n", tri: "");
        public static OldResource MedicineSh = new OldResource("medicine_ibox_sh",
            kms: "assets/kms/us/medicine_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00646487.kms,cache/00646487.kms\r\r\n",
            cmdl: "assets/kms/us/medicine_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00646487.cmdl,eu/stage/XXXX/cache/00646487.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource HandgunIbox = new OldResource("handgun_ibox_stage",
            kms: "assets/kms/us/handgun_ibox_stage_a03b.kms,us/stage/XXXX/cache/004da20c.kms,cache/004da20c.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/004da20c.cmdl,eu/stage/XXXX/cache/004da20c.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource HandgunSh = new OldResource("handgun_ibox_sh",
            kms: "assets/kms/us/handgun_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0007b199.kms,cache/0007b199.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0007b199.cmdl,eu/stage/XXXX/cache/0007b199.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource ChaffLabel = new OldResource("cgr_label",
            kms: "assets/kms/us/cgr_label_stage_a03b.kms,us/stage/XXXX/cache/00586a51.kms,cache/00586a51.kms\r\r\n",
            cmdl: "assets/kms/us/cgr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00586a51.cmdl,eu/stage/XXXX/cache/00586a51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource SocomAmmoLabel = new OldResource("scm_amo_label",
            kms: "assets/kms/us/scm_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003dfcd1.kms,cache/003dfcd1.kms\r\r\n",
            cmdl: "assets/kms/us/scm_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003dfcd1.cmdl,eu/stage/XXXX/cache/003dfcd1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource GrenadeIbox = new OldResource("grenade_ibox_stage",
            kms: "assets/kms/us/grenade_ibox_stage_a03b.kms,us/stage/XXXX/cache/00376d7d.kms,cache/00376d7d.kms\r\r\n",
            cmdl: "assets/kms/us/grenade_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00376d7d.cmdl,eu/stage/XXXX/cache/00376d7d.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource GrenadeSh = new OldResource("grenade_ibox_sh",
            kms: "assets/kms/us/grenade_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00c0267e.kms,cache/00c0267e.kms\r\r\n",
            cmdl: "assets/kms/us/grenade_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00c0267e.cmdl,eu/stage/XXXX/cache/00c0267e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource HandgunAmmoIbox = new OldResource("rifle_amo_ibox_stage",
            kms: "assets/kms/us/rifle_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/006ab337.kms,cache/006ab337.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/006ab337.cmdl,eu/stage/XXXX/cache/006ab337.cmdl\r\r\n",
            ctxr: "", tri: ""); //again, this is not a mistake on our part. This was done by Konami/Bluepoint
        public static OldResource HandgunAmmoSh = new OldResource("handgun_amo_ibox_sh",
            kms: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/009d4021.kms,cache/009d4021.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009d4021.cmdl,eu/stage/XXXX/cache/009d4021.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RationSh = new OldResource("ration_ibox_sh",
            kms: "assets/kms/us/ration_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0068e986.kms,cache/0068e986.kms\r\r\n",
            cmdl: "assets/kms/us/ration_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0068e986.cmdl,eu/stage/XXXX/cache/0068e986.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RationLabel = new OldResource("rtn_label",
            kms: "assets/kms/us/rtn_label_stage_a03b.kms,us/stage/XXXX/cache/0067d151.kms,cache/0067d151.kms\r\r\n",
            cmdl: "assets/kms/us/rtn_label_stage_a03b.cmdl,us/stage/XXXX/cache/0067d151.cmdl,eu/stage/XXXX/cache/0067d151.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource AKSuppressorLabel = new OldResource("ak_sp_label",
            kms: "assets/kms/us/ak_sp_label_stage_a03b.kms,us/stage/XXXX/cache/00b4cb62.kms,cache/00b4cb62.kms\r\r\n",
            cmdl: "assets/kms/us/ak_sp_label_stage_a03b.cmdl,us/stage/XXXX/cache/00b4cb62.cmdl,eu/stage/XXXX/cache/00b4cb62.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource CoolantSprayLabel = new OldResource("cls_label",
            kms: "assets/kms/us/cls_label.kms,us/stage/XXXX/cache/00589291.kms,cache/00589291.kms\r\r\n",
            cmdl: "assets/kms/us/cls_label.cmdl,us/stage/XXXX/cache/00589291.cmdl,eu/stage/XXXX/cache/00589291.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource UspLabel = new OldResource("usp_label",
            kms: "assets/kms/us/usp_label_stage_a03b.kms,us/stage/XXXX/cache/006ac9d1.kms,cache/006ac9d1.kms\r\r\n",
            cmdl: "assets/kms/us/usp_label_stage_a03b.cmdl,us/stage/XXXX/cache/006ac9d1.cmdl,eu/stage/XXXX/cache/006ac9d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource SocomLabel = new OldResource("scm_label",
            kms: "assets/kms/us/scm_label.kms,us/stage/XXXX/cache/00684911.kms,cache/00684911.kms\r\r\n",
            cmdl: "assets/kms/us/scm_label.cmdl,us/stage/XXXX/cache/00684911.cmdl,eu/stage/XXXX/cache/00684911.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource CigarettesLabel = new OldResource("cigarette_label",
            kms: "assets/kms/us/cigarette_label.kms,us/stage/XXXX/cache/00a2717f.kms,cache/00a2717f.kms\r\r\n",
            cmdl: "assets/kms/us/cigarette_label.cmdl,us/stage/XXXX/cache/00a2717f.cmdl,eu/stage/XXXX/cache/00a2717f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource CigarettesIbox = new OldResource("cigarette_ibox",
            kms: "assets/kms/us/cigarette_ibox.kms,us/stage/XXXX/cache/009b993b.kms,cache/009b993b.kms\r\r\n",
            cmdl: "assets/kms/us/cigarette_ibox.cmdl,us/stage/XXXX/cache/009b993b.cmdl,eu/stage/XXXX/cache/009b993b.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource CigarettesIboxSh = new OldResource("cigarette_sh",
            kms: "assets/kms/us/cigarette_sh.kms,us/stage/XXXX/cache/00b8e828.kms,cache/00b8e828.kms\r\r\n",
            cmdl: "assets/kms/us/cigarette_sh.cmdl,us/stage/XXXX/cache/00b8e828.cmdl,eu/stage/XXXX/cache/00b8e828.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource SensorALabel = new OldResource("bsn_a_label",
            kms: "assets/kms/us/bsn_a_label.kms,us/stage/XXXX/cache/00c327a7.kms,cache/00c327a7.kms\r\r\n",
            cmdl: "assets/kms/us/bsn_a_label.cmdl,us/stage/XXXX/cache/00c327a7.cmdl,eu/stage/XXXX/cache/00c327a7.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource APSensorIbox = new OldResource("a_p_sensor_ibox.",
            kms: "assets/kms/us/a_p_sensor_ibox.kms,us/stage/XXXX/cache/003cfc74.kms,cache/003cfc74.kms\r\r\n",
            cmdl: "assets/kms/us/a_p_sensor_ibox.cmdl,us/stage/XXXX/cache/003cfc74.cmdl,eu/stage/XXXX/cache/003cfc74.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource APSensorLabel = new OldResource("a_p_sensor_ibox_label",
            kms: "assets/kms/us/a_p_sensor_ibox_label.kms,us/stage/XXXX/cache/003132e0.kms,cache/003132e0.kms\r\r\n",
            cmdl: "assets/kms/us/a_p_sensor_ibox_label.cmdl,us/stage/XXXX/cache/003132e0.cmdl,eu/stage/XXXX/cache/003132e0.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource ScopeCustomBox = new OldResource("sougan",
            kms: "assets/kms/us/sougan.kms,us/stage/XXXX/cache/002c297b.kms,cache/002c297b.kms\r\r\n",
            cmdl: "assets/kms/us/sougan.cmdl,us/stage/XXXX/cache/002c297b.cmdl,eu/stage/XXXX/cache/002c297b.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static OldResource RaiBlade = new OldResource("rai_blade",
            kms: "", cmdl: "", ctxr: "", tri: "",
            sar: "assets/sar/us/rai_blade.sar,us/stage/XXXX/cache/00ccb3e9.sar,cache/00ccb3e9.sar\r\r\n",
            mar: "assets/mar/us/rai_blade.mar,us/stage/XXXX/cache/00ccb3e9.mar,cache/00ccb3e9.mar\r\r\n");
        public static OldResource HfbMt = new OldResource("hfb_mt",
            kms: "assets/kms/us/hfb_mt.kms,us/stage/XXXX/cache/00928aea.kms,cache/00928aea.kms\r\r\n",
            cmdl: "assets/kms/us/hfb_mt.cmdl,us/stage/XXXX/cache/00928aea.cmdl,eu/stage/XXXX/cache/00928aea.cmdl\r\r\n",
            ctxr: "",
            tri: "assets/tri/us/hfb_mt.tri,us/stage/XXXX/cache/00928aea.tri,cache/00928aea.tri\r\r\n");
        public static OldResource HfbSubMt = new OldResource("hfb_sub",
            kms: "assets/kms/us/hfb_sub_mt.kms,us/stage/XXXX/cache/00ebb2ce.kms,cache/00ebb2ce.kms\r\r\n",
            cmdl: "assets/kms/us/hfb_sub_mt.cmdl,us/stage/XXXX/cache/00ebb2ce.cmdl,eu/stage/XXXX/cache/00ebb2ce.cmdl\r\r\n",
            ctxr: "textures/flatlist/hfb_sub_gr2_alp_ovl.bmp.ctxr,stage/XXXX/cache/hfb_sub_gr2_alp_ovl.bmp.ctxr,eu/stage/XXXX/cache/00928aea/0003d28d.ctxr\r\r\n",
            tri: "");
        public static OldResource HfbMineuchi = new OldResource("hfb_mineuchi_mt",
            kms: "assets/kms/us/hfb_mineuchi_mt.kms,us/stage/XXXX/cache/0019a17d.kms,cache/0019a17d.kms\r\r\n",
            cmdl: "assets/kms/us/hfb_mineuchi_mt.cmdl,us/stage/XXXX/cache/0019a17d.cmdl,eu/stage/XXXX/cache/0019a17d.cmdl\r\r\n",
            ctxr: "",
            tri: "");
        public static OldResource HfbMineuchiSub = new OldResource("hfb_mineuchi_sub_mt",
            kms: "assets/kms/us/hfb_mineuchi_sub_mt.kms,us/stage/XXXX/cache/00142438.kms,cache/00142438.kms\r\r\n",
            cmdl: "assets/kms/us/hfb_mineuchi_sub_mt.cmdl,us/stage/XXXX/cache/00142438.cmdl,eu/stage/XXXX/cache/00142438.cmdl\r\r\n",
            ctxr: "",
            tri: "");
        public static OldResource HfbOvlMod = new OldResource("hfb_ovl_mod",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_ovl_mod1120_alp_emap.bmp.ctxr,stage/XXXX/cache/hfb_ovl_mod1120_alp_emap.bmp.ctxr,eu/stage/XXXX/cache/00310acc/00d1ba75.ctxr\r\r\n");
        public static OldResource HfbSbGr2 = new OldResource("hfb_sb_gr2",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_sb_gr2.bmp.ctxr,stage/XXXX/cache/hfb_sb_gr2.bmp.ctxr,eu/stage/XXXX/cache/00928aea/00ba532b.ctxr\r\r\n");
        public static OldResource HfbSbGrMk = new OldResource("hfb_sb_gr_mk",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_sb_gr_mk.bmp.ctxr,stage/XXXX/cache/hfb_sb_gr_mk.bmp.ctxr,eu/stage/XXXX/cache/00928aea/004d70f4.ctxr\r\r\n");
        public static OldResource Hfbs3SubAlpOvl = new OldResource("hfbs3_sub_alp_ovl",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs3_sub_alp_ovl.bmp.ctxr,stage/XXXX/cache/hfbs3_sub_alp_ovl.bmp.ctxr,eu/stage/XXXX/cache/00928aea/00a028ef.ctxr\r\r\n");
        public static OldResource Hfbs3Sb = new OldResource("hfbs3_sb",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs3_sb.bmp.ctxr,stage/XXXX/cache/hfbs3_sb.bmp.ctxr,eu/stage/XXXX/cache/00928aea/004e650d.ctxr\r\r\n");
        public static OldResource HfbTuba = new OldResource("hfb_tuba",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_tuba.bmp.ctxr,stage/XXXX/cache/hfb_tuba.bmp.ctxr,eu/stage/XXXX/cache/00928aea/002f3aeb.ctxr\r\r\n");
        public static OldResource HfbSoriGr2 = new OldResource("hfb_sori_gr2.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_sori_gr2.bmp.ctxr,stage/XXXX/cache/hfb_sori_gr2.bmp.ctxr,eu/stage/XXXX/cache/00928aea/006414c1.ctxr\r\r\n");
        public static OldResource HfbSubGr2AlpOvl = new OldResource("hfb_sub_gr2_akp_ovl",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_sub_gr2_alp_ovl.bmp.ctxr,stage/XXXX/cache/hfb_sub_gr2_alp_ovl.bmp.ctxr,eu/stage/XXXX/cache/00928aea/0003d28d.ctxr\r\r\n");
        public static OldResource HfbSoliSaya = new OldResource("hfb_soli_saya.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_soli_saya.bmp.ctxr,stage/XXXX/cache/hfb_soli_saya.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/00885bed.ctxr\r\r\n");
        public static OldResource HfbSoliSaya2 = new OldResource("hfb_soli_saya2.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_soli_saya2.bmp.ctxr,stage/XXXX/cache/hfb_soli_saya2.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/000b7de3.ctxr\r\r\n");
        public static OldResource HfbSoriGr = new OldResource("hfb_sori_gr.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfb_sori_gr.bmp.ctxr,stage/XXXX/cache/hfb_sori_gr.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/007b20a4.ctxr\r\r\n");
        public static OldResource Hfbs2Ovl = new OldResource("hfbs2_ovl_mod1120_alp_emap",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs2_ovl_mod1120_alp_emap.bmp.ctxr,stage/XXXX/cache/hfbs2_ovl_mod1120_alp_emap.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/00a4a7db.ctxr\r\r\n");
        public static OldResource HfbsGrtop = new OldResource("hfbs_grtop",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs_grtop.bmp.ctxr,stage/XXXX/cache/hfbs_grtop.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/00140be2.ctxr\r\r\n");
        public static OldResource HfbsOvlMod = new OldResource("hfbs_grtop",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs_ovl_mod1120_alp_emap.bmp.ctxr,stage/XXXX/cache/hfbs_ovl_mod1120_alp_emap.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/00c0dcdd.ctxr\r\r\n");
        public static OldResource HfbsSbGr2 = new OldResource("hfbs_sb_gr2",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/hfbs_sb_gr2.bmp.ctxr,stage/XXXX/cache/hfbs_sb_gr2.bmp.ctxr,eu/stage/XXXX/cache/00cf9afe/0076dccb.ctxr\r\r\n");
        public static OldResource SnaOsOvlMod1120AlpEmap1 = new OldResource("sna_os_ovl_mod_1120_alp_emap",
            ctxr: "textures/flatlist/sna_os_ovl_mod1120_alp_emap.bmp.ctxr,stage/r_XXXX/resident/sna_os_ovl_mod1120_alp_emap.bmp.ctxr,eu/stage/r_XXXX/resident/0055aab4/008df696.ctxr\r\r\n",
            kms: "", cmdl: "", tri: ""); //there are two of these, do we need both?
        public static OldResource SnaOsOvlMod1120AlpEmap2 = new OldResource("sna_os_ovl_mod_1120_alp_emap",
            ctxr: "textures/flatlist/sna_os_ovl_mod1120_alp_emap.bmp.ctxr,stage/r_XXXX/resident/sna_os_ovl_mod1120_alp_emap.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/008df696.ctxr\r\r\n",
            kms: "", cmdl: "", tri: ""); //there are two of these, do we need both?
        public static OldResource SnaOssApad = new OldResource("sna_oss_apad",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_apad.bmp.ctxr,stage/r_XXXX/resident/sna_oss_apad.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00a51d74.ctxr\r\r\n");
        public static OldResource SnaOssArmOvlSubAlp = new OldResource("sna_oss_arm_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_arm_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_arm_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/0055aab4/0013a13e.ctxr\r\r\n");
        public static OldResource SnaOssArmOvlSubAlp2 = new OldResource("sna_oss_arm_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_arm_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_arm_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/0013a13e.ctxr\r\r\n");
        public static OldResource SnaOssArmSs = new OldResource("sna_oss_arm_ss",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_arm_ss.bmp.ctxr,stage/r_XXXX/resident/sna_oss_arm_ss.bmp.ctxr,eu/stage/r_XXXX/resident/0055aab4/009bcd67.ctxr\r\r\n");
        public static OldResource SnaOssArmSs2 = new OldResource("sna_oss_arm_ss",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_arm_ss.bmp.ctxr,stage/r_XXXX/resident/sna_oss_arm_ss.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/009bcd67.ctxr\r\r\n");
        public static OldResource SnaOssBody = new OldResource("sna_oss_body.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_body.bmp.ctxr,stage/r_XXXX/resident/sna_oss_body.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00a599e9.ctxr\r\r\n");
        public static OldResource SnaOssBodyOvlSubAlp = new OldResource("sna_oss_body_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_body_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_body_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/001ef345.ctxr\r\r\n");
        public static OldResource SnaOssEri = new OldResource("sna_oss_eri.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_eri.bmp.ctxr,stage/r_XXXX/resident/sna_oss_eri.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00853930.ctxr\r\r\n");
        public static OldResource SnaOssEriOvlSubAlp = new OldResource("sna_oss_eri_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_eri_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_eri_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/0013613f.ctxr\r\r\n");
        public static OldResource SnaOssHip = new OldResource("sna_oss_hip.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_hip.bmp.ctxr,stage/r_XXXX/resident/sna_oss_hip.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00854417.ctxr\r\r\n");
        public static OldResource SnaOssHipOvlSubAlp = new OldResource("sna_oss_hip_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_hip_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_hip_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00c1d13f.ctxr\r\r\n");
        public static OldResource SnaOssHlstBack = new OldResource("sna_oss_hlst_back.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_hlst_back.bmp.ctxr,stage/r_XXXX/resident/sna_oss_hlst_back.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/0073b05a.ctxr\r\r\n");
        public static OldResource SnaOssHlstFro = new OldResource("sna_oss_hlst_fro.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_hlst_fro.bmp.ctxr,stage/r_XXXX/resident/sna_oss_hlst_fro.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/007bafab.ctxr\r\r\n");
        public static OldResource SnaOssHlstSide = new OldResource("sna_oss_hlst_side.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_hlst_side.bmp.ctxr,stage/r_XXXX/resident/sna_oss_hlst_side.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/007c5074.ctxr\r\r\n");
        public static OldResource SnaOssLegSs = new OldResource("sna_oss_leg_ss.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_leg_ss.bmp.ctxr,stage/r_XXXX/resident/sna_oss_leg_ss.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00c8cd7c.ctxr\r\r\n");
        public static OldResource SnaOssLegOvlSubAlp = new OldResource("sna_oss_leg_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_leg_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_leg_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00b94140.ctxr\r\r\n");
        public static OldResource SnaOssLpad = new OldResource("sna_oss_lpad.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_lpad.bmp.ctxr,stage/r_XXXX/resident/sna_oss_lpad.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00aa9d74.ctxr\r\r\n");
        public static OldResource SnaOssNeck = new OldResource("sna_oss_neck.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_neck.bmp.ctxr,stage/r_XXXX/resident/sna_oss_neck.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00ab71bb.ctxr\r\r\n");
        public static OldResource SnaOssOutshould = new OldResource("sna_oss_outshould.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_outshould.bmp.ctxr,stage/r_XXXX/resident/sna_oss_outshould.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/0011c9b2.ctxr\r\r\n");
        public static OldResource SnaOssOshouldOvlSubAlp = new OldResource("sna_oss_oshould_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_oshould_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_oshould_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00ffec06.ctxr\r\r\n");
        public static OldResource SnaOssRthigh = new OldResource("sna_oss_rthigh.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_rthigh.bmp.ctxr,stage/r_XXXX/resident/sna_oss_rthigh.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00b973fe.ctxr\r\r\n");
        public static OldResource SnaOssRthighOvlSubAlp = new OldResource("sna_oss_rthigh_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_rthigh_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_rthigh_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00c04482.ctxr\r\r\n");
        public static OldResource SnaOssShould = new OldResource("sna_oss_should.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_should.bmp.ctxr,stage/r_XXXX/resident/sna_oss_should.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00fd249b.ctxr\r\r\n");
        public static OldResource SnaOssSpad = new OldResource("sna_oss_spad.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_spad.bmp.ctxr,stage/r_XXXX/resident/sna_oss_spad.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00ae1d74.ctxr\r\r\n");
        public static OldResource SnaOssSpadOvlSubAlp = new OldResource("sna_oss_spad_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_spad_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_spad_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/0057a3cd.ctxr\r\r\n");
        public static OldResource SnaOssVbelt = new OldResource("sna_oss_vbelt.bmp",
           kms: "", cmdl: "", tri: "",
           ctxr: "textures/flatlist/sna_oss_vbelt.bmp.ctxr,stage/r_XXXX/resident/sna_oss_vbelt.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00ecc009.ctxr\r\r\n");
        public static OldResource SnaOssWeast = new OldResource("sna_oss_weast.bmp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_weast.bmp.ctxr,stage/r_XXXX/resident/sna_oss_weast.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/00fe30e9.ctxr\r\r\n");
        public static OldResource SnaOssWeastOvlSubAlp = new OldResource("sna_oss_weast_ovl_sub_alp",
            kms: "", cmdl: "", tri: "",
            ctxr: "textures/flatlist/sna_oss_weast_ovl_sub_alp.bmp.ctxr,stage/r_XXXX/resident/sna_oss_weast_ovl_sub_alp.bmp.ctxr,eu/stage/r_XXXX/resident/007c2ac3/008ef8ce.ctxr\r\r\n");
        /*

        */
    }

    public class OldResource
    {
        public static List<OldResource> ResourceList = new List<OldResource>
        {
            MGS2Resource.ColdMedsLabel, MGS2Resource.ThermalGogglesLabel, MGS2Resource.GoggleIbox, MGS2Resource.GoggleSh, MGS2Resource.AKAmmoLabel,
            MGS2Resource.AKWeaponLabel, MGS2Resource.MagazineIbox, MGS2Resource.MagazineLabel, MGS2Resource.MagazineSh, MGS2Resource.C4Label,
            MGS2Resource.ClaymoreLabel, MGS2Resource.DmicLabel, MGS2Resource.GrenadeLabel, MGS2Resource.M4AmmoLabel, MGS2Resource.M4WeaponLabel,
            MGS2Resource.NikitaAmmoLabel, MGS2Resource.NikitaWeaponLabel, MGS2Resource.PSG1AmmoLabel, MGS2Resource.PSG1WeaponLabel, MGS2Resource.PSG1TAmmoLabel,
            MGS2Resource.PSG1TWeaponLabel, MGS2Resource.RGB6AmmoLabel, MGS2Resource.RGB6WeaponLabel, MGS2Resource.BodyArmorLabel, MGS2Resource.DigitalCameraIbox,
            MGS2Resource.DigitalCameraLabel, MGS2Resource.DigitalCameraSh, MGS2Resource.StingerAmmoLabel, MGS2Resource.StingerWeaponLabel, MGS2Resource.AKSuppressorLabel,
            MGS2Resource.PentazeminLabel, MGS2Resource.SensorBLabel, MGS2Resource.SocomSuppressorLabel, MGS2Resource.MineDetectorLabel, MGS2Resource.NVGLabel,
            MGS2Resource.RifleAmmoIbox, MGS2Resource.RifleAmmoSh, MGS2Resource.RifleIbox, MGS2Resource.RifleSh, MGS2Resource.LauncherAmmoIbox,
            MGS2Resource.LauncherAmmoSh, MGS2Resource.LauncherIbox, MGS2Resource.LauncherSh, MGS2Resource.Box2Ibox, MGS2Resource.Box2Sh,
            MGS2Resource.CbxLabel, MGS2Resource.DetectorIbox, MGS2Resource.DetectorSh, MGS2Resource.DmicIbox, MGS2Resource.DmicSh,
            MGS2Resource.RationIbox, MGS2Resource.M9AmmoLabel, MGS2Resource.M9WeaponLabel, MGS2Resource.StunLabel, MGS2Resource.BandageLabel,
            MGS2Resource.ShaverLabel, MGS2Resource.MedicineIbox, MGS2Resource.MedicineSh, MGS2Resource.HandgunIbox, MGS2Resource.HandgunSh,
            MGS2Resource.ChaffLabel, MGS2Resource.SocomAmmoLabel, MGS2Resource.GrenadeIbox, MGS2Resource.GrenadeSh, MGS2Resource.HandgunAmmoIbox,
            MGS2Resource.HandgunAmmoSh, MGS2Resource.RationSh, MGS2Resource.RationLabel, MGS2Resource.CoolantSprayLabel, MGS2Resource.SocomLabel,
            MGS2Resource.UspLabel, MGS2Resource.ScopeCustomBox, MGS2Resource.ItemBox, MGS2Resource.CigarettesIbox, MGS2Resource.CigarettesIboxSh,
            MGS2Resource.CigarettesLabel, MGS2Resource.SensorALabel, MGS2Resource.APSensorIbox, MGS2Resource.APSensorLabel,
            MGS2Resource.RaiBlade, MGS2Resource.HfbMt, MGS2Resource.HfbSubMt, MGS2Resource.HfbMineuchi, MGS2Resource.HfbMineuchiSub,
            MGS2Resource.HfbOvlMod, MGS2Resource.HfbSbGr2, MGS2Resource.HfbSbGrMk, MGS2Resource.Hfbs3SubAlpOvl, MGS2Resource.Hfbs3Sb,
            MGS2Resource.HfbTuba, MGS2Resource.HfbSoriGr2, MGS2Resource.HfbSubGr2AlpOvl, MGS2Resource.HfbSoliSaya, MGS2Resource.HfbSoliSaya2,
            MGS2Resource.HfbSoriGr, MGS2Resource.Hfbs2Ovl, MGS2Resource.HfbsGrtop, MGS2Resource.HfbsOvlMod, MGS2Resource.HfbsSbGr2
        };

        public static List<OldResource> HFBladeResourceList = new List<OldResource>
        {
            MGS2Resource.RaiBlade, MGS2Resource.HfbMt, MGS2Resource.HfbSubMt, MGS2Resource.HfbMineuchi, MGS2Resource.HfbMineuchiSub,
            MGS2Resource.HfbOvlMod, MGS2Resource.HfbSbGr2, MGS2Resource.HfbSbGrMk, MGS2Resource.Hfbs3SubAlpOvl, MGS2Resource.Hfbs3Sb,
            MGS2Resource.HfbTuba, MGS2Resource.HfbSoriGr2, MGS2Resource.HfbSubGr2AlpOvl, MGS2Resource.HfbSoliSaya, MGS2Resource.HfbSoliSaya2,
            MGS2Resource.HfbSoriGr, MGS2Resource.Hfbs2Ovl, MGS2Resource.HfbsGrtop, MGS2Resource.HfbsOvlMod, MGS2Resource.HfbsSbGr2
        };

        public static List<string> AllPlantWeaponItemResources = new List<string>()
        {
            "akammolabel", "akweaponlabel", "magazineibox", "magazinelabel", "magazinessh", "c4label", "chafflabel",
            "claymorelabel", "dmiclabel", "grenadelabel", "m4ammolabel", "m4weaponlabel", "m9ammolabel", "m9weaponlabel",
            "nikitaammolabel", "nikitaweaponlabel", "psg1ammolabel", "psg1weaponlabel", "psg1tammolabel", "psg1tweaponlabel", "rgb6ammolabel",
            "rgb6weaponlabel", "socomammolabel", "stingerammolabel", "stingerweaponlabel", "stunlabel", "aksuppressorlabel",
            "bandageslabel", "bodyarmorlabel", "rationibox", "rationsh", "rationlabel", "coldmedslabel", "digitalcamerabox",
            "digitalcameralabel", "digitalcamerash", "pentazeminlabel", "sensorblabel", "shaverlabel", "socomsuppressorlabel", "thermalgoggleslabel",
            "minedetectorlabel", "nvglabel", "rifleammoibox", "rifleammosh", "rifleibox", "riflesh",
            "launcherammoibox", "launcherammosh", "launcheribox", "launchersh", "grenadeibox", "grenadesh", "handgunammoibox",
            "handgunammosh", "medicineibox", "medicinesh", "handgunibox", "handgunsh", "box2ibox", "box2sh",
            "goggleibox", "gogglesh", "cboxlabel", "detectoribox", "detectorsh", "dmicibox", "dmicsh"
        };

        public static List<string> AllTankerWeaponItemResources = new List<string>()
        {
            "chafflabel", "grenadelabel", "m9ammolabel", "stunlabel", "uspammolabel", "bandageslabel",
            "rationibox", "rationsh", "rationlabel", "coldmedslabel", "pentazeminlabel", "thermalgoggleslabel", "uspsuppressorlabel", "grenadeibox",
            "grenadesh", "handgunammoibox", "handgunammosh", "medicineibox", "medicinesh", "box2ibox", "box2sh", "goggleibox", "gogglesh", "cboxlabel"
        };

        public string CommonName { get; set; }
        public string Kms { get; set; }
        public string Cmdl { get; set; }
        public string Ctxr { get; set; }
        public string Tri { get; set; }
        public string Sar { get; set; }
        public string Mar { get; set; }

        public OldResource(string name, string kms, string cmdl, string ctxr, string tri, string sar = null, string mar = null)
        {
            CommonName = name;
            Kms = kms;
            Cmdl = cmdl;
            Ctxr = ctxr;
            Tri = tri;
            Sar = sar;
            Mar = mar;
        }

        public static OldResource LookupResource(string name)
        {
            return ResourceList.Find(x => x.CommonName == name.ToLower());
        }
    }
}
