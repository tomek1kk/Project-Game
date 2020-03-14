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

namespace GameMaster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameMaster gameMaster = new GameMaster(new GuiMantainer());
            gameMaster.Start();
        }

    }
}
