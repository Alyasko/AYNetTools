using System;
using System.Collections.Generic;
using System.Text;

namespace NetDevTools.ProjectRunner.Configuration
{
    public class CustomCommand
    {
        public string Text { get; set; }
        public string Command { get; set; }
    }

    public class Config
    {
        public List<CustomCommand> Commands { get; set; }
        public string DefaultSolutionName { get; set; }
    }
}
