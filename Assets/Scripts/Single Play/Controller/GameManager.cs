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
        public int Score => _score;

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

        }

        void Update()
        {

        }

        #region Init Methods

        private void InitValue()
        {
            _score = 0;
        }

        #endregion
    }
}