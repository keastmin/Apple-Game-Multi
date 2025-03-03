using System.Collections;
using TMPro;
using UnityEngine;

namespace SinglePlay.Apple
{
    public class Apple : MonoBehaviour
    {
        [SerializeField] private int minNum = 1;
        [SerializeField] private int maxNum = 9;

        [SerializeField] private float gravityForce = 4f;
        [SerializeField] private float minDropForce = 10f;
        [SerializeField] private float maxDropForce = 15f;

        public TextMeshPro NumberText;
        public GameObject AppleEdge;
        public SpriteRenderer AppleSprite;
        public Rigidbody2D AppleRigidbody;

        private int _number;
        public int Number // 숫자가 정해지면 사과의 텍스트도 변경
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
                NumberText.text = _number.ToString();
            }
        }

        /// <summary>
        /// 사과 생성시 랜덤 숫자 지정
        /// </summary>
        public void SetNumber()
        {
            Number = Random.Range(minNum, maxNum + 1);
        }

        public void DropApple(float dropTime)
        {
            AppleRigidbody.gravityScale = gravityForce;
            AppleRigidbody.AddForce(GetRandomDirection() * GetRandomForce(), ForceMode2D.Impulse);
        }

        private float GetRandomForce()
        {
            return UnityEngine.Random.Range(minDropForce, maxDropForce);
        }

        private Vector2 GetRandomDirection()
        {
            float x = UnityEngine.Random.Range(-1f, 1f);
            float y = UnityEngine.Random.Range(0f, 1f);
            Vector2 direction = new Vector2(x, y);
            return direction.normalized;
        }
    }
}