using System;
using System.Linq;
using NetDevTools.ProjectRunner.Models;

namespace NetDevTools.ProjectRunner
{
    public class AutocompletionHandler : IAutoCompleteHandler
    {
        private readonly Solution _solution;

        public AutocompletionHandler(Solution solution)
        {
            _solution = solution;
        }

        // characters to start completion from
        public char[] Separators { get; set; } = new char[] {' '};

        // text - The current text entered in the console
        // index - The index of the terminal cursor within {text}
        public string[] GetSuggestions(string text, int index)
        {
//            x => x.ExecutableFile != null &&
//                 x.ExecutableFile.FullName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1
//                 || x.ProjectFile.FullName.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1
            
            return _solution.Projects.Where(
                x => x.Name.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1
            ).Select(x => x.Name).ToArray();
        }
    }
}