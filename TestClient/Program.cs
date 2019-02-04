using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NsisoLauncherCore.Auth;
using NsisoLauncherCore;
using NsisoLauncherCore.Modules;
using NsisoLauncherCore.Util;
using NsisoLauncherCore.Net;
using Version = NsisoLauncherCore.Modules.Version;
using System.Net.Http;
using NsisoLauncherCore.Net.Server;

namespace TestClient
{
    class Program
    {

        static LaunchHandler handler = new LaunchHandler();
        static MultiThreadDownloader downloader = new MultiThreadDownloader();
        static void Main(string[] args)
        {

            //ServerInfo info = new ServerInfo("d3.a0mc.com", 33020);
            //info.StartGetServerInfo();


            Console.WriteLine("Nsiso Launcher Core Test Client");
            handler.LaunchLog += (s, l) => WriteLog(l);
            handler.GameLog += (a, b) => Console.WriteLine(b);
            handler.GameExit += Handler_GameExit;
            Console.WriteLine("Begin searching version list...");
            var vers = handler.GetVersionsAsync().Result;
            Console.WriteLine("There have {0} version(s) valiable.", vers.Count);
            int uid = 0;
            vers.ForEach((e) =>
            {
                Console.WriteLine("({0}){1}", uid, e.ID);
                uid++;
            });
            Console.Write("Please select which version you want to launch:");
            int selectUid = int.Parse(Console.ReadLine());
            var launchVer = vers[selectUid];

            NsisoLauncherCore.Net.FunctionAPI.FunctionAPIHandler asa = new NsisoLauncherCore.Net.FunctionAPI.FunctionAPIHandler(DownloadSource.BMCLAPI);
            var ress = asa.GetLiteloaderList(launchVer).Result;


            Console.WriteLine("Check & download lost lib");
            var downloadTasks = FileHelper.GetLostDependDownloadTaskAsync(DownloadSource.BMCLAPI, handler, launchVer).Result;
            Console.WriteLine("There have {0} depend file lost.", downloadTasks.Count);
            if (downloadTasks.Count != 0)
            {
                Console.WriteLine("Need download now? (Y/N)");
                char choosen = Console.ReadLine().FirstOrDefault();
                if (choosen.Equals('Y'))
                {
                    downloader.SetDownloadTasks(downloadTasks);
                    downloader.DownloadLog += (a, b) => { WriteLog(b); };
                    downloader.DownloadCompleted += (a, b) =>
                    {
                        LaunchGame(handler.RefreshVersion(launchVer));
                        Console.WriteLine("下载完成");
                    };
                    downloader.StartDownload();
                }
                else
                {
                    LaunchGame(launchVer);
                }
            }
            else
            {
                LaunchGame(launchVer);
            }

            //NsisoLauncherCore.Util.Installer.ForgeInstaller forgeInstaller = new NsisoLauncherCore.Util.Installer.ForgeInstaller(
            //    PathManager.TempDirectory + "\\liteloader-installer-1.7.10-04.jar");
            //forgeInstaller.BeginInstall(handler.GameRootPath);


            //YggdrasilAuthenticator authenticator = new YggdrasilAuthenticator(new NsisoLauncherCore.Net.MojangApi.Endpoints.Credentials()
            //{
            //    Username = "timshsid@gmail.com",
            //    Password = "tim2001105"
            //})
            //{
            //    ProxyAuthServerAddress = new Uri("https://www.baidu.com")
            //};
            //var result = authenticator.DoAuthenticate();

            Console.ReadKey();
        }

        private static void Handler_GameExit(object sender, GameExitArg arg)
        {
            Console.WriteLine("The game is exit with code:{0}", arg.ExitCode);
            if (arg.ExitCode != 0)
            {
                CrashHelper crashHelper = new CrashHelper();
                crashHelper.GetCrashInfo(handler, arg);
            }
        }

        static void LaunchGame(Version launchVer)
        {
            Console.WriteLine("Ready to launch version {0}", launchVer.ID);
            OfflineAuthenticator authenticator = new OfflineAuthenticator("Nsiso");
            var launchResult = handler.LaunchAsync(new LaunchSetting()
            {
                Version = launchVer,
                AuthenticateResult = authenticator.DoAuthenticate()
            }).Result;
        }

        static void WriteLog(Log log)
        {
            Console.WriteLine("[{0}/{1}]{2}", log.LogLevel, DateTime.Now, log.Message);
            if (log.Exception != null)
            {
                Console.WriteLine(log.Exception);
            }
        }
    }
}
