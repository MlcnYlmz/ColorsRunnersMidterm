using System.Collections.Generic;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Commands.Stack
{
    public class StackLastItemRemoverCommand
    {
        private StackManager _stackManager;
        private List<GameObject> _collectableStack;
        private Transform _levelHolder;

        public StackLastItemRemoverCommand(StackManager stackManager, ref List<GameObject> collectableStack)
        {
            _stackManager = stackManager;
            _collectableStack = collectableStack;
            _levelHolder = GameObject.Find("LevelHolder")?.transform;
        }

        public void Execute()
        {
            if (_collectableStack.Count > 0)
            {
                int last = _collectableStack.Count - 1;

                if (last >= 0)
                {
                    GameObject lastItem = _collectableStack[last];
                    _collectableStack.RemoveAt(last);
                    _collectableStack.TrimExcess();

                    if (_levelHolder != null && _levelHolder.childCount > 0)
                    {
                        lastItem.transform.SetParent(_levelHolder.GetChild(0));
                        lastItem.SetActive(false);

                        if (_stackManager != null && _stackManager.StackJumperCommand != null)
                        {
                            _stackManager.StackJumperCommand.Execute(last - 1, last);
                        }

                        if (_stackManager != null && _stackManager.StackTypeUpdaterCommand != null)
                        {
                            _stackManager.StackTypeUpdaterCommand.Execute();
                        }

                        _stackManager?.OnSetStackAmount();
                    }
                }
            }
        }
    }
}