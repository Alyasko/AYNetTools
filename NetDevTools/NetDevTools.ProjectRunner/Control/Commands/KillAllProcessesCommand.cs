using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NetDevTools.ProjectRunner.Helpers;

namespace NetDevTools.ProjectRunner.Control.Commands
{
    class KillAllProcessesCommand : ITextCommand
    {
        private readonly List<Process> _runningProcesses;

        public KillAllProcessesCommand(List<Process> runningProcesses)
        {
            _runningProcesses = runningProcesses;
        }

        public string Text => "kill all processes";
        public string Description => "Kills all processes executed by this application";
        public bool Run(string command, AppContext ctx)
        {
            foreach (var proc in _runningProcesses)
                ProcessTools.KillProcessAndChildren(proc.Id);
            _runningProcesses.Clear();

            return false;
        }
    }
}
