using System;
using System.Collections.Generic;
using System.Data;
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

        static string AKAmmoLabel = "003ce0e9";
        static string AKWeaponLabel = "00f53890";
        static string MagazineIbox = "00ca7cd0";
        static string MagazineLabel = "007ee425";
        static string MagazineSh = "009e33e1";
        static string C4Label = "00586251";
        static string ChaffLabel = "00586a51";
        static string ClaymoreLabel = "00589111";
        static string DMicLabel = "005999d1";
        static string DMicIbox = "0029430e";
        static string DMicSh = "00889f69";
        static string GrenadeLabel = "005cbf11";
        static string M4AmmoLabel = "0036d0ed";
        static string M4WeaponLabel = "00f58ad0";
        static string M9AmmoLabel = "003d86e5";
        static string M9WeaponLabel = "0060ea51";
        static string NikitaAmmoLabel = "003db0ed";
        static string NikitaWeaponLabel = "00638ad1";
        static string PSG1AmmoLabel = "003dd4b9";
        static string PSG1WeaponLabel = "0065c791";
        static string PSG1TAmmoLabel = "001dfe6e";
        static string PSG1TWeaponLabel = "00bc2c9f";
        static string RGB6AmmoLabel = "003deea5";
        static string RGB6WeaponLabel = "00676651";
        static string SocomAmmoLabel = "003dfcd1";
        static string StingerAmmoLabel = "003e0539";
        static string StingerWeaponLabel = "0068cf91";
        static string StunLabel = "00686a51";
        static string USPAmmoLabel = "003e24dd";
        static string AKSuppressorLabel = "00b4cb72";
        static string BandagesLabel = "00684291";
        static string BodyArmorLabel = "00573911";
        static string CboxLabel = "005843d1";
        static string RationLabel = "0067d151";
        static string RationIbox = "00bd7cce";
        static string RationSh = "0068e986";
        static string ColdMedsLabel = "00f971fb";
        static string DigitalCameraLabel = "00ca2e0f";
        static string DigitalCameraIbox = "001cd720";
        static string DigitalCameraSh = "00320878";
        static string PentazeminLabel = "00efa25d";
        static string SensorBLabel = "00c327e7";
        static string ShaverLabel = "00687351";
        static string SocomSuppressorLabel = "00f504ea";
        static string ThermalGogglesLabel = "006968d1";
        static string USPSuppressorLabel = "0055062b";
        static string MineDetectorLabel = "00629ed1";
        static string NVGLabel = "006368d1";
        static string RifleAmmoIbox = "0012ff3a";
        static string RifleAmmoSh = "009e9447";
        static string RifleIbox = "00d26236";
        static string RifleSh = "001cf3f9";
        static string LauncherAmmoIbox = "00eb0f44";
        static string LauncherAmmoSh = "00a4004f";
        static string LauncherIbox = "005362e4";
        static string LauncherSh = "0073b479";
        static string GrenadeIbox = "00376d7d";
        static string GrenadeSh = "00c0267e";
        static string HandgunAmmoIbox = "006ab337";
        static string HandgunAmmoSh = "009d4021";
        static string MedicineIbox = "00b37ec5";
        static string MedicineSh = "00646487";
        static string HandgunIbox = "004da20c";
        static string HandgunSh = "0007b199";
        static string Box2Ibox = "008bacc2";
        static string Box2Sh = "0062d09e";
        static string GoggleIbox = "00706bd2";
        static string GoggleSh = "00eac2fd";
        static string DetectorIbox = "0033475f";
        static string DetectorSh = "00b1246b";
        static List<string> allWeaponItemResources = new List<string>()
        {
            AKAmmoLabel, AKWeaponLabel, MagazineIbox, MagazineLabel, MagazineSh, C4Label, ChaffLabel,
            ClaymoreLabel, DMicLabel, GrenadeLabel, M4AmmoLabel, M4WeaponLabel, M9AmmoLabel, M9WeaponLabel,
            NikitaAmmoLabel, NikitaWeaponLabel, PSG1AmmoLabel, PSG1WeaponLabel, PSG1TAmmoLabel, PSG1TWeaponLabel, RGB6AmmoLabel,
            RGB6WeaponLabel, SocomAmmoLabel, StingerAmmoLabel, StingerWeaponLabel, StunLabel, USPAmmoLabel, AKSuppressorLabel,
            BandagesLabel, BodyArmorLabel, RationIbox, RationSh , RationLabel, ColdMedsLabel, DigitalCameraIbox,
            DigitalCameraLabel, DigitalCameraSh, PentazeminLabel, SensorBLabel, ShaverLabel, SocomSuppressorLabel, ThermalGogglesLabel,
            USPSuppressorLabel, MineDetectorLabel, NVGLabel, RifleAmmoIbox, RifleAmmoSh, RifleIbox, RifleSh,
            LauncherAmmoIbox, LauncherAmmoSh, LauncherIbox, LauncherSh, GrenadeIbox, GrenadeSh, HandgunAmmoIbox,
            HandgunAmmoSh, MedicineIbox, MedicineSh, HandgunIbox, HandgunSh, Box2Ibox, Box2Sh,
            GoggleIbox, GoggleSh, CboxLabel, DetectorIbox, DetectorSh, DMicIbox, DMicSh
        }; // 70 resources

        static List<string> AllPlantWeaponItemResources = new List<string>()
        {
            AKAmmoLabel, AKWeaponLabel, MagazineIbox, MagazineLabel, MagazineSh, C4Label, ChaffLabel,
            ClaymoreLabel, DMicLabel, GrenadeLabel, M4AmmoLabel, M4WeaponLabel, M9AmmoLabel, M9WeaponLabel,
            NikitaAmmoLabel, NikitaWeaponLabel, PSG1AmmoLabel, PSG1WeaponLabel, PSG1TAmmoLabel, PSG1TWeaponLabel, RGB6AmmoLabel,
            RGB6WeaponLabel, SocomAmmoLabel, StingerAmmoLabel, StingerWeaponLabel, StunLabel, AKSuppressorLabel,
            BandagesLabel, BodyArmorLabel, RationIbox, RationSh , RationLabel, ColdMedsLabel, DigitalCameraIbox,
            DigitalCameraLabel, DigitalCameraSh, PentazeminLabel, SensorBLabel, ShaverLabel, SocomSuppressorLabel, ThermalGogglesLabel,
            MineDetectorLabel, NVGLabel, RifleAmmoIbox, RifleAmmoSh, RifleIbox, RifleSh,
            LauncherAmmoIbox, LauncherAmmoSh, LauncherIbox, LauncherSh, GrenadeIbox, GrenadeSh, HandgunAmmoIbox,
            HandgunAmmoSh, MedicineIbox, MedicineSh, HandgunIbox, HandgunSh, Box2Ibox, Box2Sh,
            GoggleIbox, GoggleSh, CboxLabel, DetectorIbox, DetectorSh, DMicIbox, DMicSh
        };

        static List<string> AllTankerWeaponItemResources = new List<string>()
        {
            ChaffLabel, GrenadeLabel, M9AmmoLabel, StunLabel, USPAmmoLabel, BandagesLabel,
            RationIbox, RationSh, RationLabel, ColdMedsLabel, PentazeminLabel, ThermalGogglesLabel, USPSuppressorLabel, GrenadeIbox,
            GrenadeSh, HandgunAmmoIbox, HandgunAmmoSh, MedicineIbox, MedicineSh, Box2Ibox, Box2Sh, GoggleIbox, GoggleSh, CboxLabel
        };

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

            List<GcxResources> missingAssets = new List<GcxResources>();
            foreach (GcxResources resourceMap in resourceMapping)
            {
                if (!resourceMap._gcxFile.StartsWith("w0"))
                {
                    List<string> missingResources = new List<string>();
                    foreach (string resource in AllPlantWeaponItemResources)
                    {
                        if (!resourceMap._resources.Contains($"{resource}.kms"))
                        {
                            missingResources.Add(resource);
                        }
                    }
                    if(missingResources.Count > 0)
                        missingAssets.Add(new GcxResources(resourceMap._gcxFile, missingResources));
                }
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
