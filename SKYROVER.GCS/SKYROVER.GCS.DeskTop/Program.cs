using GMap.NET.CacheProviders;
using log4net;
using log4net.Config;
using MissionPlanner;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop
{
   public static class Program
    {

        private static readonly ILog log = LogManager.GetLogger("Program");

        public static bool WindowsStoreApp { get { return Application.ExecutablePath.Contains("WindowsApps"); } }

        public static DateTime starttime = DateTime.Now;

        public static string name { get; internal set; }

        internal static Thread Thread;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool canRun= CheckRunTime();
            if (!canRun) return;

            Thread = Thread.CurrentThread;
            Application.EnableVisualStyles();
            XmlConfigurator.Configure();
            log.Info("******************* Logging Configured *******************");



            Application.SetCompatibleTextRenderingDefault(false);

            // set the cache provider to my custom version
           
            GMap.NET.Internals.CacheLocator.Location= Settings.GetDataDirectory() + "gmapcache" + Path.DirectorySeparatorChar;// cachePath;// Settings.GetDataDirectory() + "gmapcache";
            // GMap.NET.GMaps.Instance.PrimaryCache = new SQLitePureImageCache();
            // add proxy settings
            GMap.NET.MapProviders.GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMap.NET.MapProviders.GMapProvider.WebProxy.Credentials = CredentialCache.DefaultCredentials;

            ServicePointManager.DefaultConnectionLimit = 10;

            // generic status report screen
            MAVLinkInterface.CreateIProgressReporterDialogue += title =>
                new ProgressReporterDialogue() { StartPosition = FormStartPosition.CenterScreen, Text = title,ShowInTaskbar=false, Opacity=0 };

            System.Windows.Forms.Application.ThreadException += Application_ThreadException;

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            CleanupFiles();

            CustomMessageBox.ShowEvent += (text, caption, buttons, icon) =>
            {
                return (CustomMessageBox.DialogResult)(int)MissionPlanner.MsgBox.CustomMessageBox.Show(text, caption, (MessageBoxButtons)(int)buttons, (MessageBoxIcon)(int)icon);
            };



            try
            {

                Thread.CurrentThread.Name = "Base Thread";
                Application.Run(new MainUI());

            } catch (Exception ex) {

                log.Error(ex.Message);
            }
          
        }


        private static bool CheckRunTime()
        {

            // make sure new enough .net framework is installed

            Microsoft.Win32.RegistryKey installed_versions =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
            string[] version_names = installed_versions.GetSubKeyNames();
            //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
            double Framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1),
                CultureInfo.InvariantCulture);
            int SP =
                Convert.ToInt32(installed_versions.OpenSubKey(version_names[version_names.Length - 1])
                    .GetValue("SP", 0));

            if (Framework < 4.0)
            {
                CustomMessageBox.Show("This program requires .NET Framework 4.0. You currently have " + Framework, "提示");
                return false;
            }
            return true;
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
           // throw new NotImplementedException();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
           // throw new NotImplementedException();
        }


        static void CleanupFiles()
        {
            try
            {
                //cleanup bad file
                string file = Settings.GetRunningDirectory() +
                              @"LogAnalyzer\tests\TestUnderpowered.py";
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch { }

            try
            {
                var file = "NumpyDotNet.dll";
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch
            {

            }
            try
            {
                foreach (string newupdater in Directory.GetFiles(Settings.GetRunningDirectory(), "Updater.exe*.new"))
                {
                    File.Copy(newupdater, newupdater.Remove(newupdater.Length - 4), true);
                    File.Delete(newupdater);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception during update", ex);
            }

            try
            {
                foreach (string newupdater in Directory.GetFiles(Settings.GetRunningDirectory(), "tlogThumbnailHandler.dll.new"))
                {
                    File.Copy(newupdater, newupdater.Remove(newupdater.Length - 4), true);
                    File.Delete(newupdater);
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception during update", ex);
            }
        }
    }
}
