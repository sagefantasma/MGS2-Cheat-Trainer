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
            try
            {
                TrainerConfig trainerConfig = LoadConfig();
                if (trainerConfig.AutoLaunchGame)
                {
                    Thread mgs2Thread = new Thread(() => Mgs2Runner(trainerConfig));
                    mgs2Thread.Start();
                }
            } 
            catch (Exception e) 
            { 
                //TODO: add logging :)
            }
            Application.Run(new GUI());            
        }

        private static TrainerConfig LoadConfig()
        {
            try
            {
                return JsonSerializer.Deserialize<TrainerConfig>(File.ReadAllText("TrainerConfig.json"));
            }
            catch(Exception e)
            {
                //TODO: add logging :)
                return null;
            }
        }
        
        private static void CloseMGS2(object sender, EventArgs e)
        {
            //!!!NOTE!!!: This _WILL NOT WORK_ if you are running this program in a debugger and use the "Stop Debugging" feature.
            MGS2Process?.CloseMainWindow();
            MGS2Process?.Dispose();
        }

        private static bool IsDirectLaunchEnabled(FileInfo mgs2Executable, FileInfo steamAppIdFile)
        {
            if (steamAppIdFile.Exists)
            {
                try
                {
                    string steamAppIdFileContents = File.ReadAllText(steamAppIdFile.FullName);
                    if (steamAppIdFileContents.Equals(MGS2Constants.SteamAppId))
                    {
                        //the steam_appid file exists and has the appropriate content
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //TODO: add logging :)
                    return false;
                }
            }

            return false;
        }

        private static void EnableDirectLaunch(FileInfo steamAppIdFile)
        {
            try
            {
                //the steam_appid file is either missing, or has incorrect contents. overwrite it so we can direct launch w/o issue.
                using (StreamWriter writer = new StreamWriter(File.Open(steamAppIdFile.FullName, FileMode.Create)))
                {
                    writer.WriteLine(MGS2Constants.SteamAppId);
                }
            }
            catch(Exception e)
            {
                //TODO: add logging :)
            }
        }

        private static void StartMgs2(string mgs2Location, string mgs2Directory, TrainerConfig config)
        {
            ProcessStartInfo mgs2StartInfo = new ProcessStartInfo(mgs2Location)
            {
                WorkingDirectory = mgs2Directory,
                UseShellExecute = false,
                CreateNoWindow = false,
            };
            MGS2Process.StartInfo = mgs2StartInfo;
            MGS2Process.Start();

            if (config.CloseGameWithTrainer)
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

        private static void Mgs2Runner(TrainerConfig config)
        {
            MGS2Process = new Process();
            try
            {
                //mgs2.StartInfo = new ProcessStartInfo("steam://rungameid/2131640"); //leaving this here in case it makes sense to use this method instead for some reason
                string mgs2Location = config.Mgs2ExePath;
                FileInfo mgs2Executable = new FileInfo(mgs2Location);
                FileInfo steamAppIdFile = new FileInfo(mgs2Executable.DirectoryName + MGS2Constants.SteamAppIdFileName);

                bool directLaunchEnabled = IsDirectLaunchEnabled(mgs2Executable, steamAppIdFile);                
                if (!directLaunchEnabled)
                {
                    try
                    {
                        EnableDirectLaunch(steamAppIdFile);
                    }
                    catch(Exception e)
                    {
                        //TODO: add logging :)
                        //as of right now, I don't know if it will work without this shortcut, so i won't throw yet.
                    }
                }

                StartMgs2(mgs2Location, mgs2Executable.DirectoryName, config);
            }
            catch(Exception e)
            {
                //TODO: add logging :)
            }
        }
    }
}
