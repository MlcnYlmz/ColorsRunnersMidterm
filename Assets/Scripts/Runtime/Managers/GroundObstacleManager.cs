using System;
using Runtime.Commands.Obstacle;
using Runtime.Controllers.Obstacles;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Runtime.Managers
{
    public class GroundObstacleManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private ObstacleDroneController obstacleDroneController;

        #endregion

        #region Private Variables

        private ObstacleAttackCommand _obstacleAttackCommand;

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
                    obstacleDroneController.DroneAttack();
                    break;
                case GroundObstacleTypes.Turret:
                    obstacleDroneController.TurretAttack();
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