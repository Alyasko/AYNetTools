using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using NetCrossRun.Core;
using NetDevTools.ProjectRunner.Configuration;

namespace NetDevTools.ProjectRunner
{
    public class CommandProcessor
    {
        private readonly SolutionManager _solutionManager;
        private readonly AppContext _context;

        public CommandProcessor(AppContext context, SolutionManager solutionManager)
        {
            _solutionManager = solutionManager;
            _context = context;
        }

        private void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                if(!proc.HasExited)
                    proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

        public void Run()
        {
            var processes = new List<Process>();

            while (true)
            {
                var input = ReadLine.Read("Enter project name or command:\n> ");

                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;
                if (input.Equals("kill all", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var proc in processes)
                    {
                        KillProcessAndChildren(proc.Id);
                    }
                    processes.Clear();
                    continue;
                }

                var customCommand =
                    _context.AppConfig.Commands.FirstOrDefault(x => x.Text.Equals(input, StringComparison.OrdinalIgnoreCase));
                if (customCommand != null)
                {
                    var concatCommands = string.Join(" && ", customCommand.Commands);
                    processes.Add(concatCommands.ExecuteCommand(new ProcessStartInfo()
                    {
                        WorkingDirectory = _solutionManager.Solution.SolutionDirectory.FullName,
                        CreateNoWindow = false,
                        UseShellExecute = true
                    }));
                    continue;
                }

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
                processes.Add(process);
            }
        }
    }
}
