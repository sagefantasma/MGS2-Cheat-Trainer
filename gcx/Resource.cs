using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gcx
{
    public abstract partial class Resource
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Extension;
        public string Stage { get; set; }
        public string FullText { get; set; }
        public Resource(string name, string hash, string stage, string text) 
        { 
            Name = name;
            Hash = hash;
            Stage = stage;
            FullText = text;
        }
    }
}
