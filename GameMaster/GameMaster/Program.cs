using GameMaster.GUI;
using GameMaster.Configuration;
using Serilog;

namespace GameMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("Logs\\GameMasterLog-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Logger.Information("GameMaster started");
            GMConfiguration config = GMConfiguration.ReadConfiguration(args);
            GameMaster gameMaster = new GameMaster(new GuiMantainer(),config, new ProxyMessageHandler());
            gameMaster.Start();
            Log.CloseAndFlush();
        }

    }
}
