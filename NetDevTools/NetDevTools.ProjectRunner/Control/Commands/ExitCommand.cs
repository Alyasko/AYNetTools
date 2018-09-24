using System;

namespace NetDevTools.ProjectRunner.Control.Commands
{
    class ExitCommand : ITextCommand
    {
        public string Text => "exit";
        public string Description => "Close the application";
        public bool Run(string command, AppContext ctx)
        {
            return true;
        }
    }
}
