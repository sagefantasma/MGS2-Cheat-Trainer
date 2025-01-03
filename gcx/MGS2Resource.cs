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
        public static Resource ColdMedsLabel = new Resource("cold_medicine_label", 
            kms: "assets/kms/us/cold_medicine_label_stage_a03b.kms,us/stage/XXXX/cache/00f971fb.kms,cache/00f971fb.kms\r\r\n", 
            cmdl: "assets/kms/us/cold_medicine_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f971fb.cmdl,eu/stage/XXXX/cache/00f971fb.cmdl\r\r\n",
            ctxr: "textures/flatlist/coldmedicine_tx_alp.bmp.ctxr,stage/XXXX/cache/coldmedicine_tx_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/00ed17f6.ctxr\r\r\n", tri: ""); 
        public static Resource ThermalGogglesLabel = new Resource("tgl_label", 
            kms: "assets/kms/us/tgl_label_stage_a03b.kms,us/stage/XXXX/cache/006968d1.kms,cache/006968d1.kms\r\r\n",
            cmdl: "assets/kms/us/tgl_label_stage_a03b.cmdl,us/stage/XXXX/cache/006968d1.cmdl,eu/stage/XXXX/cache/006968d1.cmdl\r\r\n",
            ctxr: "", tri: ""); 
        public static Resource GoggleIbox = new Resource("goggle_ibox_stage", 
            kms: "assets/kms/us/goggle_ibox_stage_a00c.kms,us/stage/XXXX/cache/00706bd2.kms,cache/00706bd2.kms\r\r\n",
            cmdl: "assets/kms/us/goggle_ibox_stage_a00c.cmdl,us/stage/XXXX/cache/00706bd2.cmdl,eu/stage/XXXX/cache/00706bd2.cmdl\r\r\n",
            ctxr: "textures/flatlist/ibox_all4.bmp.ctxr,stage/XXXX/cache/ibox_all4.bmp.ctxr,eu/stage/XXXX/cache/00706bd2/008e6a6a.ctxr\r\r\n",
            tri: "assets/tri/us/goggle_ibox.tri,us/stage/XXXX/cache/00706bd2.tri,cache/00706bd2.tri\r\r\n"); 
        public static Resource GoggleSh = new Resource("goggle_ibox_sh", 
            kms: "assets/kms/us/goggle_ibox_sh_stage_a00c.kms,us/stage/XXXX/cache/00eac2fd.kms,cache/00eac2fd.kms\r\r\n",
            cmdl: "assets/kms/us/goggle_ibox_sh_stage_a00c.cmdl,us/stage/XXXX/cache/00eac2fd.cmdl,eu/stage/XXXX/cache/00eac2fd.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource AKAmmoLabel = new Resource("aks_amo_label", 
            kms: "assets/kms/us/aks_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003ce0e9.kms,cache/003ce0e9.kms\r\r\n",
            cmdl: "assets/kms/us/aks_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003ce0e9.cmdl,eu/stage/XXXX/cache/003ce0e9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource AKWeaponLabel = new Resource("ak_label", 
            kms: "assets/kms/us/ak_label_stage_a03b.kms,us/stage/XXXX/cache/00f53890.kms,cache/00f53890.kms\r\r\n",
            cmdl: "assets/kms/us/ak_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f53890.cmdl,eu/stage/XXXX/cache/00f53890.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource MagazineIbox = new Resource("magazine_ibox", 
            kms: "assets/kms/us/magazine_ibox_stage_a03b.kms,us/stage/XXXX/cache/00ca7cd0.kms,cache/00ca7cd0.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00ca7cd0.cmdl,eu/stage/XXXX/cache/00ca7cd0.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource MagazineLabel = new Resource("magazine_label", 
            kms: "assets/kms/us/magazine_label_stage_a03b.kms,us/stage/XXXX/cache/007ee425.kms,cache/007ee425.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_label_stage_a03b.cmdl,us/stage/XXXX/cache/007ee425.cmdl,eu/stage/XXXX/cache/007ee425.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource MagazineSh = new Resource("magazine_sh", 
            kms: "assets/kms/us/magazine_sh_stage_a03b.kms,us/stage/XXXX/cache/009e33e1.kms,cache/009e33e1.kms\r\r\n",
            cmdl: "assets/kms/us/magazine_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009e33e1.cmdl,eu/stage/XXXX/cache/009e33e1.cmdl\r\r\n",
            ctxr: "", tri: ""   );
        public static Resource C4Label = new Resource("cfr_label", 
            kms: "assets/kms/us/cfr_label_stage_a03b.kms,us/stage/XXXX/cache/00586251.kms,cache/00586251.kms\r\r\n",
            cmdl: "assets/kms/us/cfr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00586251.cmdl,eu/stage/XXXX/cache/00586251.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource ClaymoreLabel = new Resource("clm_label",
            kms: "assets/kms/us/clm_label_stage_a03b.kms,us/stage/XXXX/cache/00589111.kms,cache/00589111.kms\r\r\n",
            cmdl: "assets/kms/us/clm_label_stage_a03b.cmdl,us/stage/XXXX/cache/00589111.cmdl,eu/stage/XXXX/cache/00589111.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DmicLabel = new Resource("dmp_label",
            kms: "assets/kms/us/dmp_label_stage_a03b.kms,us/stage/XXXX/cache/005999d1.kms,cache/005999d1.kms\r\r\n",
            cmdl: "assets/kms/us/dmp_label_stage_a03b.cmdl,us/stage/XXXX/cache/005999d1.cmdl,eu/stage/XXXX/cache/005999d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource GrenadeLabel = new Resource("gre_label",
            kms: "assets/kms/us/gre_label_stage_a03b.kms,us/stage/XXXX/cache/005cbf11.kms,cache/005cbf11.kms\r\r\n",
            cmdl: "assets/kms/us/gre_label_stage_a03b.cmdl,us/stage/XXXX/cache/005cbf11.cmdl,eu/stage/XXXX/cache/005cbf11.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource M4AmmoLabel = new Resource("m4_amo_label",
            kms: "assets/kms/us/m4_amo_label_stage_a03b.kms,us/stage/XXXX/cache/0036d0ed.kms,cache/0036d0ed.kms\r\r\n",
            cmdl: "assets/kms/us/m4_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/0036d0ed.cmdl,eu/stage/XXXX/cache/0036d0ed.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource M4WeaponLabel = new Resource("m4_label",
            kms: "assets/kms/us/m4_label_stage_a03b.kms,us/stage/XXXX/cache/00f58ad0.kms,cache/00f58ad0.kms\r\r\n",
            cmdl: "assets/kms/us/m4_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f58ad0.cmdl,eu/stage/XXXX/cache/00f58ad0.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource NikitaAmmoLabel = new Resource("nkt_amo_label",
            kms: "assets/kms/us/nkt_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003db0ed.kms,cache/003db0ed.kms\r\r\n",
            cmdl: "assets/kms/us/nkt_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003db0ed.cmdl,eu/stage/XXXX/cache/003db0ed.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource NikitaWeaponLabel = new Resource("nkt_label",
            kms: "assets/kms/us/nkt_label_stage_a03b.kms,us/stage/XXXX/cache/00638ad1.kms,cache/00638ad1.kms\r\r\n",
            cmdl: "assets/kms/us/nkt_label_stage_a03b.cmdl,us/stage/XXXX/cache/00638ad1.cmdl,eu/stage/XXXX/cache/00638ad1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource PSG1AmmoLabel = new Resource("psg_amo_label",
            kms: "assets/kms/us/psg_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003dd4b9.kms,cache/003dd4b9.kms\r\r\n",
            cmdl: "assets/kms/us/psg_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003dd4b9.cmdl,eu/stage/XXXX/cache/003dd4b9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource PSG1WeaponLabel = new Resource("psg_label",
            kms: "assets/kms/us/psg_label_stage_a03b.kms,us/stage/XXXX/cache/0065c791.kms,cache/0065c791.kms\r\r\n",
            cmdl: "assets/kms/us/psg_label_stage_a03b.cmdl,us/stage/XXXX/cache/0065c791.cmdl,eu/stage/XXXX/cache/0065c791.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource PSG1TAmmoLabel = new Resource("psgt_amo_label",
            kms: "assets/kms/us/psgt_amo_label_stage_a03b.kms,us/stage/XXXX/cache/001dfe6e.kms,cache/001dfe6e.kms\r\r\n",
            cmdl: "assets/kms/us/psgt_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/001dfe6e.cmdl,eu/stage/XXXX/cache/001dfe6e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource PSG1TWeaponLabel = new Resource("psg_t_label",
            kms: "assets/kms/us/psg_t_label_stage_a03b.kms,us/stage/XXXX/cache/00bc2c9f.kms,cache/00bc2c9f.kms\r\r\n",
            cmdl: "assets/kms/us/psg_t_label_stage_a03b.cmdl,us/stage/XXXX/cache/00bc2c9f.cmdl,eu/stage/XXXX/cache/00bc2c9f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RGB6AmmoLabel = new Resource("rgb_amo_label",
            kms: "assets/kms/us/rgb_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003deea5.kms,cache/003deea5.kms\r\r\n",
            cmdl: "assets/kms/us/rgb_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003deea5.cmdl,eu/stage/XXXX/cache/003deea5.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RGB6WeaponLabel = new Resource("rgb_label",
            kms: "assets/kms/us/rgb_label_stage_a03b.kms,us/stage/XXXX/cache/00676651.kms,cache/00676651.kms\r\r\n",
            cmdl: "assets/kms/us/rgb_label_stage_a03b.cmdl,us/stage/XXXX/cache/00676651.cmdl,eu/stage/XXXX/cache/00676651.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource StingerAmmoLabel = new Resource("stg_amo_label",
            kms: "assets/kms/us/stg_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003e0539.kms,cache/003e0539.kms\r\r\n",
            cmdl: "assets/kms/us/stg_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003e0539.cmdl,eu/stage/XXXX/cache/003e0539.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource StingerWeaponLabel = new Resource("stg_label",
            kms: "assets/kms/us/stg_label_stage_a03b.kms,us/stage/XXXX/cache/0068cf91.kms,cache/0068cf91.kms\r\r\n",
            cmdl: "assets/kms/us/stg_label_stage_a03b.cmdl,us/stage/XXXX/cache/0068cf91.cmdl,eu/stage/XXXX/cache/0068cf91.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource BodyArmorLabel = new Resource("bam_label",
            kms: "assets/kms/us/bam_label_stage_a03b.kms,us/stage/XXXX/cache/00573911.kms,cache/00573911.kms\r\r\n",
            cmdl: "assets/kms/us/bam_label_stage_a03b.cmdl,us/stage/XXXX/cache/00573911.cmdl,eu/stage/XXXX/cache/00573911.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DigitalCameraIbox = new Resource("digital_camera_ibox",
            kms: "assets/kms/us/digital_camera_ibox_stage_a03b.kms,us/stage/XXXX/cache/001cd720.kms,cache/001cd720.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/001cd720.cmdl,eu/stage/XXXX/cache/001cd720.cmdl\r\r\n",
            ctxr: "textures/flatlist/degital_camera.bmp.ctxr,stage/XXXX/cache/degital_camera.bmp.ctxr,eu/stage/XXXX/cache/00883186/00fb2060.ctxr\r\r\n",
            tri: "");
        public static Resource DigitalCameraLabel = new Resource("digital_camera_label",
            kms: "assets/kms/us/digital_camera_label_stage_a03b.kms,us/stage/XXXX/cache/00ca2e0f.kms,cache/00ca2e0f.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_label_stage_a03b.cmdl,us/stage/XXXX/cache/00ca2e0f.cmdl,eu/stage/XXXX/cache/00ca2e0f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DigitalCameraSh = new Resource("digital_camera_sh",
            kms: "assets/kms/us/digital_camera_sh_stage_a03b.kms,us/stage/XXXX/cache/00320878.kms,cache/00320878.kms\r\r\n",
            cmdl: "assets/kms/us/digital_camera_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00320878.cmdl,eu/stage/XXXX/cache/00320878.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource PentazeminLabel = new Resource("dzp2_label",
            kms: "assets/kms/us/dzp2_label_stage_a03b.kms,us/stage/XXXX/cache/00efa25d.kms,cache/00efa25d.kms\r\r\n",
            cmdl: "assets/kms/us/dzp2_label_stage_a03b.cmdl,us/stage/XXXX/cache/00efa25d.cmdl,eu/stage/XXXX/cache/00efa25d.cmdl\r\r\n",
            ctxr: "textures/flatlist/dzp2_tx_alp.bmp.ctxr,stage/XXXX/cache/dzp2_tx_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/0062bc3f.ctxr\r\r\n",
            tri: "");
        public static Resource SensorBLabel = new Resource("bsn_b_label",
            kms: "assets/kms/us/bsn_b_label_stage_a03b.kms,us/stage/XXXX/cache/00c327e7.kms,cache/00c327e7.kms\r\r\n",
            cmdl: "assets/kms/us/bsn_b_label_stage_a03b.cmdl,us/stage/XXXX/cache/00c327e7.cmdl,eu/stage/XXXX/cache/00c327e7.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource SocomSuppressorLabel = new Resource("scm_sp_label",
            kms: "assets/kms/us/scm_sp_label_stage_a03b.kms,us/stage/XXXX/cache/00f504ea.kms,cache/00f504ea.kms\r\r\n",
            cmdl: "assets/kms/us/scm_sp_label_stage_a03b.cmdl,us/stage/XXXX/cache/00f504ea.cmdl,eu/stage/XXXX/cache/00f504ea.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource MineDetectorLabel = new Resource("mnd_label",
            kms: "assets/kms/us/mnd_label_stage_a03b.kms,us/stage/XXXX/cache/00629ed1.kms,cache/00629ed1.kms\r\r\n",
            cmdl: "assets/kms/us/mnd_label_stage_a03b.cmdl,us/stage/XXXX/cache/00629ed1.cmdl,eu/stage/XXXX/cache/00629ed1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource NVGLabel = new Resource("ngl_label",
            kms: "assets/kms/us/ngl_label_stage_a03b.kms,us/stage/XXXX/cache/006368d1.kms,cache/006368d1.kms\r\r\n",
            cmdl: "assets/kms/us/ngl_label_stage_a03b.cmdl,us/stage/XXXX/cache/006368d1.cmdl,eu/stage/XXXX/cache/006368d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RifleAmmoIbox = new Resource("rifle_amo_ibox",
            kms: "assets/kms/us/rifle_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/0012ff3a.kms,cache/0012ff3a.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0012ff3a.cmdl,eu/stage/XXXX/cache/0012ff3a.cmdl\r\r\n",
            ctxr: "", tri: ""); //TODO: there are at least 3 of these referenced in w22a. what is going on with this
        public static Resource RifleAmmoSh = new Resource("handgun_amo_ibox",
            kms: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/009e9447.kms,cache/009e9447.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009e9447.cmdl,eu/stage/XXXX/cache/009e9447.cmdl\r\r\n",
            ctxr: "", tri: ""); //This is NOT a mistake on my part, this is Konami/Bluepoint's doing. This is correct to the game files
        public static Resource RifleIbox = new Resource("rifle_ibox_stage",
            kms: "assets/kms/us/rifle_ibox_stage_a03b.kms,us/stage/XXXX/cache/00d26236.kms,cache/00d26236.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00d26236.cmdl,eu/stage/XXXX/cache/00d26236.cmdl\r\r\n",
            ctxr: "textures/flatlist/rifle_ibox.bmp_94bca08db682d231ae1d48d2b6385598.ctxr,stage/XXXX/cache/rifle_ibox.bmp_94bca08db682d231ae1d48d2b6385598.ctxr,eu/stage/XXXX/cache/00883186/00d26236.ctxr\r\r\n",
            tri: "");
        public static Resource RifleSh = new Resource("rifle_ibox_sh",
            kms: "assets/kms/us/rifle_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/001cf3f9.kms,cache/001cf3f9.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/001cf3f9.cmdl,eu/stage/XXXX/cache/001cf3f9.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource LauncherAmmoIbox = new Resource("launcher_amo_ibox_stage",
            kms: "assets/kms/us/launcher_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/00eb0f44.kms,cache/00eb0f44.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00eb0f44.cmdl,eu/stage/XXXX/cache/00eb0f44.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource LauncherAmmoSh = new Resource("launcher_amo_ibox_sh",
            kms: "assets/kms/us/launcher_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00a4004f.kms,cache/00a4004f.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00a4004f.cmdl,eu/stage/XXXX/cache/00a4004f.cmdl\r\r\n",
            ctxr: "textures/flatlist/launcher_ammo_side.bmp.ctxr,stage/XXXX/cache/launcher_ammo_side.bmp.ctxr,eu/stage/XXXX/cache/00883186/00ca4e21.ctxr", tri: "");
        public static Resource LauncherIbox = new Resource("launcher_ibox_stage",
            kms: "assets/kms/us/launcher_ibox_stage_a03b.kms,us/stage/XXXX/cache/005362e4.kms,cache/005362e4.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/005362e4.cmdl,eu/stage/XXXX/cache/005362e4.cmdl\r\r\n",
            ctxr: "textures/flatlist/launcher_ibox.bmp.ctxr,stage/XXXX/cache/launcher_ibox.bmp.ctxr,eu/stage/XXXX/cache/00883186/005362e4.ctxr\r\r\n",
            tri: "");
        public static Resource LauncherSh = new Resource("launcher_ibox_sh",
            kms: "assets/kms/us/launcher_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0073b479.kms,cache/0073b479.kms\r\r\n",
            cmdl: "assets/kms/us/launcher_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0073b479.cmdl,eu/stage/XXXX/cache/0073b479.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource Box2Ibox = new Resource("box2_ibox",
            kms: "assets/kms/us/box2_ibox_stage_a03b.kms,us/stage/XXXX/cache/008bacc2.kms,cache/008bacc2.kms\r\r\n",
            cmdl: "assets/kms/us/box2_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/008bacc2.cmdl,eu/stage/XXXX/cache/008bacc2.cmdl\r\r\n",
            ctxr: "textures/flatlist/ibox2_tx_all_alp.bmp.ctxr,stage/XXXX/cache/ibox2_tx_all_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/0051c0fb.ctxr\r\r\n",
            tri: "");
        public static Resource Box2Sh = new Resource("box2_ibox_sh",
            kms: "assets/kms/us/box2_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0062d09e.kms,cache/0062d09e.kms\r\r\n",
            cmdl: "assets/kms/us/box2_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0062d09e.cmdl,eu/stage/XXXX/cache/0062d09e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource CbxLabel = new Resource("cbx_label",
            kms: "assets/kms/us/cbx_label_stage_a03b.kms,us/stage/XXXX/cache/005843d1.kms,cache/005843d1.kms\r\r\n",
            cmdl: "assets/kms/us/cbx_label_stage_a03b.cmdl,us/stage/XXXX/cache/005843d1.cmdl,eu/stage/XXXX/cache/005843d1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DetectorIbox = new Resource("detector_ibox_stage",
            kms: "assets/kms/us/detector_ibox_stage_a03b.kms,us/stage/XXXX/cache/0033475f.kms,cache/0033475f.kms\r\r\n",
            cmdl: "assets/kms/us/detector_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0033475f.cmdl,eu/stage/XXXX/cache/0033475f.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DetectorSh = new Resource("detector_ibox_sh",
            kms: "assets/kms/us/detector_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00b1246b.kms,cache/00b1246b.kms\r\r\n",
            cmdl: "assets/kms/us/detector_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00b1246b.cmdl,eu/stage/XXXX/cache/00b1246b.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource DmicIbox = new Resource("box_ibox_stage",
            kms: "assets/kms/us/box_ibox_stage_a03b.kms,us/stage/XXXX/cache/0029430e.kms,cache/0029430e.kms\r\r\n",
            cmdl: "assets/kms/us/box_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/0029430e.cmdl,eu/stage/XXXX/cache/0029430e.cmdl\r\r\n",
            ctxr: "textures/flatlist/dmp_ibx_label_alp.bmp.ctxr,stage/XXXX/cache/dmp_ibx_label_alp.bmp.ctxr,eu/stage/XXXX/cache/00573de0/00055db2.ctxr\r\r\n",
            tri: "");
        public static Resource DmicSh = new Resource("box_ibox_sh",
            kms: "assets/kms/us/box_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00889f69.kms,cache/00889f69.kms\r\r\n",
            cmdl: "assets/kms/us/box_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00889f69.cmdl,eu/stage/XXXX/cache/00889f69.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RationIbox = new Resource("ration_ibox_stage",
            kms: "assets/kms/us/ration_ibox_stage_a03b.kms,us/stage/XXXX/cache/00bd7cce.kms,cache/00bd7cce.kms\r\r\n",
            cmdl: "assets/kms/us/ration_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00bd7cce.cmdl,eu/stage/XXXX/cache/00bd7cce.cmdl\r\r\n",
            ctxr: "textures/flatlist/ration_box.bmp.ctxr,stage/XXXX/cache/ration_box.bmp.ctxr,eu/stage/XXXX/cache/00883186/00b5d18b.ctxr", tri: "");
        public static Resource M9AmmoLabel = new Resource("m92_amo_label",
            kms: "assets/kms/us/m92_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003d86e5.kms,cache/003d86e5.kms\r\r\n",
            cmdl: "assets/kms/us/m92_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003d86e5.cmdl,eu/stage/XXXX/cache/003d86e5.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource M9WeaponLabel = new Resource("m92_label",
            kms: "assets/kms/us/m92_label_stage_a03b.kms,us/stage/XXXX/cache/0060ea51.kms,cache/0060ea51.kms\r\r\n",
            cmdl: "assets/kms/us/m92_label_stage_a03b.cmdl,us/stage/XXXX/cache/0060ea51.cmdl,eu/stage/XXXX/cache/0060ea51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource StunLabel = new Resource("sgr_label",
            kms: "assets/kms/us/sgr_label_stage_a03b.kms,us/stage/XXXX/cache/00686a51.kms,cache/00686a51.kms\r\r\n",
            cmdl: "assets/kms/us/sgr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00686a51.cmdl,eu/stage/XXXX/cache/00686a51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource BandageLabel = new Resource("sbs_label",
            kms: "assets/kms/us/sbs_label_stage_a03b.kms,us/stage/XXXX/cache/00684291.kms,cache/00684291.kms\r\r\n",
            cmdl: "assets/kms/us/sbs_label_stage_a03b.cmdl,us/stage/XXXX/cache/00684291.cmdl,eu/stage/XXXX/cache/00684291.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource ShaverLabel = new Resource("shv_label",
            kms: "assets/kms/us/shv_label_stage_a03b.kms,us/stage/XXXX/cache/00687351.kms,cache/00687351.kms\r\r\n",
            cmdl: "assets/kms/us/shv_label_stage_a03b.cmdl,us/stage/XXXX/cache/00687351.cmdl,eu/stage/XXXX/cache/00687351.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource MedicineIbox = new Resource("medicine_ibox_stage",
            kms: "assets/kms/us/medicine_ibox_stage_a03b.kms,us/stage/XXXX/cache/00b37ec5.kms,cache/00b37ec5.kms\r\r\n",
            cmdl: "assets/kms/us/medicine_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00b37ec5.cmdl,eu/stage/XXXX/cache/00b37ec5.cmdl\r\r\n",
            ctxr: "textures/flatlist/medicine_box.bmp.ctxr,stage/XXXX/cache/medicine_box.bmp.ctxr,eu/stage/XXXX/cache/00883186/006d819b.ctxr\r\r\n", tri: "");
        public static Resource MedicineSh = new Resource("medicine_ibox_sh",
            kms: "assets/kms/us/medicine_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00646487.kms,cache/00646487.kms\r\r\n",
            cmdl: "assets/kms/us/medicine_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00646487.cmdl,eu/stage/XXXX/cache/00646487.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource HandgunIbox = new Resource("handgun_ibox_stage",
            kms: "assets/kms/us/handgun_ibox_stage_a03b.kms,us/stage/XXXX/cache/004da20c.kms,cache/004da20c.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/004da20c.cmdl,eu/stage/XXXX/cache/004da20c.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource HandgunSh = new Resource("handgun_ibox_sh",
            kms: "assets/kms/us/handgun_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0007b199.kms,cache/0007b199.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0007b199.cmdl,eu/stage/XXXX/cache/0007b199.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource ChaffLabel = new Resource("cgr_label",
            kms: "assets/kms/us/cgr_label_stage_a03b.kms,us/stage/XXXX/cache/00586a51.kms,cache/00586a51.kms\r\r\n",
            cmdl: "assets/kms/us/cgr_label_stage_a03b.cmdl,us/stage/XXXX/cache/00586a51.cmdl,eu/stage/XXXX/cache/00586a51.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource SocomAmmoLabel = new Resource("scm_amo_label",
            kms: "assets/kms/us/scm_amo_label_stage_a03b.kms,us/stage/XXXX/cache/003dfcd1.kms,cache/003dfcd1.kms\r\r\n",
            cmdl: "assets/kms/us/scm_amo_label_stage_a03b.cmdl,us/stage/XXXX/cache/003dfcd1.cmdl,eu/stage/XXXX/cache/003dfcd1.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource GrenadeIbox = new Resource("grenade_ibox_stage",
            kms: "assets/kms/us/grenade_ibox_stage_a03b.kms,us/stage/XXXX/cache/00376d7d.kms,cache/00376d7d.kms\r\r\n",
            cmdl: "assets/kms/us/grenade_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/00376d7d.cmdl,eu/stage/XXXX/cache/00376d7d.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource GrenadeSh = new Resource("grenade_ibox_sh",
            kms: "assets/kms/us/grenade_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/00c0267e.kms,cache/00c0267e.kms\r\r\n",
            cmdl: "assets/kms/us/grenade_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/00c0267e.cmdl,eu/stage/XXXX/cache/00c0267e.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource HandgunAmmoIbox = new Resource("rifle_amo_ibox_stage",
            kms: "assets/kms/us/rifle_amo_ibox_stage_a03b.kms,us/stage/XXXX/cache/006ab337.kms,cache/006ab337.kms\r\r\n",
            cmdl: "assets/kms/us/rifle_amo_ibox_stage_a03b.cmdl,us/stage/XXXX/cache/006ab337.cmdl,eu/stage/XXXX/cache/006ab337.cmdl\r\r\n",
            ctxr: "", tri: ""); //again, this is not a mistake on our part. This was done by Konami/Bluepoint
        public static Resource HandgunAmmoSh = new Resource("handgun_amo_ibox_sh",
            kms: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/009d4021.kms,cache/009d4021.kms\r\r\n",
            cmdl: "assets/kms/us/handgun_amo_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/009d4021.cmdl,eu/stage/XXXX/cache/009d4021.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RationSh = new Resource("ration_ibox_sh",
            kms: "assets/kms/us/ration_ibox_sh_stage_a03b.kms,us/stage/XXXX/cache/0068e986.kms,cache/0068e986.kms\r\r\n",
            cmdl: "assets/kms/us/ration_ibox_sh_stage_a03b.cmdl,us/stage/XXXX/cache/0068e986.cmdl,eu/stage/XXXX/cache/0068e986.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource RationLabel = new Resource("rtn_label",
            kms: "assets/kms/us/rtn_label_stage_a03b.kms,us/stage/XXXX/cache/0067d151.kms,cache/0067d151.kms\r\r\n",
            cmdl: "assets/kms/us/rtn_label_stage_a03b.cmdl,us/stage/XXXX/cache/0067d151.cmdl,eu/stage/XXXX/cache/0067d151.cmdl\r\r\n",
            ctxr: "", tri: "");
        public static Resource AKSuppressorLabel = new Resource("ak_sp_label",
            kms: "assets/kms/us/ak_sp_label_stage_a03b.kms,us/stage/XXXX/cache/00b4cb62.kms,cache/00b4cb62.kms\r\r\n",
            cmdl: "assets/kms/us/ak_sp_label_stage_a03b.cmdl,us/stage/XXXX/cache/00b4cb62.cmdl,eu/stage/XXXX/cache/00b4cb62.cmdl\r\r\n",
            ctxr: "", tri: "");
    }

    public class Resource
    {
        public static List<Resource> ResourceList = new List<Resource>
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
            MGS2Resource.HandgunAmmoSh, MGS2Resource.RationSh, MGS2Resource.RationLabel
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

        public Resource(string name, string kms, string cmdl, string ctxr, string tri)
        {
            CommonName = name;
            Kms = kms;
            Cmdl = cmdl;
            Ctxr = ctxr;
            Tri = tri;
        }

        public static Resource LookupResource(string name)
        {
            return ResourceList.Find(x => x.CommonName == name.ToLower());
        }
    }
}
