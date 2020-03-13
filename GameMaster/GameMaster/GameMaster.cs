using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameMaster
{
    public class GameMaster
    {
        GUIMantainer _GUIMantainer;
        ManualGUIDataProvider _GUIDataProvider;
        public void Start()
        {
            InitGui();
            //TODO: rest of starting game master
        }
        public void GenerateGUI()
        {
            //TODO: use manual gui data provider to set apropriate fields
            //called every time game board is updated
        }
        private void InitGui()
        {
            //Manual GUI Data Provider can be replaced with another implementation of IGUIDataProvider
            //once code related to game board is complete
            _GUIDataProvider = new ManualGUIDataProvider(10, 10, 3);
            _GUIMantainer = new GUIMantainer(_GUIDataProvider);
            _GUIMantainer.StartGUI();
            Thread.Sleep(10000);
        }
    }
}
