using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public struct MGS2Resource
    {
        public static Resource ColdMeds = new Resource("coldmeds", //00f971fb
            kms: "assets/kms/us/cold_medicine_label_stage_a03b.kms,us/stage/w00a/cache/00f971fb.kms,cache/00f971fb.kms\r\r\n", 
            cmdl: "assets/kms/us/cold_medicine_label_stage_a03b.cmdl,us/stage/w00a/cache/00f971fb.cmdl,eu/stage/w00a/cache/00f971fb.cmdl\r\r\n",
            ctxr: "textures/flatlist/coldmedicine_tx_alp.bmp.ctxr,stage/w00a/cache/coldmedicine_tx_alp.bmp.ctxr,eu/stage/w00a/cache/00573de0/00ed17f6.ctxr\r\r\n"); 
        public static Resource ThermalGoggles = new Resource("thermalgoggles", //006968d1
            kms: "assets/kms/us/tgl_label_stage_a03b.kms,us/stage/w00a/cache/006968d1.kms,cache/006968d1.kms\r\r\n",
            cmdl: "assets/kms/us/tgl_label_stage_a03b.cmdl,us/stage/w00a/cache/006968d1.cmdl,eu/stage/w00a/cache/006968d1.cmdl\r\r\n",
            ctxr: ""); 
        public static Resource GoggleIbox = new Resource("goggleibox", //00706bd2
            kms: "assets/kms/us/goggle_ibox_stage_a03b.kms,us/stage/w00a/cache/00706bd2.kms,cache/00706bd2.kms\r\r\n",
            cmdl: "assets/kms/us/goggle_ibox_stage_a03b.cmdl,us/stage/w00a/cache/00706bd2.cmdl,eu/stage/w00a/cache/00706bd2.cmdl\r\r\n",
            ctxr: ""); 
        public static Resource GoggleSh = new Resource("gogglesh", //00eac2fd
            kms: "assets/kms/us/cartridge_amo_ibox_sh.kms,us/stage/w00a/cache/00eac2fd.kms,cache/00eac2fd.kms\r\r\n",
            cmdl: "assets/kms/us/cartridge_amo_ibox_sh.cmdl,us/stage/w00a/cache/00eac2fd.cmdl,eu/stage/w00a/cache/00eac2fd.cmdl\r\r\n",
            ctxr: ""); 
    }

    public class Resource
    {
        private static List<Resource> ResourceList = new List<Resource>
        {
            MGS2Resource.ColdMeds, MGS2Resource.ThermalGoggles, MGS2Resource.GoggleIbox, MGS2Resource.GoggleSh
        };

        string CommonName { get; set; }
        public string Kms { get; set; }
        public string Cmdl { get; set; }
        public string Ctxr { get; set; }

        public Resource(string name, string kms, string cmdl, string ctxr)
        {
            CommonName = name;
            Kms = kms;
            Cmdl = cmdl;
            Ctxr = ctxr;
        }

        public static Resource LookupResource(string name)
        {
            return ResourceList.Find(x => x.CommonName == name.ToLower());
        }
    }
}
