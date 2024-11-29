using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public struct MGS2Resource
    {
        public static Resource ColdMeds = new Resource("ColdMeds", 
            kms: "assets/kms/us/cold_medicine_label_stage_a03b.kms,us/stage/w00a/cache/00f971fb.kms,cache/00f971fb.kms\r\r\n", 
            cmdl: "assets/kms/us/cold_medicine_label_stage_a03b.cmdl,us/stage/w00a/cache/00f971fb.cmdl,eu/stage/w00a/cache/00f971fb.cmdl\r\r\n",
            ctxr: "textures/flatlist/coldmedicine_tx_alp.bmp.ctxr,stage/w00a/cache/coldmedicine_tx_alp.bmp.ctxr,eu/stage/w00a/cache/00573de0/00ed17f6.ctxr\r\r\n");
    }

    public class Resource
    {
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
            return MGS2Resource.ColdMeds;
            //TODO: the rest of the fucking owl

            Resource resource = null;

            return resource;
        }
    }
}
