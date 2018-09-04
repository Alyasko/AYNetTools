using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDevTools.ProjectRunner
{
    public class SolutionManager
    {
        private readonly DirectoryInfo _slnDirInfo;

        public SolutionManager(DirectoryInfo dirInfo)
        {
            _slnDirInfo = dirInfo;
        }

        public IEnumerable<FileInfo> CheckSolutionFolder()
        {
            return _slnDirInfo.GetFiles("*.sln");
        }
    }
}
