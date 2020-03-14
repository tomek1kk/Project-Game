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
        bool guiStarted = false;
        public void StartGui(IGuiDataProvider guiDataProvider)
        {
            if(guiStarted)
            {
                throw new InvalidOperationException("Gui is already started");
            }

            _webHost = CreateWebHostBuilder(guiDataProvider).Build();
            _guiThread = new Thread(
                new ThreadStart(() => _webHost.Run())
                );
            _guiThread.Start();

            guiStarted = true;
        }
        public void StopGui()
        {
            if(!guiStarted)
            {
                throw new InvalidOperationException("Gui hasn't been started yet");
            }

            _webHost.StopAsync().Wait();

            guiStarted = false;
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
