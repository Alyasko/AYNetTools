using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NetCrossRun.Core;
using NetDevTools.ProjectRunner.Configuration;

namespace NetDevTools.ProjectRunner
{
    public class App
    {
        public void Start(string[] args)
        {
            Console.WriteLine("--- NetDevTools.ProjectRunner ---");
            Console.WriteLine("Supports only .NET Core applications.");
            Console.WriteLine("Print 'exit' to exit the program.");
            Console.WriteLine("Print 'kill all' to kill all running processes.");

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

            var solutionName = ReadLine.Read($"Enter solution name (default '{Config.I.DefaultSolutionName}'): ", Config.I.DefaultSolutionName);
            if (string.IsNullOrWhiteSpace(solutionName))
                throw new InvalidOperationException("Solution name is empty.");

            var solution = solutionManager.SelectSolution(solutionName);
            ReadLine.AutoCompletionHandler = new AutocompletionHandler(solutionManager.Solution);

            // List projects.
            Console.WriteLine($"Loaded {solution.Projects.Count()} projects.");

            // Run command processor.
            var cmdProcessor = new CommandProcessor(solutionManager);
            cmdProcessor.Run();
        }
    }
}