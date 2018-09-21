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
        public string[] Commands { get; set; }
    }

    public class Config
    {
        public List<CustomCommand> Commands { get; set; }
        public string DefaultSolutionName { get; set; }

        public static Config Default = new Config()
        {
            DefaultSolutionName = "Prime.sln",
            Commands = new List<CustomCommand>()
            {
                new CustomCommand()
                {
                    Text = "ng prime.manager.client run",
                    Commands = new string[]
                    {
                        "cd \\Ext\\Prime.Manager.Client\\ui && ng serve"
                    }
                }
            }
        };

        public static Config Load(FileInfo file)
        {
            Config result = null;

            if (file.Exists)
            {
                var data = File.ReadAllText(file.FullName);
                result = JsonConvert.DeserializeObject<Config>(data);
            }
            else
            {
                result = Config.Default;

                var configSerialized = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
                File.WriteAllText(file.FullName, configSerialized);
            }

            return result;
        }
    }
}
