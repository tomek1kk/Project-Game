using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMasterIntegrationTests
{
    public class StubGuiMaintainer : IGuiMantainer
    {
        IGuiActionsExecutor guiActionsExecutor;
        public void StartGame()
        {
            guiActionsExecutor.StartGame();
        }
        public void StartGui(IGuiDataProvider guiDataProvider, IGuiActionsExecutor guiActionsExecutor)
        {
            this.guiActionsExecutor = guiActionsExecutor;
        }

        public void StopGui()
        {

        }
    }
}
