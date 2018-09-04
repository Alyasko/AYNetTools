using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NetDevTools.ProjectRunner
{
    public class App
    {
        public void Start(string[] args)
        {
            Console.WriteLine("--- NetDevTools.ProjectRunner ---");

            var slnPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (args.Length > 0)
                slnPath = args[0];

            var slnDir = new DirectoryInfo(slnPath);

            if(!slnDir.Exists)
                throw new DirectoryNotFoundException("Specified directory not found.");

            Console.WriteLine($"App folder is '{slnDir.Name}'");

            var solutionManager = new SolutionManager(slnDir);

            var slnFiles = solutionManager.CheckSolutionFolder().ToList();
            if (!slnFiles.Any())
                throw new InvalidOperationException("No .sln files found in directory.");

            foreach (var slnFile in slnFiles)
                Console.WriteLine($"Found: {slnFile.Name}");
        }
    }
}
