using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using WatiN.Core;

namespace FunnelWeb.Tests.Helpers
{
    public abstract class IntegrationTest
    {
        private static int Port = 10091;
        private readonly TheDatabase requirements;
        protected static string DestinationPath;
        private static Process webServerProcess;
        protected static TemporaryDatabase Database { get; private set; }
        
        static IntegrationTest()
        {
            DeployTheWebApplication();
            StartWebServer();
        }

        protected IntegrationTest(TheDatabase requirements)
        {
            this.requirements = requirements;
        }

        protected abstract void Execute();

        public int Rand()
        {
            return new Random(DateTime.Now.Millisecond).Next(1, 1000);
        }

        [Test]
        public void Run()
        {
            if ((requirements & TheDatabase.MustBeFresh) == TheDatabase.MustBeFresh || Database == null)
            {
                if (Database != null)
                    Database.Dispose();

                Database = new TemporaryDatabase();
                Database.CreateAndDeploy();

                UpdateConnectionStringInWebConfig(Database.ConnectionString);
            }

            Browser = new IE();
            Execute();
        }

        protected IE Browser { get; set; }

        protected void LogIn()
        {
            Browser.GoTo(RootUrl + "login");

            Assert.IsTrue(Browser.ContainsText("log in"));

            Browser.TextField(Find.ByName("Username")).AppendText("test");
            Browser.TextField(Find.ByName("Password")).AppendText("test");

            Browser.Button(Find.ByValue("Submit")).Click();

            Browser.WaitUntilContainsText("Admin");
            Browser.WaitForComplete();
        }

        protected void LogOut()
        {
            if (Browser.ContainsText("Logout"))
            {
                Browser.Link(Find.ByText("Logout")).Click();
            }
        }

        private static void StartWebServer()
        {
            Port = 10091;

            var exe = typeof(IntegrationTest).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\");
            exe = Path.GetDirectoryName(exe);  // Debug
            exe = Path.Combine(exe, "WebDev.WebServer40.exe");

            webServerProcess = Process.Start(exe, string.Format("/port:{0} /silent /vpath:\"/\" /path:\"{1}\"", Port, DestinationPath));
        }

        private void DeleteWebFiles()
        {
            
        }

        protected static string RootUrl
        {
            get { return string.Format("http://127.0.0.1:{0}/", Port); }
        }

        private static void UpdateConnectionStringInWebConfig(string connectionString)
        {
            var path = Path.Combine(DestinationPath, "web.config");
            var contents = File.ReadAllText(path);
            contents = Regex.Replace(contents, "funnelweb\\.configuration\\.database\\.connection\\\"\\svalue=\\\"(.+?)\\\"", x => x.Value.Replace(x.Groups[1].Value, connectionString));
            File.WriteAllText(path, contents);
        }

        private static void DeployTheWebApplication()
        {
            var sourcePath = typeof(IntegrationTest).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\");
            sourcePath = Path.GetDirectoryName(sourcePath);  // Debug
            sourcePath = Path.GetDirectoryName(sourcePath);  // bin
            sourcePath = Path.GetDirectoryName(sourcePath);  // Project
            sourcePath = Path.GetDirectoryName(sourcePath);  // Solution
            sourcePath = Path.Combine(sourcePath, "FunnelWeb.Web");

            DestinationPath = typeof(IntegrationTest).Assembly.CodeBase.Replace("file:///", "").Replace("/", "\\");
            DestinationPath = Path.GetDirectoryName(DestinationPath);  // Debug
            DestinationPath = Path.Combine(DestinationPath, "FunnelWeb.WebTesting");
            Directory.CreateDirectory(DestinationPath);

            // Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourcePath, DestinationPath));

            // Copy all the files
            foreach (var newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, DestinationPath));                    
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("Could not copy file: '{0}'. Reason: {1}", newPath, ex));
                }
            }
        }
    }
}
