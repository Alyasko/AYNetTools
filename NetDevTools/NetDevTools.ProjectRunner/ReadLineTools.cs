using System;
using System.Diagnostics;
using System.Management;

namespace NetDevTools.ProjectRunner
{
    public static class ReadLineTools
    {
        public static bool Confirm(string question)
        {
            return ReadLine.Read($"{question} (Y/N, default Y): ", "Y").Equals("y", StringComparison.OrdinalIgnoreCase);
        }
    }
}