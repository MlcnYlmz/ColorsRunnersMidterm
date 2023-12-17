using Runtime.Signals;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Runtime.Controllers.Obstacles
{
    public class ObstacleDrone : MonoBehaviour
    {
        [Button("Drone Area")]
        public void DroneArea()
        {
            ObstacleSignals.Instance.onObstacleDroneArea?.Invoke();
        }
    }
}