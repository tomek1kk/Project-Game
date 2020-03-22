using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameMaster.AspNet;
using GameMaster.GUI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GameMaster.Configuration;
using GameMaster.Game;

namespace GameMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {

            GMConfiguration config = GMConfiguration.ReadConfiguration(args);
            GameMaster gameMaster = new GameMaster(new GuiMantainer(),config, new ProxyMessageHandler());
            gameMaster.Start();
        }

    }
}
