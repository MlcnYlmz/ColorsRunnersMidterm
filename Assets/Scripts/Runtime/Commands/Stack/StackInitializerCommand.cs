using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackInitializerCommand
    {
        private StackManager _stackManager;
        private GameObject _collectableStickMan;

        public StackInitializerCommand(StackManager stackManager,
            ref GameObject collectableStickMan)
        {
            _stackManager = stackManager;
            _collectableStickMan = collectableStickMan;
        }

        public void Execute()
        {
            var stackLevel = CoreGameSignals.Instance.onGetStackLevel();
            for (int i = 1; i < stackLevel; i++)
            {
                GameObject obj = Object.Instantiate(_collectableStickMan);
                _stackManager.AdderOnStackCommand.Execute(obj);
            }
            _stackManager.StackTypeUpdaterCommand.Execute();
        }
    }
}