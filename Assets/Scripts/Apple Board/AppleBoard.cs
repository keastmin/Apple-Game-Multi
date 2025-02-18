using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class AppleBoard : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private int _row = 10;
    [SerializeField] private int _col = 17;

    [Header("Component")]
    [SerializeField] private GameObject _apple;
    [SerializeField] private GameObject _board;
    [SerializeField] private SpriteRenderer _boardSprite;

    // apple grid
    private Apple[,] _apples;

    // cache
    private Vector2 _gridAnchor;
    private float _boardWidth;
    private float _boardHeight;
    private float _spaceX;
    private float _spaceY;

    private void OnValidate()
    {
        GridInit(_col, _row);
    }

    private void Awake()
    {
        OnValidate();
    }

    void Start()
    {
        AppleGenerate();
    }

    /// <summary>
    /// 그리드 초기화 메소드
    /// </summary>
    /// <param name="col">그리드의 열</param>
    /// <param name="row">그리드의 행</param>
    private void GridInit(int col, int row)
    {
        // 보드 정보 초기화
        BoardInfoInit(col, row);
        
        // 그리드 초기화
        _apples = new Apple[col, row];
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

    /// <summary>
    /// 사과 생성 메소드
    /// </summary>
    private void AppleGenerate()
    {
        if (_apple != null)
        {
            for (int i = 0; i < _col; i++)
            {
                float posX = _gridAnchor.x + (_spaceX / 2f) + (_spaceX * i);

                for (int j = 0; j < _row; j++)
                {
                    float posY = _gridAnchor.y + (_spaceY / 2f) + (_spaceY * j);
                    Vector3 pos = new Vector3(posX, posY, 0f);

                    GameObject apple = Instantiate(_apple, pos, Quaternion.identity, _board.transform);
                    apple.transform.localScale = new Vector3(_spaceX / _boardWidth, _spaceY / _boardHeight, 1f);
                    apple.TryGetComponent(out _apples[i, j]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        BoardInfoInit(_col, _row);
        for(int i = 0; i <= _col; i++)
        {
            Vector2 lineStartPos = new Vector2(_gridAnchor.x + i * _spaceX, _gridAnchor.y);
            Vector2 lineEndPos = new Vector2(_gridAnchor.x + i * _spaceX, _gridAnchor.y + _boardHeight);
            Gizmos.DrawLine(lineStartPos, lineEndPos);
        }

        for(int i = 0; i<= _row; i++)
        {
            Vector2 lineStartPos = new Vector2(_gridAnchor.x, _gridAnchor.y + i * _spaceY);
            Vector2 lineEndPos = new Vector2(_gridAnchor.x + _boardWidth, _gridAnchor.y + i * _spaceY);
            Gizmos.DrawLine(lineStartPos, lineEndPos);
        }
    }
}
