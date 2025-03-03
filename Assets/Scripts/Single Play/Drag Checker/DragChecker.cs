using SinglePlay.Apple;
using SinglePlay.Manager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DragChecker : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private AppleController appleController;

    [SerializeField] private GameObject checkerObject;

    private Transform _checkerTransform;
    private SpriteRenderer _checkSpriteRenderer;

    private Vector2 _startPos;
    private Vector2 _endPos;

    public float TopY => _checkerTransform.position.y + Mathf.Abs(_checkSpriteRenderer.size.y) / 2f;
    public float BottomY => _checkerTransform.position.y - Mathf.Abs(_checkSpriteRenderer.size.y) / 2f;
    public float LeftX => _checkerTransform.position.x - Mathf.Abs(_checkSpriteRenderer.size.x) / 2f;
    public float RightX => _checkerTransform.position.x + Mathf.Abs(_checkSpriteRenderer.size.x) / 2f;

    private int _appleSum = 0;
    private List<(int, int)> _appleIndex; // (row, col)

    private void Start()
    {
        checkerObject.TryGetComponent(out _checkerTransform);
        checkerObject.TryGetComponent(out _checkSpriteRenderer);

        InitChecker();
        GameManager.Instance.OnRestartGame += InitChecker;
    }

    private void Update()
    {
        CheckMouseEvent();
    }

    private void InitChecker()
    {
        _appleIndex = new List<(int, int)>();
        checkerObject.gameObject.SetActive(false);
        _checkSpriteRenderer.size = Vector2.zero;
        _checkerTransform.position = Vector2.zero;
    }

    private void CheckMouseEvent()
    {
        if (!UIManager.Instance.MenuPanelActive && !GameManager.Instance.IsGameEnd)
        {
            CheckerMouseDown();
            CheckerMouseDrag();
            CheckerMouseUp();
        }
    }

    private void CheckerMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _appleSum = 0;
            InitCheckInfo();
            checkerObject.SetActive(true);
        }
    }

    private void InitCheckInfo()
    {
        _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _endPos = _startPos;
        SetCheckerTransform();
    }

    private void CheckerMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_endPos != currentMousePos)
            {
                _endPos = currentMousePos;
                SetCheckerTransform();
                _appleSum = CheckAppleNumbers(out _appleIndex);

                if(_appleSum == 10)
                {
                    _checkSpriteRenderer.color = new Vector4(1, 0, 0, 0.4f);
                    _appleIndex.ForEach(index => appleController.Apples[index.Item1, index.Item2].AppleEdge.SetActive(true));
                }
                else
                {
                    _checkSpriteRenderer.color = new Vector4(0, 1, 0, 0.4f);
                    _appleIndex.ForEach(index => appleController.Apples[index.Item1, index.Item2].AppleEdge.SetActive(false));
                }
            }
        }
    }

    private void SetCheckerTransform()
    {
        float distanceX = (_startPos.x - _endPos.x);
        float distanceY = (_startPos.y - _endPos.y);

        float centerX = (_startPos.x + _endPos.x) / 2f;
        float centerY = (_startPos.y + _endPos.y) / 2f;

        _checkSpriteRenderer.size = new Vector2(distanceX, distanceY);
        _checkerTransform.position = new Vector2(centerX, centerY);
    }

    private int CheckAppleNumbers(out List<(int, int)> appleIndexList)
    {
        int sum = 0;
        appleIndexList = new List<(int, int)>();
        for(int i = 0; i < appleController.Row; i++)
        {
            for (int j = 0; j < appleController.Col; j++)
            {
                if (appleController.Apples[i, j] != null)
                {
                    Apple apple = appleController.Apples[i, j];
                    Vector2 applePos = apple.transform.position;

                    if (applePos.x > LeftX && applePos.x < RightX && applePos.y > BottomY && applePos.y < TopY)
                    {
                        sum += apple.Number;
                        appleIndexList.Add((i, j));
                    }
                }
            }
        }

        return sum;
    }

    private void CheckerMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _appleIndex.ForEach(index => appleController.Apples[index.Item1, index.Item2].AppleEdge.SetActive(false));
            if (_appleSum == 10)
            {
                appleController.CorrectNumberApples(_appleIndex);
            }

            _checkSpriteRenderer.color = new Vector4(1, 0, 0, 0.4f);
            checkerObject.SetActive(false);
        }
    }
}
