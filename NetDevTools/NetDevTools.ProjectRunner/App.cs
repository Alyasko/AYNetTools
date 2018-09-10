using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NetCrossRun.Core;

namespace NetDevTools.ProjectRunner
{
    public class App
    {
        public void Start(string[] args)
        {
            Console.WriteLine("--- NetDevTools.ProjectRunner ---");
            Console.WriteLine("Supports only .NET Core applications.");
            Console.WriteLine("Print 'exit' to exit the program.");

            // Open sln folder.
            var slnPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (args.Length > 0)
                slnPath = args[0];

            var slnDir = new DirectoryInfo(slnPath);
            if (!slnDir.Exists)
                throw new DirectoryNotFoundException("Specified directory not found.");

            Console.WriteLine($"App folder is '{slnDir.Name}'");

            // Load solution.
            var solutionManager = new SolutionManager(slnDir);
            if (!solutionManager.Solutions.Any())
                throw new InvalidOperationException("No .sln files found in directory.");

            foreach (var slnFile in solutionManager.Solutions)
                Console.WriteLine($"Found: {slnFile.Name}");

            var solution = solutionManager.SelectSolution(SolutionManager.DefaultSolutionName);
            ReadLine.AutoCompletionHandler = new AutocompletionHandler(solutionManager.Solution);

            // List projects.
            Console.WriteLine($"Loaded {solution.Projects.Count()} projects.");

            while (true)
            {
                var input = ReadLine.Read("Enter project name: ");

                if(input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;

                var selectedProject = solutionManager.FindProject(input);
                if (selectedProject == null)
                {
                    Console.WriteLine("Project not found.");
                    continue;
                }

                // Build and run.
                Console.Write($"{selectedProject.Name} selected. ");
                if (selectedProject.BuildActionNeeded)
                {
                    Console.Write("Build required. ");

                    if (ReadLineTools.Confirm("Run build?"))
                    {
                        solutionManager.BuildSolution();
                        Console.WriteLine("Solution built. ");
                    }
                    else
                    {
                        Console.WriteLine($"Building '{selectedProject.Name}'");
                    }
                }

                if (ReadLineTools.Confirm($"Run project '{selectedProject.Name}'?"))
                {
                    var command = $"dotnet run";
                    command.ExecuteCommand(new ProcessStartInfo()
                    {
                        WorkingDirectory = selectedProject.ProjectDirectory.FullName,
                        CreateNoWindow = false,
                        UseShellExecute = true
                    });
                }
            }
        }
    }
}