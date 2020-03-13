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
    public class GuiMantainer
    {
        readonly Thread _guiThread;
        readonly IWebHost _webHost;
        public GuiMantainer(IGuiDataProvider guiDataProvider)
        {
            _webHost = CreateWebHostBuilder(guiDataProvider).Build();
            _guiThread = new Thread(
                new ThreadStart(()=> _webHost.Run())
                );
        }
        public void StartGui()
        {
            _guiThread.Start();
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
