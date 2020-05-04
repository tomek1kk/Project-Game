using GameMaster.GUI;
using GameMaster.Configuration;
using Serilog;

namespace GameMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger.Information("GameMaster started");
            GMConfiguration config = GMConfiguration.ReadConfiguration(args);
            CreateLogger(config.LoggingMode);
            GameMaster gameMaster = new GameMaster(new GuiMantainer(),config, new ProxyMessageHandler());
            gameMaster.Start();
            gameMaster.WaitForEnd();
            Log.CloseAndFlush();
            gameMaster.Dispose();
        }
        private static void CreateLogger(string mode)
        {
            if(mode == "debug")
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs\\GameMasterLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("Logs\\GameMasterLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }
        }
    }
}
