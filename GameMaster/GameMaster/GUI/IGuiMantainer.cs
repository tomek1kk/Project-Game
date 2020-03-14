using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{
    public interface IGuiMantainer
    {
        void StartGui(IGuiDataProvider guiDataProvider);
        void StopGui();
    }
}
