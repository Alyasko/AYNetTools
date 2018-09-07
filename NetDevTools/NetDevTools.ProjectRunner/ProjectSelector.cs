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
        }

        public Project SelectProject()
        {
            var prevInput = "";

            Project selectedProject = null;
            
            while (true)
            {
                Console.WriteLine("Enter project name: ");
                Console.Write(prevInput);
                
                var projPart = prevInput + Console.ReadLine();
                var projPossible = _solutionManager.FindProject(projPart).ToList();
                var projCount = projPossible.Count;

                if (projCount == 0)
                {
                    Console.WriteLine("No projects found.");
                }
                else if (projCount == 1)
                {
                    Console.WriteLine($"Project selected ({projPossible.First().Name})");
                    prevInput = "";
                    selectedProject = projPossible.First();
                    
                    break;
                }
                else
                {
                    foreach (var possibleProject in projPossible)
                    {
                        Console.WriteLine(possibleProject.ProjectFile);
                    }

                    prevInput = projPart;
                }
            }

            return selectedProject;
        }
    }
}
