using Runtime.Extentions;
using UnityEngine.Events;
using Runtime.Enums;

namespace Runtime.Signals
{
    public class ObstacleSignals : MonoSingleton<ObstacleSignals>
    {
        public UnityAction onObstacleDroneArea = delegate { };
        public UnityAction onObstacleTurretArea = delegate { };
        public UnityAction<bool> onObstacleColorMatch = delegate { };
        public UnityAction onObstacleAttack = delegate { };
        public UnityAction<GroundObstacleTypes> onSendObstacleGroundType = delegate { };
        
    }
}