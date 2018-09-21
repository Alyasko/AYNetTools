using System;
using System.IO;
using System.Reflection;
using NetDevTools.ProjectRunner.Configuration;
using NetDevTools.ProjectRunner.Helpers;

namespace NetDevTools.ProjectRunner
{
    public class AppContext
    {
        private AppContext() { }

        public static AppContext Create(string[] args)
        {
            var context = new AppContext();

            var slnPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configFileName = AppConstants.DefaultConfigName;
            if (args.Length > 0)
                slnPath = args[0];
            if (args.Length > 1)
                configFileName = args[1];

            context.SolutionDirectory = InitSolutionDirectory(slnPath);
            context.ConfigFile = InitConfigFile(configFileName);
            context.AppConfig = Config.Load(context.ConfigFile);

            return context;
        }

        private static FileInfo InitConfigFile(string configFileName)
        {
            if (string.IsNullOrWhiteSpace(configFileName))
                throw new InvalidOperationException("Invalid config file name.");

            var configFile = new FileInfo(configFileName);
            if (!configFile.Exists)
                throw new FileNotFoundException("Specified config file does not exist.");

            return configFile;
        }

        private static DirectoryInfo InitSolutionDirectory(string slnPath)
        {
            if (string.IsNullOrWhiteSpace(slnPath))
                throw new InvalidOperationException("Invalid solution directory path string.");

            var slnDi = new DirectoryInfo(slnPath);
            if (!slnDi.Exists)
                throw new DirectoryNotFoundException("Specified solution directory does not exist.");

            return slnDi;
        }

        public DirectoryInfo SolutionDirectory { get; set; }
        public FileInfo ConfigFile { get; set; }
        public Config AppConfig { get; set; }
    }
}
