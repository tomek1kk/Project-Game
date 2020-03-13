using GameMaster.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace GameMaster.GUI
{
    public class GUIMantainer
    {
        readonly Thread _guiThread;
        readonly IWebHost _webHost;
        public GUIMantainer(IGUIDataProvider GUIDataProvider)
        {
            _webHost = CreateWebHostBuilder(GUIDataProvider).Build();
            _guiThread = new Thread(
                new ThreadStart(()=> _webHost.Run())
                );
        }
        public void StartGUI()
        {
            _guiThread.Start();
        }

        static IWebHostBuilder CreateWebHostBuilder(IGUIDataProvider GUIDataProvider) =>
            WebHost.CreateDefaultBuilder()
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(GUIDataProvider);
                    
                })
                .UseStartup<Startup>();
    }
}
