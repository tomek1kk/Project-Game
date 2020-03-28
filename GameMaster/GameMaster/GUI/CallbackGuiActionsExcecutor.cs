using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{
    public class CallbackGuiActionsExcecutor : IGuiActionsExecutor
    {
        Action _startGameCallback;
        public CallbackGuiActionsExcecutor(Action startGameCallback)
        {
            _startGameCallback = startGameCallback;
        }
        public void StartGame()
        {
            _startGameCallback.Invoke();
        }
    }
}
