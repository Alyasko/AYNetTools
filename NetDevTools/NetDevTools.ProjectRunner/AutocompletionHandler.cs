﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetDevTools.ProjectRunner.Configuration;
using NetDevTools.ProjectRunner.Control;
using NetDevTools.ProjectRunner.Models;

namespace NetDevTools.ProjectRunner
{
    public class AutocompletionHandler : IAutoCompleteHandler
    {
        private readonly Solution _solution;
        private readonly AppContext _context;

        private readonly CommandsLookup _commandsLookup;

        public AutocompletionHandler(AppContext context, Solution solution, CommandsLookup commandsLookup)
        {
            _solution = solution;
            _commandsLookup = commandsLookup;
            _context = context;
        }

        // characters to start completion from
        public char[] Separators { get; set; } = new char[] {' '};

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
            var suggestions = _solution.Projects.Where(x => x.ProjectType == ProjectType.DotNetCoreRunnable)
                .Select(x => x.Name)
                .Concat(_commandsLookup.Keys);

            return suggestions.Where(x => x.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1).OrderBy(x => x.Length).ToArray();
        }
    }
}