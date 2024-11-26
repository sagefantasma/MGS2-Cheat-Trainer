using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal static class GcxLevelResourceMapper
    {
        //this is a support class designed to take in a directory of .gcx files as well as a superdirectory
        //of resources and output a .csv file that has a breakdown of every resource included in each .gcx
        //file for analysis and planning

        /*
         * After completing work on this class, I determined that all of the required resources are NOT
         * present in every level, however the tanker(which was the main source of missing resources)
         * still runs with every single proc added, even though a lot of the procs are missing resources.
         * Presumably, if I were to call some of the invalid functions they would break, but without that,
         * it runs fine.
         * 
         * Next thing to test is tanker/plant exclusives. If everything on the tanker is present on every level
         * in the tanker, and same for the plant; then that's fantastic! We should be able to make a basic randomizer
         * with just that.
         */

        static string _gcxDirectory { get; set; }
        static string _resourceSuperDirectory { get; set; }
        internal class GcxResources
        {
            public string _gcxFile;
            public List<string> _resources;

            internal GcxResources(string gcxFile, List<string> resources)
            {
                _gcxFile = gcxFile;
                _resources = resources;
            }
        }

        static List<string> allWeaponItemResources = new List<string>()
        {
            "003ce0e9", "00f53890", "00ca7cd0", "007ee425", "009e33e1", "00586251", "00586a51",
            "00589111", "005999d1", "005cbf11", "0036d0ed", "00f58ad0", "00512dc4", "0060ea51",
            "003db0ed", "00638ad1", "003dd4b9", "0065c791", "001dfe6e", "00bc2c9f", "003deea5",
            "00676651", "003dfcd1", "003e0539", "0068cf91", "00686a51", "003e24dd", "00b4cb72",
            "00684291", "00573911", "00bd7cce", "0068e986", "0067d151", "00f971fb", "001cd720", 
            "00ca2e0f", "00320878", "00efa25d", "00c327e7", "00687351", "00f504ea", "006968d1",
            "0055062b", "00629ed1", "006368d1", "0012ff3a", "009e9447", "00d26236", "001cf3f9",
            "00eb0f44", "00a4004f", "005362e4", "0073b479", "00376d7d", "00c0267e", "006ab337",
            "009d4021", "00b37ec5", "00646487", "004da20c", "0007b199", "008bacc2", "0062d09e",
            "00706bd2", "00eac2fd", "005843d1", "0033475f", "00b1246b", "0029430e", "00889f69"
        }; // 70 resources

        public static string MapLevelsToResources(string gcxDirectory, string resourceSuperDirectory)
        {
            _gcxDirectory = gcxDirectory;
            _resourceSuperDirectory = resourceSuperDirectory;
            string csv = "levelResources.csv";

            DirectoryInfo gcxDirectoryInfo = new DirectoryInfo(_gcxDirectory);
            DirectoryInfo resourceSuperDirectoryInfo = new DirectoryInfo(_resourceSuperDirectory);

            List<GcxResources> resourceMapping = new List<GcxResources>();
            foreach(FileInfo file in gcxDirectoryInfo.GetFiles())
            {
                string parsedStageName = file.Name.Replace("scenerio_stage_", "").Replace(".gcx", "");
                DirectoryInfo gcxResourceDirectory = resourceSuperDirectoryInfo.GetDirectories(parsedStageName).FirstOrDefault();
                FileInfo bpAssets = gcxResourceDirectory.GetFiles("bp_assets.txt").FirstOrDefault();
                FileInfo manifest = gcxResourceDirectory.GetFiles("manifest.txt").FirstOrDefault();
                string bpAssetsContents = File.ReadAllText(bpAssets.FullName);
                string manifestContents = File.ReadAllText(manifest.FullName);
                List<string> resources = AggregateResources(bpAssetsContents, manifestContents);
                resourceMapping.Add(new GcxResources(parsedStageName, resources));
            }

            string csvFormattedContents = FormatToCsv(resourceMapping);
            File.WriteAllText(csv, csvFormattedContents);

            return csv;
        }

        private static List<string> AggregateResources(string assets, string manifest)
        {
            //each line ends with \r\r\n for both files
            List<string> assetsResources = assets.Split(new string[] { "\r\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> manifestResources = manifest.Split(new string[] { "\r\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
            
            List<string> cleanedAssets = new List<string>();
            foreach (string resource in assetsResources)
            {
                cleanedAssets.Add(resource.Split('/').LastOrDefault());
            }
            foreach (string resource in manifestResources)
            {
                cleanedAssets.Add(resource.Split('/').LastOrDefault());
            }

            return cleanedAssets;
        }

        private static string FormatToCsv(List<GcxResources> resourceMappings)
        {
            //each row as a file, each column being different resources(except the first one being the file name)
            string csvFormattedString = "";
            foreach (GcxResources resourceMapping in resourceMappings)
            {
                csvFormattedString += resourceMapping._gcxFile+":";
                foreach(string resource in resourceMapping._resources)
                {
                    csvFormattedString += resource + ":";
                }
                csvFormattedString += "\r\n";
            }

            return csvFormattedString;
        }
    }
}
