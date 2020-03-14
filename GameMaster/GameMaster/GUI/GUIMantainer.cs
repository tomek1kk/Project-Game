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
    public class GuiMantainer : IGuiMantainer
    {
        Thread _guiThread;
        IWebHost _webHost;
        public void StartGui(IGuiDataProvider guiDataProvider)
        {
            _webHost = CreateWebHostBuilder(guiDataProvider).Build();
            _guiThread = new Thread(
                new ThreadStart(() => _webHost.Run())
                );
            _guiThread.Start();
        }
        public void StopGui()
        {
            _webHost.StopAsync().Wait();
        }

        static IWebHostBuilder CreateWebHostBuilder(IGuiDataProvider guiDataProvider) =>
            WebHost.CreateDefaultBuilder()
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(guiDataProvider);
                    
                })
                .UseStartup<Startup>();
    }
}
