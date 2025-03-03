using SinglePlay.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinglePlay.Apple
{
    public class AppleController : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private int row = 10;
        [SerializeField] private int col = 17;

        [Header("Apple")]
        [SerializeField] private float appleDropTime = 3f;

        [Header("Component")]
        [SerializeField] private AppleBoard appleBoard;
        [SerializeField] private ObjectPool objectPool;

        // apple grid
        public Apple[,] Apples;

        // properties
        public int Row => row;
        public int Col => col;

        private void Awake()
        {
            if(Apples == null)
            {
                Apples = new Apple[row, col];
            }
        }

        private void Start()
        {
            GameManager.Instance.OnRestartGame += InitGrid;

            objectPool.InitPool(row * col);
            InitGrid();
        }

        private void InitGrid()
        {
            for(int i = 0; i < row; i++)
            {             
                for(int j = 0; j < col; j++)
                {
                    if (Apples[i, j] == null)
                    {
                        GameObject appleObject = GetApple(out Apples[i, j]);
                        PositionApple(appleObject.transform, j, i);
                    }

                    Apples[i, j].AppleEdge.SetActive(false);
                    Apples[i, j].SetNumber();
                }
            }
        }

        /// <summary>
        /// Positioning the apple on the board
        /// </summary>
        /// <param name="appleTransform">Apple`s Transform Component</param>
        /// <param name="x">X index</param>
        /// <param name="y">Y index</param>
        private void PositionApple(Transform appleTransform, int x, int y)
        {
            appleBoard.AppleOnBoard(appleTransform, col, row, x, y);
        }

        private GameObject GetApple(out Apple apple)
        {
            GameObject appleObject = objectPool.GetObject();
            appleObject.TryGetComponent(out apple);
            apple.SetNumber();
            return appleObject;
        }

        private void ReturnApple(Apple apple)
        {
            StartCoroutine(ReturnAppleToPool(apple, appleDropTime));
        }

        private IEnumerator ReturnAppleToPool(Apple apple, float dropTime)
        {
            apple.DropApple(dropTime);
            yield return new WaitForSeconds(dropTime);
            apple.AppleRigidbody.velocity = Vector2.zero;
            apple.AppleRigidbody.gravityScale = 0f;
            objectPool.ReturnObject(apple.gameObject);
        }

        /// <summary>
        /// Drop apples with a sum of 10 and reflect them in the score
        /// </summary>
        /// <param name="appleIndex">a list of apple`s indexes with a sum of ten</param>
        public void CorrectNumberApples(List<(int, int)> appleIndex)
        {
            GameManager.Instance.Score += appleIndex.Count;
            foreach(var index in appleIndex)
            {
                Apple apple = Apples[index.Item1, index.Item2];
                Apples[index.Item1, index.Item2] = null;
                ReturnApple(apple);
            }
        }
    }
}