using System;
using Runtime.Controllers.Obstacles;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class GroundObstacleManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private GroundObstacleController obstacleDroneController;

        #endregion

        #region Private Variables
        
        [ShowInInspector] GroundObstacleTypes _obstacleType;

        #endregion

        #endregion


        private void Awake()
        {
        }


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ObstacleSignals.Instance.onObstacleAttack += OnObstacleAttack;
            ObstacleSignals.Instance.onSendObstacleGroundType += OnGetObstacleGroundType;
        }

        private void OnGetObstacleGroundType(GroundObstacleTypes groundType)
        {
            _obstacleType = groundType;
        }

        private void OnObstacleAttack()
        {
            switch (_obstacleType)
            {
                case GroundObstacleTypes.Drone:
                    obstacleDroneController.DroneArea();
                    break;
                case GroundObstacleTypes.Turret:
                    obstacleDroneController.TurretArea();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            ObstacleSignals.Instance.onObstacleAttack -= OnObstacleAttack;
            ObstacleSignals.Instance.onSendObstacleGroundType -= OnGetObstacleGroundType;
        }
    }
}