using UnityEngine;

public class DragChecker : MonoBehaviour
{
    public enum CheckerState
    {
        Idle,
        Dragging,
        End
    }

    [SerializeField] private GameObject checkerObject;

    private Transform _checkerTransform;
    private SpriteRenderer _checkSpriteRenderer;

    private Vector2 _startPos;
    private Vector2 _endPos;

    private CheckerState _state = CheckerState.Idle;

    public CheckerState State => _state;
    public float TopY => _checkerTransform.position.y + Mathf.Abs(_checkSpriteRenderer.size.y) / 2f;
    public float BottomY => _checkerTransform.position.y - Mathf.Abs(_checkSpriteRenderer.size.y) / 2f;
    public float LeftX => _checkerTransform.position.x - Mathf.Abs(_checkSpriteRenderer.size.x) / 2f;
    public float RightX => _checkerTransform.position.x + Mathf.Abs(_checkSpriteRenderer.size.x) / 2f;

    public SpriteRenderer CheckerSpriteRenderer => _checkSpriteRenderer;

    private void Start()
    {
        if (!checkerObject.TryGetComponent(out _checkerTransform))
        {
            Debug.Log("실패: Transform");
        }
        if (!checkerObject.TryGetComponent(out _checkSpriteRenderer))
        {
            Debug.Log("실패: SpriteRenderer");
        }
        checkerObject.gameObject.SetActive(false);
    }

    private void Update()
    {
        _state = CheckerState.Idle;
        CheckerMouseDown();
        CheckerMouseDrag();
        CheckerMouseUp();
    }

    private void CheckerMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
                _state = CheckerState.Dragging;
                _endPos = currentMousePos;
                SetCheckerTransform();
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

    private void CheckerMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _state = CheckerState.End;
            CheckerSpriteRenderer.color = new Vector4(1, 0, 0, 0.4f);
            checkerObject.SetActive(false);
        }
    }

    public float[] GetCheckerSize()
    {
        return new float[] { LeftX, RightX, TopY, BottomY };
    }
}
