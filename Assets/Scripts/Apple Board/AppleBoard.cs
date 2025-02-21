using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Cell
{
    public bool C_IsEmpty;
    public int C_IndexX;
    public int C_IndexY;
    public Apple C_Apple;

    public Cell(bool isEmpty = true, int indexX = 0, int indexY = 0, Apple apple = null)
    {
        C_IsEmpty = isEmpty;
        C_IndexX = indexX;
        C_IndexY = indexY;
        C_Apple = apple;
    }
}

public class AppleBoard : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int _row = 10;
    [SerializeField] private int _col = 17;

    [Header("Component")]
    [SerializeField] private GameObject _apple;
    [SerializeField] private GameObject _board;
    [SerializeField] private SpriteRenderer _boardSprite;

    [Header("Object Pool")]
    [SerializeField] private ObjectPool _objectPool;

    [Header("Drag Checker")]
    [SerializeField] private DragChecker _dragChecker;

    // apple grid
    private Cell[,] _cells;

    // cache
    private Vector2 _gridAnchor;
    private float _boardWidth;
    private float _boardHeight;
    private float _spaceX;
    private float _spaceY;

    private void OnValidate()
    {
        BoardInfoInit(_col, _row);
    }

    private void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        _objectPool.InitPool();
        FillGridCell();
    }

    private void Update()
    {
        // 드래그 체커 상태에 따라 사과 변경
        if(_dragChecker.State == DragChecker.CheckerState.Dragging)
        {
            CheckAppleInRange();
        }
        else if(_dragChecker.State == DragChecker.CheckerState.End)
        {
            EndCheckApple();
        }
    }

    /// <summary>
    /// 보드의 정보 초기화 메소드
    /// </summary>
    /// <param name="col">그리드의 열</param>
    /// <param name="row">그리드의 행</param>
    private void BoardInfoInit(int col, int row)
    {
        Vector3 boardSize = _boardSprite.bounds.size;
        Vector2 boardCenter = _boardSprite.bounds.center;
        _boardWidth = boardSize.x;
        _boardHeight = boardSize.y;
        _gridAnchor = new Vector2(boardCenter.x - (_boardWidth / 2f), boardCenter.y - (_boardHeight / 2f));
        _spaceX = _boardWidth / col;
        _spaceY = _boardHeight / row;
    }

    private void FillGridCell()
    {
        // 그리드 초기화
        _cells = new Cell[_col, _row];

        for(int i = 0; i < _col; i++)
        {
            float posX = _gridAnchor.x + (_spaceX / 2f) + (_spaceX * i);

            for(int j = 0; j < _row; j++)
            {
                float posY = _gridAnchor.y + (_spaceY / 2f) + (_spaceY * j);
                Vector3 pos = new Vector3(posX, posY, 0f);
                Vector3 scale = new Vector3(_spaceX / _boardWidth, _spaceY / _boardHeight, 1f);

                _cells[i, j] = new Cell();
                _cells[i, j].C_IndexX = i;
                _cells[i, j].C_IndexY = j;

                // 사과 생성
                AppleGenerate(scale, pos, _cells[i, j]);
            }
        }
    }
    
    /// <summary>
    /// 사과 생성 메소드
    /// </summary>
    private void AppleGenerate(Vector3 scale, Vector3 pos, Cell cell)
    {
        GameObject obj = _objectPool.GetObject();

        if (obj != null)
        {
            obj.transform.SetParent(_board.transform);
            obj.transform.localScale = scale;
            obj.transform.position = pos;
            obj.TryGetComponent(out cell.C_Apple);
            cell.C_IsEmpty = false;
        }
    }



    private void GetApple(Cell cell)
    {
        Vector3 pos = GetApplePos(cell);
        Vector3 scale = new Vector3(_spaceX / _boardWidth, _spaceY / _boardHeight, 1f);
        AppleGenerate(scale, pos, cell);
    }

    private void CheckAppleInRange()
    {
        float[] checkerSize = _dragChecker.GetCheckerSize();
        int sum = 0;
        foreach (var cell in _cells)
        {
            if (!cell.C_IsEmpty)
            {
                Vector3 pos = GetApplePos(cell);
                Apple apple = cell.C_Apple;
                
                if(pos.x > _dragChecker.LeftX && pos.x < _dragChecker.RightX && pos.y > _dragChecker.BottomY && pos.y < _dragChecker.TopY)
                {
                    apple.AppleEdge.SetActive(true);
                    sum += apple.Number;
                }
                else
                {
                    apple.AppleEdge.SetActive(false);
                }
            }
        }

        if(sum == 10)
        {
            _dragChecker.CheckerSpriteRenderer.color = new Vector4(1, 0, 0, 0.4f);
        }
        else
        {
            _dragChecker.CheckerSpriteRenderer.color = new Vector4(0, 1, 0, 0.4f);
        }
    }

    private void EndCheckApple()
    {
        float[] checkerSize = _dragChecker.GetCheckerSize();
        List<Cell> selectedCells = new List<Cell>();
        int sum = 0;
        foreach (var cell in _cells)
        {
            if (!cell.C_IsEmpty)
            {
                Vector3 pos = GetApplePos(cell);
                Apple apple = cell.C_Apple;

                if (pos.x > _dragChecker.LeftX && pos.x < _dragChecker.RightX && pos.y > _dragChecker.BottomY && pos.y < _dragChecker.TopY)
                {
                    selectedCells.Add(cell);
                }
            }
        }

        foreach(var cell in selectedCells)
        {
            Apple apple = cell.C_Apple;
            apple.AppleEdge.SetActive(false);
            sum += apple.Number;
        }

        if(sum == 10)
        {
            FindCorrectNumberApples(selectedCells);
        }
        else
        {
            foreach(var cell in selectedCells)
            {
                cell.C_Apple.AppleEdge.SetActive(false);
            }
        }
    }

    private void FindCorrectNumberApples(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            StartCoroutine(ReturnApple(cell));
        }
    }

    private IEnumerator ReturnApple(Cell cell)
    {
        Apple apple = cell.C_Apple;
        apple.AppleRigidbody.gravityScale = 4f;
        apple.AppleRigidbody.AddForce(GetRandomDirection() * GetRandomForce(), ForceMode2D.Impulse);
        cell.C_IsEmpty = true;
        cell.C_Apple = null;
        yield return new WaitForSeconds(3f);
        apple.AppleRigidbody.velocity = Vector2.zero;
        apple.AppleRigidbody.gravityScale = 0f;
        _objectPool.ReturnObject(apple.gameObject);
    }

    #region Helper

    private Vector3 GetApplePos(Cell cell)
    {
        float posX = _gridAnchor.x + (_spaceX / 2f) + (_spaceX * cell.C_IndexX);
        float posY = _gridAnchor.y + (_spaceY / 2f) + (_spaceY * cell.C_IndexY);
        Vector3 pos = new Vector3(posX, posY, 0f);
        return pos;
    }

    private float GetRandomForce()
    {
        return UnityEngine.Random.Range(10f, 15f);
    }

    private Vector2 GetRandomDirection()
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(0f, 1f);
        Vector2 direction = new Vector2(x, y);
        return direction.normalized;
    }

    #endregion
}
