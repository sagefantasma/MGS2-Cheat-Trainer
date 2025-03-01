namespace gcx
{
    public class Ctxr : Resource
    {
        public Ctxr(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "ctxr";
        }
    }
    public class Cmdl : Resource
    {
        public Cmdl(string name, string hash, string stage, string text, SubType subType) : base(name, hash, stage, text)
        {
            SubType = subType;
            Extension = "cmdl";
        }

        public SubType SubType { get; set; }
    }
    public enum SubType
    {
        Evm,
        Kms,
        Zms
    }
    public class Tri : Resource
    {
        public Tri(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "tri";
        }
    }
    public class Zms : Resource
    {
        public Zms(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "zms";
        }
    }
    public class Var : Resource
    {
        public Var(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "var";
        }
    }
    public class Sar : Resource
    {
        public Sar(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "sar";
        }
    }
    public class Row : Resource
    {
        public Row(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "row";
        }
    }
    public class O2d : Resource
    {
        public O2d(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "o2d";
        }
    }
    public class Mar : Resource
    {
        public Mar(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "mar";
        }
    }
    public class Kms : Resource
    {
        public Kms(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "kms";
        }
    }
    public class Evm : Resource
    {
        public Evm(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "evm";
        }
    }
    public class Cv2 : Resource
    {
        public Cv2(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "cv2";
        }
    }
    public class Hzx : Resource
    {
        public Hzx(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "hzx";
        }
    }
    public class Lt2 : Resource
    {
        public Lt2(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "lt2";
        }
    }
    public class Far : Resource
    {
        public Far(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "far";
        }
    }
    public class Anm : Resource
    {
        public Anm(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "anm";
        }
    }
    public class Gcx : Resource
    {
        public Gcx(string name, string hash, string stage, string text) : base(name, hash, stage, text)
        {
            Extension = "gcx";
        }
    }
}
