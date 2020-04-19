using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CommunicationServerNamespace
{
    public class Configuration
    {
        private static string pathToDefaultConfiguration = @".\Configuration\defaultConfiguration.json";

        public string CsIP { get; set; }
        public int GMPort { get; set; }
        public int AgentPort { get; set; }

        public static Configuration ReadConfiguration(string[] programArguments)
        {
            if (programArguments.Length == 0)
                return useConfiguration(pathToDefaultConfiguration);
            else
                return useGivenConfiguration(programArguments);
        }

        private static Configuration useGivenConfiguration(string[] programArguments)
        {
            try
            {
                return useConfiguration(programArguments[0]);
            }
            catch
            {
                Console.WriteLine("BAD GIVEN CONFIGURATION! INSTEAD MOCK CONFIGURATION USED.");
                return useConfiguration(pathToDefaultConfiguration);
            }
        }

        private static Configuration useConfiguration(string pathToConfiguration)
        {
            var dir = Directory.GetCurrentDirectory();
            string fileContent = File.ReadAllText(pathToConfiguration);
            var tmp = JsonSerializer.Deserialize<Configuration>(fileContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return tmp;
        }
    }
}
