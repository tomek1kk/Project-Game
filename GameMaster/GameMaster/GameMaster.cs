using GameMaster.Configuration;
using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameMaster
{
    public class GameMaster
    {
        readonly IGuiMantainer _guiMantainer;
        readonly GMConfiguration _gmConfiguration;
        ManualGuiDataProvider _guiDataProvider;

        public GameMaster(IGuiMantainer guiMantainer, GMConfiguration config)
        {
            _guiMantainer = guiMantainer;
            _gmConfiguration = config;
        }
        public void Start()
        {
            InitGui();
            //TODO: rest of starting game master

            Thread.Sleep(10000);
            _guiMantainer.StopGui();
        }
        public void GenerateGui()
        {
            //TODO: use manual gui data provider to set apropriate fields
            //called every time game board is updated
        }
        private void InitGui()
        {
            //Manual Gui Data Provider can be replaced with another implementation of IGuiDataProvider
            //once code related to game board is complete
            _guiDataProvider = new ManualGuiDataProvider(_gmConfiguration.BoardX, _gmConfiguration.BoardY, _gmConfiguration.GoalAreaHight);
            _guiMantainer.StartGui(_guiDataProvider);
        }
    }
}
