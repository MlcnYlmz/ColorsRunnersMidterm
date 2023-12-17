using System.Collections;
using DG.Tweening;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums;
using Runtime.Keys;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerPhysicsController physicsController;
        [SerializeField] private PlayerMeshController meshController;

        #endregion

        #region Private Variables

        [ShowInInspector] private PlayerData _data;
        [ShowInInspector] private CollectableData _dataCollectable;
        private const string PlayerDataPath = "Data/CD_Player";


        public CollectableColorTypes playerColorType;

        private readonly string _collectableDataPath = "Data/CD_Collectable";

        #endregion

        #endregion



        private void Awake()
        {
            _data = GetPlayerData();
            _dataCollectable = GetCollectableData();
            SendPlayerDataToControllers();
            SendDataToController();
        }


        private void Start()
        {
            meshController.PlayerColorChanged((int)playerColorType);
            PlayerSignals.Instance.onColorType?.Invoke(playerColorType);
        }

        private CollectableData GetCollectableData() => Resources.Load<CD_Collectable>(_collectableDataPath).Data;

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(PlayerDataPath).Data;

        private void SendPlayerDataToControllers()
        {
            movementController.SetMovementData(_data.MovementData);
        }

        private void SendDataToController()
        {
            meshController.SetColorData(_dataCollectable.ColorData);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(true);
            InputSignals.Instance.onInputReleased += () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(false);
            InputSignals.Instance.onInputDragged += OnInputDragged;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful +=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            CoreGameSignals.Instance.onLevelFailed +=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            CoreGameSignals.Instance.onReset += OnReset;

            PlayerSignals.Instance.onSetTotalScore += OnSetTotalScore;
            PlayerSignals.Instance.onSetStackScore += OnSetStackScore;
            CoreGameSignals.Instance.onMiniGameEntered += OnMiniGameEntered;
        }


        private void OnPlay()
        {
            PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
        }

        private void OnInputDragged(HorizontalnputParams inputParams)
        {
            movementController.UpdateInputValue(inputParams);
        }

        private void OnMiniGameEntered()
        {
            DOVirtual.DelayedCall(0.25f, () =>
            {
                PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            });
            StartCoroutine(WaitForFinal());
        }

        private void OnSetTotalScore(int value)
        {
            
        }

        private void OnSetStackScore(int value)
        {
            meshController.UpdateStackScore(value);
        }

        private void OnReset()
        {
            movementController.OnReset();
            animationController.OnReset();
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(true);
            InputSignals.Instance.onInputReleased -= () => PlayerSignals.Instance.onMoveConditionChanged?.Invoke(false);
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onLevelSuccessful -=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(true);
            CoreGameSignals.Instance.onLevelFailed -=
                () => PlayerSignals.Instance.onPlayConditionChanged?.Invoke(false);
            CoreGameSignals.Instance.onReset -= OnReset;

            PlayerSignals.Instance.onSetTotalScore -= OnSetTotalScore;
            PlayerSignals.Instance.onSetStackScore -= OnSetStackScore;

            CoreGameSignals.Instance.onMiniGameEntered -= OnMiniGameEntered;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        internal void SetStackPosition()
        {
            var position = transform.position;
            Vector2 pos = new Vector2(position.x, position.z);
            StackSignals.Instance.onStackFollowPlayer?.Invoke(pos);
        }

        private IEnumerator WaitForFinal()
        {
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Idle);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            CoreGameSignals.Instance.onMiniGameStart?.Invoke();
        }

        internal void ChangeColor(CollectableColorTypes typeValue)
        {
            playerColorType = typeValue;

            PlayerSignals.Instance.onColorType?.Invoke(playerColorType);

            meshController.PlayerColorChanged((int)typeValue);
        }

        internal void SetSlowSpeed()
        {
            movementController.SlowState(true);
        }

        internal void SetNormalSpeed()
        {
            movementController.SlowState(false);
        }


        internal void DynamicGroundObstacleState()
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                movementController.OnReset();
                ObstacleSignals.Instance.onObstacleDroneArea?.Invoke();
            });
        }

        internal void GroundObstacleState()
        {
            movementController.SlowState(true);
            
            DOVirtual.DelayedCall(0.75f, () =>
            {
                ObstacleSignals.Instance.onObstacleTurretArea?.Invoke();
            });
        }
    }
}