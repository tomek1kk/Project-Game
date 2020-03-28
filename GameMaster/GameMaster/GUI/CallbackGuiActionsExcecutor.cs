using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{
    public class CallbackGuiActionsExcecutor : IGuiActionsExecutor
    {
        Action _startGameCallback;
        bool _gameStarted = false;
        public CallbackGuiActionsExcecutor(Action startGameCallback)
        {
            _startGameCallback = startGameCallback;
        }
        public void StartGame()
        {
            lock (this)
            {
                if(!_gameStarted)
                {
                    _startGameCallback.Invoke();
                    _gameStarted = true;
                }
            }
        }
    }
}
