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
            //this did something funky when adding files to the manifest - seemed like it was bad indexing for some reason?
            //doing this and only this resulted in only the "coldmedslabel" showing up, but not the actual medicine box... why?
            //adding assets/tri/us/itembox.tri,us/stage/w00a/cache/00883186.tri,cache/00883186.tri to the manifest got a box to show up, but not with textures. i think this is because 00883186 is not attached to the correct ibox texture in the bpassets
            //adding what i thought was all the relevant textures actually wasnt enough either... i'm genuinely considering just adding every texture at this point and seeing what happens xdd
            //either way i think that is a good experiment, and could possibly reveal quite a bit. i want to try this
            //1. develop a tool that will go through all manifest & bp_assets files, and collate a master list. (separate items by 0x0D, 0x0D, 0x0A)
            //2. create a test manifest & bp_assets file with ALL items from master list, correcting for the stage name.
            //3. test it out
            //4. ???
            //5. Profit

            //for bpassets:
            //textures/flatlist, then kms files

            //for manifest:
            //
            /*GcxLevelResourceMapper.MapLevelsToResources("C:\\Users\\yonan\\Source\\Repos\\MGS2-Cheat-Trainer\\gcx\\bin\\Debug\\main game",
                "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage");*/
            /*GcxLevelResourceMapper.BuildMasterResourceList("C:\\Users\\yonan\\Source\\Repos\\MGS2-Cheat-Trainer\\gcx\\bin\\Debug\\main game",
                "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage");*/
            /*ResourceEditor.AddResource("w04a", "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage", "coldmedslabel");
            ResourceEditor.AddResource("w04a", "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage", "medicineibox");
            ResourceEditor.AddResource("w04a", "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage", "medicinesh");
            /*
            return;*/
            /*foreach(Resource value in Resource.ResourceList)
            {
                ResourceEditor.AddResource("w04a", "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage", value.CommonName);
            }*/
            List<string> strings = new List<string>();
            foreach(Resource value in Resource.ResourceList)
            {
                strings.Add(value.CommonName);
            }
            List<string> stages = new List<string> { "w00a", "w00b", "w00c", "w01a", "w01b", "w01c", "w01d", "w01e", "w01f",
            "w02a", "w03a", "w03b", "w04a", "w04b", "w04c", "w11a", "w11b", "w11c", "w12a", "w12b", "w12c", "w13a", "w13b",
            "w14a", "w15a", "w15b", "w16a", "w16b", "w17a", "w18a", "w19a", "w20a", "w20b", "w20c", "w20d", "w21a", "w21b",
            "w22a", "w23a", "w23b", "w24a", "w24b", "w24c", "w24d", "w24e", "w25a", "w25b", "w25c", "w25d", "w28a", "w31a",
            "w31b", "w31c", "w31d", "w31f", "w32a", "w32b", "w41a", "w42a", "w43a", "w44a", "w45a", "w46a", "w51a", "w61a"};
            //w31f is in the list of stages, but doesn't have its own gcx... it just reuses w31c's gcx file??? so weird.
            foreach(string stage in stages)
                ResourceEditor.AddResources(stage, "C:\\Users\\yonan\\Documents\\Pinned Folders\\C Drive Steam Games\\MGS2\\eu\\stage", strings);
            ShowGui();
            return;
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
