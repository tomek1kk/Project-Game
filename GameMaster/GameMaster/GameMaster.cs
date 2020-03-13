using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameMaster
{
    public class GameMaster
    {
        GuiMantainer _guiMantainer;
        ManualGuiDataProvider _guiDataProvider;
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
            _guiDataProvider = new ManualGuiDataProvider(10, 10, 3);
            _guiMantainer = new GuiMantainer(_guiDataProvider);
            _guiMantainer.StartGui();
        }
    }
}
