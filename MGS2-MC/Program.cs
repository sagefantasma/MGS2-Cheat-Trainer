using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using static MGS2_MC.TrainerConfigStructure;

namespace MGS2_MC
{
    internal static class Program
    {
        internal static Process MGS2Process;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TrainerConfig trainerConfig = LoadConfig();
            if (trainerConfig.AutoLaunchGame)
            {
                Thread mgs2Thread = new Thread(() => MGS2Runner(trainerConfig));
                mgs2Thread.Start();
            }
            Application.Run(new GUI());            
        }

        private static TrainerConfig LoadConfig()
        {
            return JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText("TrainerConfig.json"));
        }

        private static void MGS2Runner(TrainerConfig config)
        {
            //TODO: 
            MGS2Process = new Process();
            try
            {
                //mgs2.StartInfo = new ProcessStartInfo("steam://rungameid/2131640"); //leaving this here in case it makes sense to use this method instead for some reason
                string mgs2Location = config.Mgs2ExePath;
                FileInfo directoryInfo = new FileInfo(mgs2Location);
                ProcessStartInfo mgs2StartInfo = new ProcessStartInfo(mgs2Location)
                {
                    WorkingDirectory = directoryInfo.DirectoryName,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                };
                MGS2Process.StartInfo = mgs2StartInfo;
                MGS2Process.Start();

                while (!MGS2Process.HasExited)
                {

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
