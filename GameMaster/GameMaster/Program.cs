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
            ManualGUIDataProvider GUIDataProvider = new ManualGUIDataProvider(15, 10, 3);
            GUIDataProvider.SetField(3, 0, GUI.FieldType.BluePlayer);
            GUIMantainer gUIInitializer = new GUIMantainer(GUIDataProvider);
            gUIInitializer.StartGUI();
            Random r = new Random();
            for(int k = 0; k < 10; k++)
            {
                Thread.Sleep(1000);
                var baseModel = GUIDataProvider.GetCurrentBoardModel();
                for (int i = 0; i < baseModel.Fields.GetLength(0); i++)
                {
                    for (int j = 0; j < baseModel.Fields.GetLength(1); j++)
                    {
                        baseModel.Fields[i, j] = (FieldType)(r.Next() % Enum.GetValues(typeof(FieldType)).Length);
                    }
                }
            }
        }

    }
}
