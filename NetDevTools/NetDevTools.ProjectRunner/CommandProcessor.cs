using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace NetDevTools.ProjectRunner
{
    public class CommandProcessor
    {
        private readonly SolutionManager _solutionManager;

        public CommandProcessor(SolutionManager solutionManager)
        {
            _solutionManager = solutionManager;
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
                var input = ReadLine.Read("Enter project name: ");

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

                var selectedProject = _solutionManager.FindProject(input);
                if (selectedProject == null)
                {
                    Console.WriteLine("Project not found.");
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
