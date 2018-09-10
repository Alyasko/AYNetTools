using System;

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