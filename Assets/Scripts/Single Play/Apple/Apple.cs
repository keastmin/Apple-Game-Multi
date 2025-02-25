using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [SerializeField] private int minNum = 1;
    [SerializeField] private int maxNum = 9;

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

    private void Start()
    {
        SetNumber();
    }

    /// <summary>
    /// 사과 생성시 랜덤 숫자 지정
    /// </summary>
    private void SetNumber()
    {
        Number = Random.Range(minNum, maxNum + 1);
    }
}
