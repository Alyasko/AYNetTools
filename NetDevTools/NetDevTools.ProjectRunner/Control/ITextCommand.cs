using System;

namespace NetDevTools.ProjectRunner.Control
{
    public interface ITextCommand
    {
        string Text { get; }
        string Description { get; }
        
        /// <summary>
        /// Returns true if command loop should break.
        /// </summary>
        bool Run(string command, AppContext ctx);
    }
}
