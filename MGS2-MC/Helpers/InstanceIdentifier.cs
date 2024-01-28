using System;
using System.Collections.Generic;

namespace MGS2_MC
{
    internal abstract class InstanceIdentifier
    {
        public static string CreateInstanceIdentifier() 
        {
            Random random = new Random((int)DateTimeOffset.Now.ToUnixTimeSeconds());
            int agent = random.Next(Patriot_Agents.Count);
            int ai = random.Next(Patriot_AIs.Count);
            int founder = random.Next(Patriot_Founders.Count);

            return $"{Patriot_Founders[founder]}:{Patriot_AIs[ai]}:{Patriot_Agents[agent]}";
        }

        static readonly List<string> Patriot_Agents = new List<string>()
        {
            "Ames",
            "Drebin",
            "Fatman",
            "Gurlukovich",
            "Houseman",
            "Johnson",
            "Ocelot",
            "Raiden",
            "Rosemary",
            "Sears",
            "Silverburgh",
            "Snake"
        };

        static readonly List<string> Patriot_AIs = new List<string>()
        {
            "AL",
            "GW",
            "JD",
            "TJ",
            "TR"
        };

        static readonly List<string> Patriot_Founders = new List<string>()
        {
            "Adamska",
            "David",
            "Donald",
            "EVA",
            "Jane",
            "John"
        };
    }
}
