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
            ManualGUIDataProvider gUIDataProvider = new ManualGUIDataProvider(15, 10, 3);
            gUIDataProvider.SetField(3, 0, GUI.FieldType.BluePlayer);
            CreateWebHostBuilder(args, gUIDataProvider).Build().Run();
        }

        static IWebHostBuilder CreateWebHostBuilder(string[] args, IGUIDataProvider gUIDataProvider) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(gUIDataProvider);
                })
                .UseStartup<Startup>();
    }
}
