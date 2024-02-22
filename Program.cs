using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;
using IronGitHub;
using System.Net.Http;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Lab5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int option;

            while (true)
            {
                Console.WriteLine("Welcome to the AllTalk Text to Speech App! To contiune, " +
               "please enter 1 to start the AllTalk Service, and 2 to install AllTalk.(Installing is required for first time use; this may take a while)");
                option = int.Parse(Console.ReadLine());

                if (option >= 1 && option <= 2)
                {
                    if (option == 1)
                    {
                        string sourceDir = @"C:\Users\swift\Documents\GitHub\alltalktts\alltalktts";
                        string condaActivateCmd = @"C:\WINDOWS\system32\cmd.exe";
                        string condaActivateArgs = $"/C C:\\Users\\swift\\Documents\\GitHub\\alltalktts\\alltalktts\\alltalk_environment\\conda\\condabin\\conda.bat activate C:\\Users\\swift\\Documents\\GitHub\\alltalktts\\alltalktts\\alltalk_environment\\env";
                        string allTalkCmd = Path.Combine(sourceDir, "start_alltalk.bat");

                        ProcessStartInfo startCondaInfo = new ProcessStartInfo
                        {
                            FileName = condaActivateCmd,
                            Arguments = condaActivateArgs,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        };

                        Process startCondaProcess = new Process
                        {
                            StartInfo = startCondaInfo
                        };

                        startCondaProcess.Start();
                        startCondaProcess.WaitForExit(); // Wait for the conda activation process to complete

                        ProcessStartInfo startAllTalkInfo = new ProcessStartInfo
                        {
                            FileName = allTalkCmd,
                            Arguments = "allTalkTTS",
                            CreateNoWindow = false,
                            UseShellExecute = true
                        };

                        Process startAllTalkProcess = new Process
                        {
                            StartInfo = startAllTalkInfo
                        };
                        startAllTalkProcess.Start();


                        //check the service
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("http://127.0.0.1:7851/api/ready");

                        HttpResponseMessage response = client.GetAsync("ready").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Service is ready!");
                        }
                        else
                        {
                            Console.WriteLine("Service is not ready!");
                        }


                        client.Get("voices")
                .EnsureSuccessStatusCode()
                .GetAwaiter().GetResult()
                .Dump();

                        string json = client.GetStringAsync("voices")
                                .GetAwaiter()
                                .GetResult();

                        JObject obj = JObject.Parse(json)!;
                        JArray voiceArray = (JArray)obj["voices"]!;
                        List<string> voiceList = new List<string>();

                        foreach (JObject voiceItem in voiceArray)
                        {
                            Voice voice = new Voice
                            {
                                Name = (string)voiceItem["name"]!,
                                Path = (string)voiceItem["path"]!
                            };
                            voiceList.Add(voice);
                        }

                        foreach (Voice voice in voiceList)
                        {
                            Console.Out.WriteLine($"Name: {voice.Name} | Path: {voice.Path}");
                        }


                    }
                    else if (option == 2)
                    {
                        Console.WriteLine("Installing AllTalk");
                        // Git github = new Git();
                        // github.InitializeSourceUrl(@"https://github.com/erew123/alltalk_tts.git, C:\Users\swift\OneDrive - East Tennessee State University" +
                        //     @"\Lab5\AllTalkGit");

                        //will go through the install process in a new window

                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = @"C:\Users\swift\OneDrive - East Tennessee State University\Lab5\AllTalkGit\alltalk_tts\atsetup.bat",
                            CreateNoWindow = true, // Set this to true to open in a new window
                            UseShellExecute = true
                        };

                        Process.Start(startInfo);

                        Console.WriteLine("Done. Please press enter to start the AllTalk service:");
                        Console.ReadLine();

                    }
                    else
                    {
                        Console.WriteLine("Please select an option between 1 and 2.");
                    }
                }

            }




            /*
           var startInfo = new ProcessStartInfo
           {
               FileName = "cmd.exe",
               RedirectStandardInput = true,
               RedirectStandardOutput = true,
               UseShellExecute = false,
               CreateNoWindow = false,
           };

           var process = new Process { StartInfo = startInfo };

           process.Start();
           process.StandardInput.WriteLine(@"cd C:\Users\swift\Documents\GitHub\alltalktts\alltalktts");
           process.StandardInput.WriteLine(@"set CONDA_ROOT_PREFIX=C:\Users\swift\Documents\GitHub\alltalktts\alltalktts\alltalk_environment\conda ");
           process.StandardInput.WriteLine(@"set INSTALL_ENV_DIR=C:\Users\swift\Documents\GitHub\alltalktts\alltalktts\alltalk_environment\env ");
           process.StandardInput.WriteLine(@"call C:\Users\swift\Documents\GitHub\alltalktts\alltalktts\alltalk_environment\conda\condabin\conda.bat");
           process.StandardInput.WriteLine(@"activate C:\Users\swift\Documents\GitHub\alltalktts\alltalktts\alltalk_environment\env" );
           process.StandardInput.WriteLine(@"start_alltalk.bat");

            
           */





           Console.WriteLine("Done. running");













            /*
                        var config = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

                        config.GetSection("GitHub").Bind("access_token");
                        var accessToken = config["GitHub:AccessToken"] as string;
                        if (int.TryParse(accessToken, out int accessTokenInt) == false)
                        {
                            throw new Exception("Invalid access token");
                        }
                        var token = accessTokenInt;


                        var client = new GitHubClient(token);
                    var cloneUrl = "git@github.com:user/repository.git";

                    string pathToClone = @"C:\Projects\MyCsharpapp\repos\";
                    string folderName = "folder-name";
                    string clonePath = Path.Combine(pathToClone, folderName);

                    await client.CloneAsync(cloneUrl, clonePath, "master");
            */


        }






    }
}
