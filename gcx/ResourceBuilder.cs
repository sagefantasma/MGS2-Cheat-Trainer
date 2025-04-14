using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public static class ResourceBuilder
    {
        public static void BuildResources(List<Resource> bpAssetsResources, List<Resource> manifestResources, string outputFolder = "")
        {
            string bpAssetsContents = string.Empty;

            foreach (Resource bpAsset in bpAssetsResources)
            {
                bpAssetsContents += bpAsset.FullText + "\r\r\n";
            }

            string manifestContents = string.Empty;

            foreach (Resource manifestAsset in manifestResources)
            {
                manifestContents += manifestAsset.FullText + "\r\r\n";
            }

            string bpAssetsFile = outputFolder + "bp_assets.txt";
            File.WriteAllText(bpAssetsFile, bpAssetsContents);
            string manifestFile = outputFolder + "manifest.txt";
            File.WriteAllText(manifestFile, manifestContents);
        }
    }
}
