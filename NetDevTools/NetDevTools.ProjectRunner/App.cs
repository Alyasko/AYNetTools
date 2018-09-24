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
using NetDevTools.ProjectRunner.Helpers;
using CommandProcessor = NetDevTools.ProjectRunner.Control.CommandProcessor;

namespace NetDevTools.ProjectRunner
{
    public class App
    {
        private void Log(string message)
        {
            Console.WriteLine(message);
        }

        private void NewLine()
        {
            Console.WriteLine();
        }

        public void Start(string[] args)
        {
            try
            {
                StartInternal(args);
            }
            catch (Exception e)
            {
                Log($"Error occurred: {e.Message}");
            }
        }

        private void StartInternal(string[] args)
        {
            Log("--- NetDevTools.ProjectRunner ---");
            Log("Supports running only .NET Core applications.");
            NewLine();
            Log("> Print 'exit' to exit the program.");
            Log("> Print 'kill all' to kill all running processes.");
            NewLine();

            var context = AppContext.Create(args);
            Log($"Using config file '{context.ConfigFile.FullName}'.");
            Log($"App folder is '{context.SolutionDirectory.FullName}'");
            NewLine();

            // Load solution.
            var solutionManager = new SolutionManager(context);
            solutionManager.LoadSolutions();

            // Print out found solutions.
            foreach (var slnFile in solutionManager.Solutions)
                Log($"Found solution: {slnFile.Name}");

            // Read solution name.
            var solutionName = ReadLine.Read($"Enter solution name (default '{context.AppConfig.DefaultSolutionName}'): ", context.AppConfig.DefaultSolutionName);
            if (string.IsNullOrWhiteSpace(solutionName))
                throw new InvalidOperationException("Solution name is empty.");

            solutionManager.SelectSolution(solutionName);

            Log($"Loaded {solutionManager.Solution.Projects.Count()} projects.");

            // Run command processor.
            var cmdProcessor = new CommandProcessor(context, solutionManager);
            cmdProcessor.Run();
        }
    }
}