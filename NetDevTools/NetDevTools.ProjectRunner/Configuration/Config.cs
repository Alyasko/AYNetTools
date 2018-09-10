using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

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

        public static Config Default = new Config()
        {
            DefaultSolutionName = "Prime.sln",
            Commands = new List<CustomCommand>()
        };

        private static Config LoadConfig()
        {
            var fileName = "config.json";
            Config result = null;

            if (File.Exists(fileName))
            {
                var data = File.ReadAllText("config.json");
                result = JsonConvert.DeserializeObject<Config>(data);
            }
            else
            {
                result = Config.Default;

                var configSerialized = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
                File.WriteAllText(fileName, configSerialized);
            }

            return result;
        }

        private static readonly Lazy<Config> _i = new Lazy<Config>(LoadConfig);
        public static Config I => _i.Value;
    }
}
