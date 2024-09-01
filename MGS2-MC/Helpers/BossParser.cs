using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MGS2_MC.Helpers
{
    public static class BossParser
    {
        public static Constants.Boss ParseNode(string nodeName)
        {
            switch(nodeName)
            {
                case "Olga":
                    return Constants.Boss.Olga;
                case "Fortune":
                    return Constants.Boss.Fortune;
                case "Fatman":
                    return Constants.Boss.Fatman;
                case "Harrier":
                    return Constants.Boss.Harrier;
                case "Vamp":
                    return Constants.Boss.Vamp;
                case "Vamp (Sniping)":
                    return Constants.Boss.VampSnipe;
                case "Solidus":
                    return Constants.Boss.Solidus;
                case "RAY #01":
                    return Constants.Boss.Ray1;
                case "RAY #02":
                    return Constants.Boss.Ray2;
                case "RAY #03":
                    return Constants.Boss.Ray3;
                case "RAY #04":
                    return Constants.Boss.Ray4;
                case "RAY #05":
                    return Constants.Boss.Ray5;
                case "RAY #06":
                    return Constants.Boss.Ray6;
                case "RAY #07":
                    return Constants.Boss.Ray7;
                case "RAY #08":
                    return Constants.Boss.Ray8;
                case "RAY #09":
                    return Constants.Boss.Ray9;
                case "RAY #10":
                    return Constants.Boss.Ray10;
                case "RAY #11":
                    return Constants.Boss.Ray11;
                case "RAY #12":
                    return Constants.Boss.Ray12;
                case "RAY #13":
                    return Constants.Boss.Ray13;
                case "RAY #14":
                    return Constants.Boss.Ray14;
                case "RAY #15":
                    return Constants.Boss.Ray15;
                case "RAY #16":
                    return Constants.Boss.Ray16;
                case "RAY #17":
                    return Constants.Boss.Ray17;
                case "RAY #18":
                    return Constants.Boss.Ray18;
                case "RAY #19":
                    return Constants.Boss.Ray19;
                case "RAY #20":
                    return Constants.Boss.Ray20;
                case "RAY #21":
                    return Constants.Boss.Ray21;
                case "RAY #22":
                    return Constants.Boss.Ray22;
                case "RAY #23":
                    return Constants.Boss.Ray23;
                case "RAY #24":
                    return Constants.Boss.Ray24;
                case "RAY #25":
                    return Constants.Boss.Ray25;
                default:
                    throw new InvalidEnumArgumentException("Boss not recognized.");
            }
        }
    }
}
