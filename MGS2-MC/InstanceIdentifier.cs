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
            "Ocelot",
            "Houseman",
            "Ames",
            "Fatman",
            "Rosemary",
            "Drebin",
            "Sears",
            "Johnson",
            "Gurlukovich",
            "Silverburgh"
        };

        static readonly List<string> Patriot_AIs = new List<string>()
        {
            "JD",
            "GW",
            "TJ",
            "TR",
            "AL"
        };

        static readonly List<string> Patriot_Founders = new List<string>()
        {
            "Oh",
            "Adamska",
            "John",
            "Anderson",
            "Clark",
            "Mama"
        };
    }
}
