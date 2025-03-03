using SinglePlay.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinglePlay.Apple
{
    public class AppleBoard : MonoBehaviour
    {
        private SpriteRenderer _boardSprite;

        private void Awake()
        {
            TryGetComponent(out _boardSprite);
        }

        /// <summary>
        /// Positioning the target apple on the board
        /// </summary>
        /// <param name="appleTransform">Target Apple Transform</param>
        /// <param name="col">Max col</param>
        /// <param name="row">Max row</param>
        /// <param name="targetX">Target x index</param>
        /// <param name="targetY">Target y index</param>
        public void AppleOnBoard(Transform appleTransform, int col, int row, int targetX, int targetY)
        {
            // board and grid size
            Vector2 boardSize = _boardSprite.bounds.size;
            Vector2 boardCenter = _boardSprite.bounds.center;
            float spaceX = boardSize.x / col;
            float spaceY = boardSize.y / row;

            // start pos
            float startPosX = (boardCenter.x - (boardSize.x / 2f)) + (spaceX / 2f);
            float startPosY = (boardCenter.y - (boardSize.y / 2f)) + (spaceY / 2f);
            Vector2 gridStartPosition = new Vector2(startPosX, startPosY);

            // target pos
            float posX = gridStartPosition.x + (spaceX * targetX);
            float posY = gridStartPosition.y + (spaceY * targetY);
            Vector2 pos = new Vector2(posX, posY);

            // target scale
            float scaleX = spaceX / boardSize.x;
            float scaleY = spaceY / boardSize.y;
            Vector3 scale = new Vector3(scaleX, scaleY, 1f);

            SetApplePosition(appleTransform, pos, scale);
        }

        private void SetApplePosition(Transform appleTransform, Vector2 pos, Vector3 scale)
        {
            appleTransform.SetParent(transform);
            appleTransform.position = pos;
            appleTransform.localScale = scale;
        }
    }
}