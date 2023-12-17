using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Collectables
{
    public class CollectablePhysicsController : MonoBehaviour
    {
        

        [SerializeField] private CollectableManager manager;

        private const string CollectableTag = "Collectable";
        private const string CollectedTag = "Collected";
        private const string GateTag = "Gate";
        private const string AtmTag = "ATM";
        private const string ObstacleTag = "Obstacle";
        private const string ConveyorTag = "Conveyor";

        private void OnTriggerEnter(Collider other)
        {
            string otherTag = other.tag;
            
            switch (otherTag)
            {
                case CollectableTag when CompareTag(CollectedTag):
                    HandleCollectableInteraction(other);
                    break;

                case GateTag when CompareTag(CollectedTag):
                    manager.CollectableUpgrade(manager.GetCurrentValue());
                    break;

                case AtmTag when CompareTag(CollectedTag):
                    manager.InteractionWithAtm(transform.parent.gameObject);
                    break;

                case ObstacleTag when CompareTag(CollectedTag):
                    manager.InteractionWithObstacle(transform.parent.gameObject);
                    break;

                case ConveyorTag when CompareTag(CollectedTag):
                    manager.InteractionWithConveyor();
                    break;
            }
        }

        private void HandleCollectableInteraction(Collider other)
        {
            other.tag = CollectedTag;
            manager.InteractionWithCollectable(other.transform.parent.gameObject);
        }
    }
}