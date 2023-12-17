using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        [SerializeField] private CollectableManager manager;

        private const string PlayerTag = "Player";
        private const string CollectableTag = "Collectable";
        private const string CollectedTag = "Collected";
        private const string GateTag = "Gate";
        private const string GateRedTag = "Gate Red";
        private const string GateBlueTag = "Gate Blue";
        private const string GateGreenTag = "Gate Green";
        private const string ObstacleTag = "Obstacle";
        private const string ConveyorTag = "Conveyor";

        private GateTypes _gateTypes;

        private void OnTriggerEnter(Collider other)
        {
            if (!CompareTag(CollectedTag))
                return;

            switch (other.tag)
            {
                case GateTag:
                    manager.CollectableUpgrade(manager.GetCurrentValue());
                    break;

                case ObstacleTag:
                    manager.InteractionWithObstacle(transform.parent.gameObject);
                    break;

                case ConveyorTag:
                    manager.InteractionWithConveyor();
                    break;

                case GateBlueTag:
                    CollectableUpgrade(GateTypes.GateBlue, "Blue");
                    break;

                case GateGreenTag:
                    CollectableUpgrade(GateTypes.GateGreen, "Green");
                    break;

                case GateRedTag:
                    CollectableUpgrade(GateTypes.GateRed, "Red");
                    break;
            }
        }

        private void CollectableUpgrade(GateTypes gateType, string debugMessage)
        {
            manager.CollectableUpgrade((int)gateType);
            Debug.LogWarning(debugMessage);
        }
    }
}
