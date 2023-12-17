using UnityEngine;
using Runtime.Signals;

namespace Runtime.Controllers.Obstacles
{
    public class GroundObstacleController : MonoBehaviour
    {



        internal void DroneArea()
        {
            StackSignals.Instance.onInteractionObstacleWithPlayer?.Invoke();
        }


        internal void TurretArea()
        {
            StackSignals.Instance.onInteractionObstacleWithPlayer?.Invoke();
        }
    }
}