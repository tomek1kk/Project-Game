using Agent.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Agent
{
    public class AgentConfiguration
    {
        private static string pathToDefaultConfiguration = @"../../../Configuration\DefaultConfig.txt";
        public int CsPort { get; set; }
        public string CsIp { get; set; }
        public string TeamId { get; set; }
        public StrategyType Strategy { get; set; }

        public static AgentConfiguration ReadConfiguration(string[] programArguments)
        {
            if (programArguments.Length == 0)
                return useConfiguration(pathToDefaultConfiguration);
            else
                return useGivenConfiguration(programArguments);
        }

        private static AgentConfiguration useGivenConfiguration(string[] programArguments)
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

        private static AgentConfiguration useConfiguration(string pathToConfiguration)
        {
            var dir = Directory.GetCurrentDirectory();
            string fileContent = File.ReadAllText(pathToConfiguration);
            var tmp = JsonSerializer.Deserialize<AgentConfiguration>(fileContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return tmp;
        }
    }
}
