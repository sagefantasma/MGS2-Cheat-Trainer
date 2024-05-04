using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                gcx_comparator.CompareGCXFiles(args.ToList(), minimumMatchLength * 3);
                
                Console.Write("Press Y to restart, or any other key to quit: ");
                rerun = true;
            } while (Console.ReadKey().Key == ConsoleKey.Y);
        }
    }
}
