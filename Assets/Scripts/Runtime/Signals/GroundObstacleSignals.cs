using Runtime.Enums;
using Runtime.Extentions;
using Runtime.Keys;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class GroundObstacleSignals : MonoSingleton<GroundObstacleSignals>
    {
        public UnityAction onObstacleAttack = delegate { };
    }
}