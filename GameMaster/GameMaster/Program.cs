using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.AspNet;
using GameMaster.GUI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GameMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ManualGUIDataProvider GUIDataProvider = new ManualGUIDataProvider(15, 10, 3);
            GUIDataProvider.SetField(3, 0, GUI.FieldType.BluePlayer);
            CreateWebHostBuilder(args, GUIDataProvider).Build().Run();
        }

        static IWebHostBuilder CreateWebHostBuilder(string[] args, IGUIDataProvider GUIDataProvider) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(GUIDataProvider);
                })
                .UseStartup<Startup>();
    }
}
