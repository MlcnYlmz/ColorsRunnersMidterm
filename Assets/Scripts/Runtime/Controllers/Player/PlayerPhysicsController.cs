using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] private Rigidbody managerRigidbody;
        [SerializeField] private PlayerManager playerManager;

        private bool _isColorMatchFailed;

        // Tags
        private const string ObstacleTag = "Obstacle";
        private const string AtmTag = "ATM";
        private const string CollectableTag = "Collectable";
        private const string ConveyorTag = "Conveyor";
        private const string GateRedTag = "Gate Red";
        private const string GateBlueTag = "Gate Blue";
        private const string GateGreenTag = "Gate Green";
        private const string ColorfulObstacleTag = "Colorful Obstacle";
        private const string ColorfulDynamicObstacleTag = "Colorful Dynamic";
        private const string GroundObstacleTag = "Colorful Ground";

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case ObstacleTag:
                    HandleObstacleCollision(other);
                    break;

                case AtmTag:
                    CoreGameSignals.Instance.onAtmTouched?.Invoke(other.gameObject);
                    break;

                case CollectableTag:
                    HandleCollectableCollision(other);
                    break;

                case ConveyorTag:
                    CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
                    break;

                case GateRedTag:
                case GateBlueTag:
                case GateGreenTag:
                    HandleGateCollision(other);
                    break;

                case ColorfulObstacleTag:
                    playerManager.GroundObstacleState();
                    Debug.LogWarning("Slow Speed");
                    ObstacleSignals.Instance.onSendObstacleGroundType.Invoke(GroundObstacleTypes.Turret);
                    GroundObstacleSignals.Instance.onObstacleAttack.Invoke();
                    break;

                case ColorfulDynamicObstacleTag:
                    
                    Debug.LogWarning("Drone Area");
                    ObstacleSignals.Instance.onSendObstacleGroundType.Invoke(GroundObstacleTypes.Turret);
                    GroundObstacleSignals.Instance.onObstacleAttack.Invoke();
                    break;

                case GroundObstacleTag:
                    HandleGroundObstacleCollision(other);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(ColorfulObstacleTag))
            {
                playerManager.SetNormalSpeed();
                Debug.LogWarning("Normal Speed");
                ResetColorMatchState();
            }
        }

        private void HandleObstacleCollision(Collider other)
        {
            managerRigidbody.transform.DOMoveZ(managerRigidbody.transform.position.z - 10f, 1f).SetEase(Ease.OutBack);
            StackSignals.Instance.onInteractionObstacleWithPlayer?.Invoke();
            other.gameObject.SetActive(false);
        }

        private void HandleCollectableCollision(Collider other)
        {
            CollectableManager collectableManager = other.transform.parent.GetComponent<CollectableManager>();

            if (collectableManager != null)
            {
                CollectableColorTypes collectableColorType = collectableManager.collectableColorType;
                CollectableColorTypes playerColorType = playerManager.playerColorType;

                if (collectableColorType == playerColorType)
                {
                    Debug.Log("Same Color " + collectableColorType);
                    other.tag = "Collected";
                    StackSignals.Instance.onInteractionCollectable?.Invoke(other.transform.parent.gameObject);
                    StackSignals.Instance.onUpdateAnimation?.Invoke();
                }
                else
                {
                    Debug.Log("Another Color: " + collectableColorType);
                    StackSignals.Instance.onInteractionObstacleWithPlayer?.Invoke();
                    other.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("CollectableManager component not found on the collectable GameObject.");
            }
        }

        private void HandleGateCollision(Collider other)
        {
            if (other.CompareTag(GateBlueTag))
            {
                playerManager.ChangeColor(CollectableColorTypes.Blue);
                Debug.LogWarning("Blue");
            }
            else if (other.CompareTag(GateGreenTag))
            {
                playerManager.ChangeColor(CollectableColorTypes.Green);
                Debug.LogWarning("Green");
            }
            else if (other.CompareTag(GateRedTag))
            {
                playerManager.ChangeColor(CollectableColorTypes.Red);
                Debug.LogWarning("Red");
            }
        }

        private void HandleGroundObstacleCollision(Collider other)
        {
            Debug.Log("Turret Area");
            var otherMaterial = other.gameObject.GetComponent<MeshRenderer>().materials[0];
            var otherColor = CleanUpMaterialName(otherMaterial.name);
            var playerColor = playerManager.playerColorType.ToString();
            Debug.LogWarning("Other Color :" + otherColor);

            if (otherColor == playerManager.playerColorType.ToString())
            {
                _isColorMatchFailed = false;
                Debug.LogWarning("Same Turret Area Color");
                ObstacleSignals.Instance.onObstacleColorMatch?.Invoke(!_isColorMatchFailed);
            }
            else if (otherColor != playerManager.playerColorType.ToString())
            {
                _isColorMatchFailed = true;
                Debug.LogWarning("Player Color " + playerColor);
                Debug.LogWarning("Another Turret Area Color");
                ObstacleSignals.Instance.onObstacleColorMatch?.Invoke(!_isColorMatchFailed);
            }
        }

        private void ResetColorMatchState()
        {
            _isColorMatchFailed = false;
            ObstacleSignals.Instance.onObstacleColorMatch?.Invoke(!_isColorMatchFailed);
        }

        private string CleanUpMaterialName(string fullName)
        {
            int indexOfParenthesis = fullName.IndexOf(" (");
            return indexOfParenthesis >= 0 ? fullName.Substring(0, indexOfParenthesis) : fullName;
        }
    }
}
