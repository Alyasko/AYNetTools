using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NetCrossRun.Core;

namespace NetDevTools.ProjectRunner.Models
{
    public class Project
    {
        public Project(string name, string fullPath)
        {
            Name = name;
            ProjectFile = new FileInfo(fullPath);
            if (ProjectFile.Exists == false)
                throw new FileNotFoundException($"Project file '{fullPath}' does not exist.");

            ProjectType = ParseProjectType(ProjectFile);
            
            ProjectDirectory = ProjectFile.Directory;

            var appRuntimePath = Path.Combine(ProjectDirectory.FullName,
                Path.Combine(Path.Combine("bin", "debug"), "netcoreapp2.0"));
            AppRuntimeDirectory = new DirectoryInfo(appRuntimePath);

            if (AppRuntimeDirectory != null && AppRuntimeDirectory.Exists)
            {
                var exeDllFiles = AppRuntimeDirectory.EnumerateFiles().Where(x =>
                    x.Name.IndexOf(name + ".exe", StringComparison.OrdinalIgnoreCase) != -1 ||
                    x.Name.IndexOf(name + ".dll", StringComparison.OrdinalIgnoreCase) != -1).ToList();

                if (exeDllFiles.Any())
                {
                    ExecutableFile = exeDllFiles.FirstOrDefault();
                }
                else
                {
                    BuildActionNeeded = true;
                }
            }
            else
            {
                BuildActionNeeded = true;
            }
        }

        private ProjectType ParseProjectType(FileInfo projectFile)
        {
            var allText = File.ReadAllText(projectFile.FullName);
            var match = Regex.Match(allText, @"<TargetFramework>(?<type>.*?)<\/TargetFramework>");
            if (!match.Success)
                throw new InvalidOperationException($"Unable to find project type for project '{projectFile.Name}'");

            var projectType = match.Groups["type"].Value;

            if (projectType.Equals("netstandard2.0", StringComparison.OrdinalIgnoreCase))
                return ProjectType.DotNetStandardLib;

            if (projectType.Equals("netcoreapp2.0", StringComparison.OrdinalIgnoreCase))
                return ProjectType.DotNetCoreRunnable;

            return ProjectType.Undefined;
        }

        public string Name { get; set; }
        public FileInfo ProjectFile { get; set; }

        public DirectoryInfo AppRuntimeDirectory { get; set; }
        public FileInfo ExecutableFile { get; set; }
        public DirectoryInfo ProjectDirectory { get; set; }

        public bool BuildActionNeeded { get; set; }
        public ProjectType ProjectType { get; set; } = ProjectType.Undefined;

        public Process Run()
        {
            var command = $"dotnet run";
            return command.ExecuteCommand(new ProcessStartInfo()
            {
                WorkingDirectory = ProjectDirectory.FullName,
                CreateNoWindow = false,
                UseShellExecute = true
            });
        }
    }
}