using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NetCrossRun.Core;
using NetDevTools.ProjectRunner.Configuration;

namespace NetDevTools.ProjectRunner.Control.Commands
{
    class ConfigCustomCommand : ITextCommand
    {
        private readonly string _text;
        private readonly List<Process> _runningProcesses;
        private readonly SolutionManager _slnManager;

        private readonly string _concatCommand;

        public ConfigCustomCommand(SolutionManager slnManager, string text, CustomCommand customCommand, List<Process> runningProcesses)
        {
            _slnManager = slnManager;
            _text = text;
            _runningProcesses = runningProcesses;

            _concatCommand = string.Join(" && ", customCommand.Commands);
        }

        public string Text => _text;
        public string Description => null;
        public bool Run(string command, AppContext ctx)
        {
            var process = _concatCommand.ExecuteCommand(new ProcessStartInfo()
            {
                WorkingDirectory = _slnManager.Solution.SolutionDirectory.FullName,
                CreateNoWindow = false,
                UseShellExecute = true
            });

            _runningProcesses.Add(process);
            return false;
        }
    }
}
