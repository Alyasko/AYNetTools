using System;
using System.Linq;
using NetDevTools.ProjectRunner.Models;

namespace NetDevTools.ProjectRunner
{
    public class ProjectSelector
    {
        private readonly SolutionManager _solutionManager;

        public ProjectSelector(SolutionManager solutionManager)
        {
            _solutionManager = solutionManager;
            ReadLine.AutoCompletionHandler = new AutocompletionHandler(solutionManager.Solution);
        }

        public Project SelectProject()
        {
            var prevInput = "";

            Project selectedProject = null;

            while (true)
            {
                var projName = ReadLine.Read("Enter project name: ");

                selectedProject = _solutionManager.FindProject(projName);
                if (selectedProject == null)
                    Console.WriteLine("Project not found.");
                else
                    break;
            }

            return selectedProject;
        }
    }
}