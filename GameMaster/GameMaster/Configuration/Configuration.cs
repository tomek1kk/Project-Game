using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GameMaster.Configuration
{
    /// <summary>
    /// Reads arguments of application. In Visual Studio you can specify it by clicking: 'Debug'->GameMaster Properties->Application arguments.
    /// The argument is path to configuration. If there is no arguments then use default.
    /// </summary>
    public class GMConfiguration
    {
        private static string pathToDefaultConfiguration = @".\Configuration\defaultConfiguration.txt";

        public int BoardX { get; set; }
        public int BoardY { get; set; }
        public int MovePenalty { get; set; }
        public int DiscoveryPenalty { get; set; }
        public int PutPenalty { get; set; }
        public int DestroyPiecePenalty { get; set; }
        public int InformationExchangePenalty {get; set;}
        public int CheckForShamPenalty { get; set; }
        public int NumberOfGoals { get; set; }
        public string CsIP { get; set; }
        public int CsPort { get; set; }
        public int GoalAreaHeight { get; set; }
        public int NumberOfPieces { get; set; }
        public double ShamPieceProbability { get; set; }
        public int TeamSize { get; set; }
        public string LoggingMode { get; set; }

        public static GMConfiguration ReadConfiguration(string[] programArguments)
        {
            if (programArguments.Length == 0)
                return useConfiguration(pathToDefaultConfiguration);
            else
                return useGivenConfiguration(programArguments);
        }

        private static GMConfiguration useGivenConfiguration(string[] programArguments)
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

        private static GMConfiguration useConfiguration(string pathToConfiguration)
        {
            var dir = Directory.GetCurrentDirectory();
            string fileContent = File.ReadAllText(pathToConfiguration);
            var tmp = JsonSerializer.Deserialize<GMConfiguration>(fileContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return tmp;
        }
    }
}
