using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public static class ResourceParser
    {
        public static Resource ParseResource(string resourceText)
        {
            int firstComma = resourceText.IndexOf(',');
            int lastSlashBeforeFirstComma = resourceText.Substring(0, firstComma).LastIndexOf("/");
            string name = resourceText.Substring(lastSlashBeforeFirstComma + 1, firstComma - lastSlashBeforeFirstComma - 1).Trim();
            int lastPeriod = resourceText.LastIndexOf(".");
            int lastSlash = resourceText.LastIndexOf("/");
            string hash = resourceText.Substring(lastSlash + 1, lastPeriod - lastSlash - 1).Trim();
            int stageIndex = resourceText.LastIndexOf("/stage/");
            int cacheIndex = resourceText.LastIndexOf("/cache/");
            int residentIndex = resourceText.LastIndexOf("/resident/");
            string stage;
            if(residentIndex != -1)
            {
                stage = resourceText.Substring(stageIndex + 7, residentIndex - stageIndex - 7).Trim();
            }
            else 
            {
                stage = resourceText.Substring(stageIndex + 7, cacheIndex - stageIndex - 7).Trim();
            }

            if (resourceText.EndsWith("ctxr"))
            {
                return new Ctxr(name, hash, stage, resourceText);
            }
            else if (resourceText.EndsWith("tri"))
            {
                return new Tri(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("zms"))
            {
                return new Zms(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("var"))
            {
                return new Var(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("sar"))
            {
                return new Sar(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("row"))
            {
                return new Row(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("o2d"))
            {
                return new O2d(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("mar"))
            {
                return new Mar(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("kms"))
            {
                return new Kms(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("evm"))
            {
                return new Evm(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("cv2"))
            {
                return new Cv2(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("hzx"))
            {
                return new Hzx(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("lt2"))
            {
                return new Lt2(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("far"))
            {
                return new Far(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("anm"))
            {
                return new Anm(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("gcx"))
            {
                return new Gcx(name, hash, stage, resourceText);
            }
            else if(resourceText.EndsWith("cmdl"))
            {
                SubType subType;
                if (resourceText.Contains("/kms/"))
                {
                    subType = SubType.Kms;
                }
                else if (resourceText.Contains("/evm/"))
                {
                    subType = SubType.Evm;
                }
                else if (resourceText.Contains("/zms/"))
                {
                    subType = SubType.Zms;
                }
                else
                {
                    throw new Exception("Unknown subtype for cmdl!");
                }
                return new Cmdl(name, hash, stage, resourceText, subType);
            }
            else
            {
                throw new Exception("Unknown resource type!");
            }
        }
    }
}
