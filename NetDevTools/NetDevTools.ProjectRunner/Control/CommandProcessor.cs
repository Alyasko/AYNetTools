using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetCrossRun.Core;
using NetDevTools.ProjectRunner.Control.Commands;
using NetDevTools.ProjectRunner.Helpers;

namespace NetDevTools.ProjectRunner.Control
{
    public class CommandProcessor
    {
        private readonly SolutionManager _solutionManager;
        private readonly AppContext _context;
        private readonly CommandsLookup _basicCommandsLookup = new CommandsLookup();

        private readonly List<Process> _runningProcesses = new List<Process>();

        public CommandProcessor(AppContext context, SolutionManager solutionManager)
        {
            _solutionManager = solutionManager;
            _context = context;

            InitBasicCommands();
            InitCustomCommands();

            ReadLine.AutoCompletionHandler = new AutocompletionHandler(context, solutionManager.Solution, _basicCommandsLookup);
        }

        private void InitCustomCommands()
        {
            foreach (var appConfigCommand in _context.AppConfig.Commands)
            {
                _basicCommandsLookup.Add(new ConfigCustomCommand(_solutionManager, appConfigCommand.Text, appConfigCommand, _runningProcesses));
            }
        }

        private void InitBasicCommands()
        {
            _basicCommandsLookup.Add(new ExitCommand());
            _basicCommandsLookup.Add(new KillAllProcessesCommand(_runningProcesses));
        }

        public void Run()
        {
            _runningProcesses.Clear();

            while (true)
            {
                var input = ReadLine.Read("Enter project name or command:\n> ");

                var command = _basicCommandsLookup[input];
                if (command != null)
                {
                    var breakLoop = command.Run(input, _context);
                    if(breakLoop)
                        break;
                }
                else
                {
                    var selectedProject = _solutionManager.FindProject(input);
                    if (selectedProject == null)
                    {
                        Console.WriteLine("Project or command not found.");
                        continue;
                    }

                    // Build and run.
                    if (selectedProject.BuildActionNeeded)
                    {
                        Console.Write("Build required. ");

                        if (ReadLineTools.Confirm("Run build?"))
                        {
                            _solutionManager.BuildSolution();
                            Console.WriteLine("Solution built. ");
                        }
                        else
                        {
                            Console.WriteLine($"Building '{selectedProject.Name}'");
                        }
                    }

                    var process = selectedProject.Run();
                    _runningProcesses.Add(process);
                }
            }
        }
    }
}
