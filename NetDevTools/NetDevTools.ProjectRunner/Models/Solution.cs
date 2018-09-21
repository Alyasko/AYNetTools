using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDevTools.ProjectRunner.Models
{
    public class Solution
    {
        public Solution(FileInfo slnFile)
        {
            SolutionFile = slnFile;
            if (SolutionFile.Exists == false)
                throw new FileNotFoundException("Solution file not found.");

            SolutionDirectory = SolutionFile.Directory;
            
            LoadProjects();
        }

        private void LoadProjects()
        {
            var projectLoader = new ProjectLoader(SolutionDirectory);
            Projects = projectLoader.Load(SolutionFile).ToList();
        }
        
        public FileInfo SolutionFile { get; set; }
        public DirectoryInfo SolutionDirectory { get; set; }
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }
}