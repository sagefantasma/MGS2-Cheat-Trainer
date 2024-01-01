using System;
using System.Diagnostics;
using System.IO;
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
        
        private static void CloseMGS2(object sender, EventArgs e)
        {
            //!!!NOTE!!!: This _WILL NOT WORK_ if you are running this program in a debugger and use the "Stop Debugging" feature.
            MGS2Process?.CloseMainWindow();
            MGS2Process?.Dispose();
        }

        private static void MGS2Runner(TrainerConfig config)
        {
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

                if(config.CloseGameWithTrainer)
                {
                    Application.ApplicationExit += (sender, args) => CloseMGS2(sender, args);
                }

                while (!MGS2Process.HasExited)
                {
                    //this thread loops forever while MGS2 is running
                }

                if (config.CloseTrainerWithGame)
                {
                    Application.Exit();
                }
                        
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
