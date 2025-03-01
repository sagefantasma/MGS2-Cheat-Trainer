using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class ResourceExtractor
    {
        string bpAssetsLocation { get; set; }
        string manifestLocation { get; set; }
        public List<string> bpAssetsResources { get; private set; }
        public List<string> manifestResources { get; private set; }

        public ResourceExtractor(string assetDirectory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(assetDirectory);
            List<FileInfo> files = directoryInfo.EnumerateFiles().ToList();
            bpAssetsLocation = files.Where(x => x.Name.Contains("bp_assets.txt")).FirstOrDefault().FullName;
            manifestLocation = files.Where(x => x.Name.Contains("manifest.txt")).FirstOrDefault().FullName;
        }

        public void ExtractResources()
        {
            string bpAssetsContents = File.ReadAllText(bpAssetsLocation);
            string manifestContents = File.ReadAllText(manifestLocation);

            bpAssetsResources = bpAssetsContents.Split(new string[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            manifestResources = manifestContents.Split(new string[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
