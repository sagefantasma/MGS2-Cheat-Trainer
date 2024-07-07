using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    internal class GCX_Object
    {
        public class Procedure
        {
            public string Name { get; private set; }
            public string DecodedContents { get; private set; }
            public byte[] RawContents { get; set; }

            public readonly int ProcTablePosition;
            public readonly int ScriptLength;
            public readonly int ScriptInitialPosition;

            public Procedure(string name, byte[] raw, string decoded, int procTablePosition, int scriptPosition)
            {
                Name = name;
                DecodedContents = decoded;
                RawContents = raw;
                ScriptLength = raw.Length;
                ScriptInitialPosition = scriptPosition;
                ProcTablePosition = procTablePosition;
            }
        }
    }
}
