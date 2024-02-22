using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Json;
using IronGitHub;
using System.Net.Http;

using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Lab5
{
    internal class Program
    {
        static async Task Main()
        {
            int option;

            while (true)
            {
                Console.WriteLine($"Welcome to the AllTalk Text to Speech App! Please enter one of the three options below: \n1. Start the AllTalk Service  " +
                    $"\n2. install AllTalk(required for first time use) \n3. Enter voicelines (after the service has started)");     
                option = int.Parse(Console.ReadLine());

                if (option >= 1 && option <= 3)
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


                    else if (option == 3)
                    {

                        //check the service
                        await CheckServiceReady();

                        Console.WriteLine("Please press enter to contiune:");
                        Console.ReadLine();
                        await Request();

                    }
                    else
                    {
                        Console.WriteLine("Please select an option between 1 and 2.");
                    }
                }

            }


            static async Task CheckServiceReady()
            {
                string apiUrl = "http://127.0.0.1:7851/api/ready";

                while (true)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                           
                            HttpResponseMessage response = await client.GetAsync(apiUrl);

                            // check if request was successful
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Service is ready!");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Service is not ready! Will try again in 10 seconds.");

                                
                                await Task.Delay(TimeSpan.FromSeconds(10));
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        
                        Console.WriteLine($"Exception: {ex.Message}. Will try again in 10 seconds.");

                        // wait for 10 seconds before retrying
                        await Task.Delay(TimeSpan.FromSeconds(10));
                    }
                }
            }




            static async Task Request()
            {

                string apiUrl = "http://127.0.0.1:7851/api/tts-generate";

                try
                {


                    Console.Write("Please enter the text you would like to convert to speech: ");
                    string textInput = Console.ReadLine();

                    Console.Write("Please enter the character voice (e.g., female_01.wav): ");
                    string characterVoice = Console.ReadLine();

                    using (HttpClient client1 = new HttpClient())
                    {
                        // Prepare the form data
                        var formData = new Dictionary<string, string>
                                        {
                                            { "text_input", textInput },
                                            { "text_filtering", "standard" },
                                            { "character_voice_gen", characterVoice },
                                            { "narrator_enabled", "false" },
                                            { "narrator_voice_gen", "male_01.wav" },
                                            { "text_not_inside", "character" },
                                            { "language", "en" },
                                            { "output_file_name", "myoutputfile" },
                                            { "output_file_timestamp", "true" },
                                            { "autoplay", "true" },
                                            { "autoplay_volume", "0.6" }
                                        };

                        // Create the HTTP content with form data
                        var content = new FormUrlEncodedContent(formData);

                        // Make POST request
                        HttpResponseMessage response1 = await client1.PostAsync(apiUrl, content);

                        // Check if request was successful
                        if (response1.IsSuccessStatusCode)
                        {
                            // Handle successful response
                            Console.WriteLine("Request successful!");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {response1.StatusCode} - {response1.ReasonPhrase}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }







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
