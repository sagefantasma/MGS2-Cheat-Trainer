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
            /*List<string> stages = new List<string> { "w00a", "w00b", "w00c", "w01a", "w01b", "w01c", "w01d", "w01e", "w01f",
            "w02a", "w03a", "w03b", "w04a", "w04b", "w04c", "w11a", "w11b", "w11c", "w12a", "w12b", "w12c", "w13a", "w13b",
            "w14a", "w15a", "w15b", "w16a", "w16b", "w17a", "w18a", "w19a", "w20a", "w20b", "w20c", "w20d", "w21a", "w21b",
            "w22a", "w23a", "w23b", "w24a", "w24b", "w24c", "w24d", "w25a", "w25b", "w25c", "w25d", "w28a", "w31a",
            "w31b", "w31c", "w31d", "w32a", "w41a", "w42a", "w43a", "w44a", "w45a", "w46a", "w51a", "w61a"};
            foreach(string gcxFile in stages)
            {
                string gcxPath = $"main game\\scenerio_stage_{gcxFile}.gcx";
                GcxEditor gcx_Editor = new GcxEditor();
                gcx_Editor.CallDecompiler(gcxPath);
                List<DecodedProc> allFileFunctions = gcx_Editor.BuildContentTree();
                List<DecodedProc> spawns = new List<DecodedProc>();
                foreach (DecodedProc entry in allFileFunctions)
                {
                    if (MGS2Randomizer.ContainsSpawningFunctions(entry))
                        spawns.Add(entry);
                }
                ProcEditor procEditor = new ProcEditor(spawns, true);
                procEditor.WriteOutSpawns(gcxFile);
            }*/

            //ResourceEditor.AddResource()

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
