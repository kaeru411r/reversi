using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class Board : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("セルのプレハブ")]
    [SerializeField] Cell _cellPrefab;
    [Tooltip("ボード半面の長さ")]
    [SerializeField] int _capaciousness = 4;
    [Tooltip("ターンプレイヤー")]
    [SerializeField] Stone _turn = Stone.Player1;

    /// <summary>盤面</summary>
    Cell[,] _board;
    /// <summary>グリッドレイアウトグループ</summary>
    GridLayoutGroup _group;
    /// <summary>レクトトランスフォーム</summary>
    RectTransform _rect;
    /// <summary>配置可能なセル</summary>
    bool[,] _availableCells;


    private void Awake()
    {
        _group = GetComponent<GridLayoutGroup>();
        _group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _group.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _group.startAxis = GridLayoutGroup.Axis.Horizontal;
        _group.childAlignment = TextAnchor.MiddleCenter;
        _rect = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        Highlight();
    }

    static public Cell[,] BoardCreate(int capaciousness, Cell prefab)
    {
        Cell[,] board = new Cell[capaciousness * 2, capaciousness * 2];
        for (int i = 0; i < capaciousness * 2; i++)
        {
            for (int k = 0; k < capaciousness * 2; k++)
            {
                board[i, k] = Instantiate(prefab);
                board[i, k].name = $"cell({i}, {k})";
            }
        }
        return board;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetUp()
    {
        if (!_cellPrefab)
        {
            Debug.LogError($"{nameof(_cellPrefab)}がnullです");
            return;
        }
        _group.constraintCount = _capaciousness * 2;
        float size = Mathf.Min(_rect.rect.width / (_capaciousness * 2), _rect.rect.height / (_capaciousness * 2));
        _group.cellSize = new Vector2(size, size);
        _board = BoardCreate(_capaciousness, _cellPrefab);
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.GetLength(1); k++)
            {
                _board[i, k].transform.SetParent(transform);
                if (k == _capaciousness - 1 && i == _capaciousness - 1 || k == _capaciousness && i == _capaciousness)
                {
                    _board[i, k].Stone = Stone.Player2;
                }
                else if (k == _capaciousness && i == _capaciousness - 1 || k == _capaciousness - 1 && i == _capaciousness)
                {
                    _board[i, k].Stone = Stone.Player1;
                }
            }
        }
    }

    bool[,] AvailableCellsSearch(Stone stone)
    {
        bool[,] cells = new bool[_board.GetLength(0), _board.GetLength(1)];
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.GetLength(0); k++)
            {
                cells[i, k] = AvailableCheck(k, i, stone);
            }
        }
        return cells;
    }

    bool AvailableCheck(int row, int col, Stone stone)
    {
        if (EreaChack(row, col))
        {
            if (ConcolorSearchVec(row, col, new Vector2Int(1, 1), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(1, 0), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(1, -1), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(0, -1), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(-1, -1), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(-1, 0), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(0, 1), stone) > 1)
            {
                return true;
            }
            if (ConcolorSearchVec(row, col, new Vector2Int(-1, 1), stone) > 1)
            {
                return true;
            }
        }

        return false;
    }

    int ConcolorSearchVec(int row, int col, Vector2Int vec, Stone stone)
    {
        int x = vec.x != 0 ? vec.x / Mathf.Abs(vec.x) : 0;
        int y = vec.y != 0 ? vec.y / Mathf.Abs(vec.y) : 0;
        int length = 0;
        if (TryGetCell(row + x, col + y, out Cell cell))
        {
            if (cell.Stone != Stone.Blank)
            {
                if (stone != cell.Stone)
                {
                    length = ConcolorSearchVec(row + x, col + y, vec, stone);
                    if (length != 0)
                    {
                        length++;
                    }
                    else { }
                }
                else if (cell.Stone == stone)
                {
                    length = 1;
                }
                else { }
            }
        }
        return length;
    }

    void Highlight()
    {
        _availableCells = AvailableCellsSearch(_turn);
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.GetLength(1); k++)
            {
                if (_availableCells[i, k])
                {
                    _board[i, k].State = CellState.Highlight;
                }
                else
                {
                    _board[i, k].State = CellState.Nomal;
                }

            }
        }
    }


    Cell GetCell(int row, int col)
    {
        Cell cell = null;
        if (EreaChack(row, col))
        {
            cell = _board[col, row];
        }
        return cell;
    }
    bool TryGetCell(int row, int col, out Cell cell)
    {
        cell = GetCell(row, col);
        return cell;
    }

    private void OnValidate()
    {
        if (_capaciousness <= 0)
        {
            _capaciousness = 1;
            Debug.LogWarning($"{nameof(_capaciousness)}は0以下に設定することは出来ません");
        }
    }

    public bool Place(int row, int col)
    {
        if (!EreaChack(row, col))
        {
            return false;
        }
        _board[col, row].Stone = Stone.Player1;
        return true;
    }

    bool EreaChack(int row, int col)
    {
        if (row >= 0 && col >= 0)
        {
            if (_board.GetLength(0) > col && _board.GetLength(1) > row)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        Cell cell;
        if (!eventData.pointerCurrentRaycast.gameObject.TryGetComponent<Cell>(out cell))
        {
            cell = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Cell>();
        }
        if (cell)
        {
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int k = 0; k < _board.GetLength(1); k++)
                {
                    if (cell == _board[i, k] && _availableCells[i, k])
                    {
                        Place(k, i);
                    }
                }
            }
        }
    }
}

/// <summary>
/// プレイヤーの識別子
/// </summary>
public enum Player
{
    Player1,
    Player2,
}