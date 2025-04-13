using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gcx
{
    internal class Program
    {
        private static void ShowGui()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GcxExplorer());
        }



        [STAThread]
        static void Main(string[] args)
        {
            /*
            ResourceExtractor resourceExtractor = new ResourceExtractor("C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage\\r_tnk0");
            resourceExtractor.ExtractResources();
            List<string> tankerSnakeBpAssets = resourceExtractor.bpAssetsResources.ToList();
            List<string> tankerSnakeManifest = resourceExtractor.manifestResources.ToList();
            List<Resource> tankerSnakeBpAssetsResources = new List<Resource>();
            List<Resource> tankerSnakeManifestResources = new List<Resource>();
            foreach(string bpAsset in tankerSnakeBpAssets)
            {
                tankerSnakeBpAssetsResources.Add(ResourceParser.ParseResource(bpAsset));
            }
            foreach (string manifestAsset in tankerSnakeManifest)
            {
                tankerSnakeManifestResources.Add(ResourceParser.ParseResource(manifestAsset));
            }

            resourceExtractor = new ResourceExtractor("C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage\\r_vr_1");
            resourceExtractor.ExtractResources();
            List<string> mgs1SnakeBpAssets = resourceExtractor.bpAssetsResources.ToList();
            List<string> mgs1SnakeManifest = resourceExtractor.manifestResources.ToList();
            List<Resource> mgs1SnakeBpAssetsResources = new List<Resource>();
            List<Resource> mgs1SnakeManifestResources = new List<Resource>();
            foreach (string bpAsset in mgs1SnakeBpAssets)
            {
                mgs1SnakeBpAssetsResources.Add(ResourceParser.ParseResource(bpAsset));
            }
            foreach (string manifestAsset in mgs1SnakeManifest)
            {
                mgs1SnakeManifestResources.Add(ResourceParser.ParseResource(manifestAsset));
            }

            foreach (Resource bpAsset in tankerSnakeBpAssetsResources)
            {
                Resource duplicatedResource = mgs1SnakeBpAssetsResources.FirstOrDefault(asset => asset.Hash == bpAsset.Hash);
                
                if(duplicatedResource != null)
                {
                    mgs1SnakeBpAssetsResources.Remove(duplicatedResource);
                }
            }

            foreach (Resource manifestAsset in tankerSnakeManifestResources)
            {
                Resource duplicatedResource = mgs1SnakeManifestResources.FirstOrDefault(asset => asset.Hash == manifestAsset.Hash);

                if (duplicatedResource != null)
                {
                    mgs1SnakeManifestResources.Remove(duplicatedResource);
                }
            }

            foreach(Resource bpAsset in mgs1SnakeBpAssetsResources)
            {
                //bpAsset.Stage = "r_tnk0";
                tankerSnakeBpAssetsResources.Add(bpAsset);
            }

            foreach (Resource bpAsset in mgs1SnakeManifestResources)
            {
                //bpAsset.Stage = "r_tnk0";
                tankerSnakeManifestResources.Add(bpAsset);
            }

            ResourceBuilder.BuildResources(tankerSnakeBpAssetsResources, tankerSnakeManifestResources);
            */
            ShowGui();
            return;

            List<string> strings = new List<string>();
            foreach (OldResource value in OldResource.HFBladeResourceList)
            {
                strings.Add(value.CommonName);
            }
            /*List<string> stages = new List<string> { "w00a", "w00b", "w00c", "w01a", "w01b", "w01c", "w01d", "w01e", "w01f",
            "w02a", "w03a", "w03b", "w04a", "w04b", "w04c", "w11a", "w11b", "w11c", "w12a", "w12b", "w12c", "w13a", "w13b",
            "w14a", "w15a", "w15b", "w16a", "w16b", "w17a", "w18a", "w19a", "w20a", "w20b", "w20c", "w20d", "w21a", "w21b",
            "w22a", "w23a", "w23b", "w24a", "w24b", "w24c", "w24d", "w24e", "w25a", "w25b", "w25c", "w25d", "w28a", "w31a",
            "w31b", "w31c", "w31d", "w31f", "w32a", "w32b", "w41a", "w42a", "w43a", "w44a", "w45a", "w46a", "w51a", "w61a"};*/
            List<string> stages = new List<string> { "w12a" };
            string mgs2Directory = @"C:\Users\yonan\Documents\Pinned Folders\C Drive Steam Games\MGS2\";
            foreach (string stage in stages)
                ResourceEditor.AddResources(stage, mgs2Directory + "\\eu\\stage", strings);

            
            bool rerun = false;
            do
            {
                if (rerun)
                {
                    Console.Clear();
                    Console.Write("Press y to reuse the chosen files, or any other key to choose new files: ");
                    if(Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        args = new string[0];
                    }
                }
                bool first = true;
                if (args.Length == 0)
                {
                    Console.Write("Please enter the path of the first gcx file to examine: ");
                    List<string> files = new List<string>();
                    do
                    {
                        if (!first)
                        {
                            Console.Clear();
                            Console.Write("\nPlease enter the path of the next gcx file to examine: ");
                        }
                        files.Add(Console.ReadLine());
                        Console.Write("\nPress any key to enter another gcx file, or N to begin comparing the chosen files: ");
                        first = false;
                    } while (Console.ReadKey().Key != ConsoleKey.N);

                    args = files.ToArray();
                }

                Console.Write("\n\n\nWhat is the minimum amount of matching bytes you want to examine?: ");
                int minimumMatchLength;
                first = true;
                bool successfulParse;
                do
                {
                    if (!first)
                    {
                        Console.Write("\nCould not parse an integer from what you entered, please try again: ");
                    }
                    string response = Console.ReadLine();
                    successfulParse = int.TryParse(response, out minimumMatchLength);
                    first = false;
                    Console.Clear();
                } while (!successfulParse);

                Console.Write("Examining files for matching contents...");
                GcxComparator.CompareGCXFiles(args.ToList(), minimumMatchLength * 3);
                
                Console.Write("Press Y to restart, or any other key to quit: ");
                rerun = true;
            } while (Console.ReadKey().Key == ConsoleKey.Y);
        }
    }
}
