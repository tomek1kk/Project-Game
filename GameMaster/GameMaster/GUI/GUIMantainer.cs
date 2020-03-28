using GameMaster.AspNet;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace GameMaster.GUI
{
    public class GuiMantainer : IGuiMantainer
    {
        Thread _guiThread;
        IWebHost _webHost;
        bool guiStarted = false;
        public void StartGui(IGuiDataProvider guiDataProvider, IGuiActionsExecutor guiActionsExecutor)
        {
            if(guiStarted)
            {
                throw new InvalidOperationException("Gui is already started");
            }

            _webHost = CreateWebHostBuilder(guiDataProvider, guiActionsExecutor).Build();
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

        static IWebHostBuilder CreateWebHostBuilder(IGuiDataProvider guiDataProvider,
            IGuiActionsExecutor guiActionsExecutor) =>
            WebHost.CreateDefaultBuilder()
                .ConfigureLogging(logging => {
                    logging.ClearProviders();
                })
                .ConfigureServices(servicesCollection =>
                {
                    servicesCollection.AddSingleton(guiDataProvider);
                    servicesCollection.AddSingleton(guiActionsExecutor);

                })
                .UseStartup<Startup>();
    }
}
