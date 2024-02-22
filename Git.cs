using System;
using System.Diagnostics;
namespace Lab5
{
    public class Git
    {
        private string folderPath = @"C:\Users\swift\OneDrive - East Tennessee State University\Lab5\AllTalkGit";
        private string clonePath;

        public string SourceUrl { get; private set; }

        public void InitializeSourceUrl(string sourceUrl)
        {
            SourceUrl = sourceUrl;
            CloneRepository();
        }

        public void CloneRepository()
        {
            try
            {
                var startInfo = new ProcessStartInfo(@"C:\Program Files\Git\cmd\git.exe", $"clone {SourceUrl}");
                Process.Start(startInfo);


                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }

            if (Directory.Exists(clonePath))
            {
                Directory.Delete(clonePath, true);
            }
        }
    }



}
