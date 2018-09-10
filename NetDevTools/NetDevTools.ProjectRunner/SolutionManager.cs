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
        public const string DefaultSolutionName = "Prime.sln";
        private readonly DirectoryInfo _slnDirInfo;
        
        private List<FileInfo> _slnList;
        
        public Solution Solution { get; set; }

        public SolutionManager(DirectoryInfo dirInfo)
        {
            _slnDirInfo = dirInfo;
        }

        public IEnumerable<FileInfo> LoadSolutions()
        {
            _slnList = _slnDirInfo.GetFiles("*.sln").ToList();
            return _slnList;
        }

        public void LoadSolution(string slnFileName)
        {
            var slnFi = _slnList.FirstOrDefault(x => x.Name.Equals(slnFileName, StringComparison.OrdinalIgnoreCase));
            if(slnFi == null)
                throw new InvalidOperationException("Solution not found in list.");

            Solution = new Solution(slnFi.FullName);
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
