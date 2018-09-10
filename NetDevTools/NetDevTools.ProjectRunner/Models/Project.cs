using System;
using System.IO;
using System.Linq;

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
            var allLines = projectFile.Re
            
        }

        public string Name { get; set; }
        public FileInfo ProjectFile { get; set; }

        public DirectoryInfo AppRuntimeDirectory { get; set; }
        public FileInfo ExecutableFile { get; set; }
        public DirectoryInfo ProjectDirectory { get; set; }

        public bool BuildActionNeeded { get; set; }
        public ProjectType ProjectType { get; set; } = ProjectType.Undefined;
    }
}