using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace NetDevTools.ProjectRunner.Models
{
    public class ProjectLoader
    {
        private readonly DirectoryInfo _solutionDirectory;
        
        public ProjectLoader(DirectoryInfo solutionDirectory)
        {
            _solutionDirectory = solutionDirectory;
        }

        public IEnumerable<Project> Load(FileInfo slnFileInfo)
        {
            var projects = new List<Project>();
            
            var slnText = File.ReadAllText(slnFileInfo.FullName);

            var matches = Regex.Matches(slnText,
                "Project\\(\\\"(?<guid>.*)\\\"\\)\\s+=\\s+\\\"(?<name>.*?)\\\",\\s+\\\"(?<path>.*?)\\\",\\s\\\"(?<projGuid>.*?)\\\"");
            
            foreach (Match match in matches)
            {
                var relPath = match.Groups["path"].Value;
                var projName = match.Groups["name"].Value;

                relPath = relPath.Replace("\\", Path.DirectorySeparatorChar.ToString());
                
                var fullPath = Path.Combine(_solutionDirectory.FullName, relPath);

                try
                {
                    projects.Add(new Project(projName, fullPath));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Unable to load project {projName}.");
                }
            }

            return projects;
        }
    }
}