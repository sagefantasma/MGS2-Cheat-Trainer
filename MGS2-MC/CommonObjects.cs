using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGS2_MC
{
    internal class CommonObjects
    {
        /// <summary>
        /// Details memory information related to an object.
        /// Start determines the byte where the object starts being defined.
        /// End determines the byte where the object stops being defined.
        /// Length is automatically calculated, and determines how many bytes of memory comprise the object.
        /// </summary>
        public class MemoryOffset
        {
            public int Start { get; set; }
            public int End { get; set; }
            public int Length { get; set; }

            public MemoryOffset(int offsetStart) : this(offsetStart, offsetStart)
            {
            }

            public MemoryOffset(int offsetStart, int offsetEnd)
            {
                Start = offsetStart;
                End = offsetEnd;
                Length = Math.Abs(offsetEnd - offsetStart) + 1;
            }
        }

        /// <summary>
        /// An object representing a string value in MGS2.
        /// MemoryOffset describes how it is stored in memory.
        /// Tag provides the object with a constant, human-readable name.
        /// </summary>
        public class MGS2String
        {
            public MemoryOffset MemoryOffset { get; set; }
            public string Tag { get; set; }
        }
    }
}
