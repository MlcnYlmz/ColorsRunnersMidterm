﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Controllers.MiniGame;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.Managers
{
    public class MiniGameManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Veriables

        [ShowInInspector] private GameObject buildingObject;

        [SerializeField] private GameObject wallObject;
        [SerializeField] private GameObject fakeMoneyObject;
        [SerializeField] private Transform fakePlayer;
        [SerializeField] private Material mat;

        [SerializeField] private short wallCount, fakeMoneyCount;

        [SerializeField] private WallCheckController wallChecker;

        #endregion

        #region Private Veriables

        private int _score;
        private float _multiplier;
        private Vector3 _initializePos;
        private int _scoreBuilding;

        [ShowInInspector] private List<BuildData> _data;

        private readonly string _buildDataPath = "Data/CD_Build";

        #endregion

        #endregion

        private void Awake()
        {
            _scoreBuilding = _data[0].buildRequirement;
        }
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onSendFinalScore += OnSendScore;
            ScoreSignals.Instance.onGetMultiplier += OnGetMultiplier;
            CoreGameSignals.Instance.onMiniGameStart += OnMiniGameStart;
            CoreGameSignals.Instance.onReset += OnReset;
        }


        private void OnMiniGameStart()
        {
            fakePlayer.gameObject.SetActive(true);
            StartCoroutine(GoUp());
        }

        private IEnumerator GoUp()
        {
            yield return new WaitForSeconds(1f);
            if (_score >= _scoreBuilding)
            {
                
            }
            else
            {
                fakePlayer.DOLocalMoveY(Mathf.Clamp(_score, 0, 900), 2.7f).SetEase(Ease.Flash).SetDelay(1f);
            }
            yield return new WaitForSeconds(4.5f);
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
        }

        internal void SetMultiplier(float multiplierValue)
        {
            _multiplier = multiplierValue;
        }

        private float OnGetMultiplier()
        {
            return _multiplier;
        }

        private void OnSendScore(int scoreValue)
        {
            _score = scoreValue;
        }

        private void UnSubscribeEvents()
        {
            ScoreSignals.Instance.onSendFinalScore -= OnSendScore;
            ScoreSignals.Instance.onGetMultiplier -= OnGetMultiplier;
            CoreGameSignals.Instance.onMiniGameStart -= OnMiniGameStart;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            buildingObject = _data[0].buildingPrefab;
            SpawnBuildObjects(buildingObject);
        }

        private void SpawnBuildObjects(GameObject buildingGameObject)
        {
            var ob = Instantiate(buildingGameObject, transform);
        }

        private void Init()
        {
            _initializePos = fakePlayer.localPosition;
        }

        private void SpawnWallObjects()
        {
            for (int i = 0; i <= wallCount; i++)
            {
                var ob = Instantiate(wallObject, transform);
                ob.transform.localPosition = new Vector3(0, i * 10, 0);
                ob.transform.GetChild(0).GetComponent<TextMeshPro>().text = "x" + ((i / 10f) + 1f);
            }
        }

        private void SpawnFakeMoneyObjects()
        {
            for (int i = 0; i < fakeMoneyCount; i++)
            {
                var ob = Instantiate(fakeMoneyObject, fakePlayer);
                ob.transform.localPosition = new Vector3(0, -i * 1.58f, -7);
            }
        }

        private void ResetWalls()
        {
            for (int i = 1; i < wallCount; i++)
            {
                transform.GetChild(i).GetComponent<Renderer>().material = mat;
                transform.GetChild(i).transform.position = Vector3.zero;
            }
        }

        private void OnReset()
        {
            StopAllCoroutines();
            DOTween.KillAll();
            ResetWalls();
            ResetFakePlayer();
        }

        private void ResetFakePlayer()
        {
            fakePlayer.gameObject.SetActive(false);
            fakePlayer.localPosition = _initializePos;
            wallChecker.OnReset();
        }
    }
}