using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NetCrossRun.Core;
using NetDevTools.ProjectRunner.Models;

namespace NetDevTools.ProjectRunner
{
    public class SolutionManager
    {
        private readonly AppContext _context;
        private readonly DirectoryInfo _slnDirInfo;
        
        public List<FileInfo> Solutions { get; set; }
        
        public Solution Solution { get; set; }

        public SolutionManager(AppContext context)
        {
            _context = context;
            _slnDirInfo = context.SolutionDirectory;
        }

        public void LoadSolutions()
        {
            Solutions = _slnDirInfo.GetFiles("*.sln").ToList();
            if (!Solutions.Any())
                throw new InvalidOperationException("No .sln files found in specified directory.");
        }

        public void SelectSolution(string slnFileName)
        {
            var slnFi = Solutions.FirstOrDefault(x => x.Name.Equals(slnFileName, StringComparison.OrdinalIgnoreCase));
            Solution = new Solution(slnFi);
        }

        public Project FindProject(string name)
        {
            return Solution.Projects.FirstOrDefault(x => x.Name.Equals(name));
        }

        public void BuildSolution()
        {
            $"dotnet build {Solution.SolutionFile.FullName}".ExecuteCommand().WaitForExit();
        }
    }
}
