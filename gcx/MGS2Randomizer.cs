using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class MGS2Randomizer
    {
        public static string GcxDirectory { get; private set; }
        private static List<string> filesInDirectory;
        static GcxEditor gcxEditor = new GcxEditor();

        private static ItemSet VanillaItems;
        public static Random Randomizer { get; set; }

        public MGS2Randomizer(string gcxDirectory, int seed = 0)
        {
            GcxDirectory = gcxDirectory;
            filesInDirectory = Directory.EnumerateFiles(GcxDirectory).ToList();
            if (seed == 0)
            {
                seed = new Random(DateTime.UtcNow.Hour + DateTime.UtcNow.Minute + DateTime.UtcNow.Second + DateTime.UtcNow.Millisecond).Next();
            }

            Randomizer = new Random(seed);
        }

        private static uint FindCallingBytesInGcx(Location locationToFind)
        {
            string desiredGcxFile = filesInDirectory.Find(file => file.EndsWith($"scenerio_stage_{locationToFind.GcxFile}.gcx"));
            gcxEditor.FileName = desiredGcxFile;

            List<DecodedProc> contentTree = gcxEditor.BuildContentTree();
        }
    }
}
