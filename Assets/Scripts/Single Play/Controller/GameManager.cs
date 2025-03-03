using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SinglePlay.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        
        [SerializeField] private Timer timer;

        private int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                UIManager.Instance.Score = _score;
            }
        }

        private bool _isGameEnd;
        public bool IsGameEnd => _isGameEnd;

        public event Action OnRestartGame;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            InitValue();
        }

        void Start()
        {
            OnRestartGame += InitValue;
        }

        void Update()
        {
            if(timer.CurrentTime <= 0 && !_isGameEnd)
            {
                GameEnd();
            }
        }

        #region Init Methods

        private void InitValue()
        {
            _isGameEnd = false;
            _score = 0;
        }

        #endregion

        public void GameEnd()
        {
            _isGameEnd = true;
            UIManager.Instance.TimeEnd();
        }

        public void RestartGame()
        {
            OnRestartGame?.Invoke();
        }
    }
}