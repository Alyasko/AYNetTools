using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDevTools.ProjectRunner.Control
{
    public class CommandsLookup
    {
        private readonly Dictionary<string, ITextCommand> _commands = new Dictionary<string, ITextCommand>();

        public void Add(ITextCommand command)
        {
            _commands.Add(command.Text.ToLower(), command);
        }

        public ITextCommand this[string command]
        {
            get
            {
                var lowerCommand = command.ToLower();
                return _commands.ContainsKey(lowerCommand) ? _commands[lowerCommand]: null;
            }
        }

        public string[] Keys => _commands.Keys.ToArray();
    }
}
