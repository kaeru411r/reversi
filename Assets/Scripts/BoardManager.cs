using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class BoardManager : MonoBehaviour
{
    [Tooltip("セルのプレハブ")]
    [SerializeField] Cell _cellPrefab;
    [Tooltip("ボード半面の長さ")]
    [SerializeField] int _capaciousness = 4;

    /// <summary>盤面</summary>
    Cell[,] _board;
    /// <summary>グリッドレイアウトグループ</summary>
    GridLayoutGroup _group;
    /// <summary>レクトトランスフォーム</summary>
    RectTransform _rect;

    public Cell[,] Board
    {
        get
        {
            if (_board == null)
            {
                Debug.LogError($"{nameof(SetUp)}を呼び出し、盤面のセットアップを行ってください");
                _board = new Cell[0, 0];
            }
            return _board;
        }
    }

    public bool IsActive
    {
        get
        {
            return _board != null;
        }
    }

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
        _board = BoardCreate(_capaciousness, _cellPrefab);
        _group.constraintCount = _board.GetLength(0);
        float size = Mathf.Min(_rect.rect.width / _board.GetLength(1), _rect.rect.height / _board.GetLength(0));
        _group.cellSize = new Vector2(size, size);
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(1); k++)
            {
                Board[i, k].transform.SetParent(transform);
                if (k == _capaciousness - 1 && i == _capaciousness - 1 || k == _capaciousness && i == _capaciousness)
                {
                    Board[i, k].Stone = Stone.Player2;
                }
                else if (k == _capaciousness && i == _capaciousness - 1 || k == _capaciousness - 1 && i == _capaciousness)
                {
                    Board[i, k].Stone = Stone.Player1;
                }
            }
        }
    }

    public void Delete()
    {
        foreach(Cell c in _board)
        {
            Destroy(c.gameObject);
        }
    }

    public bool[,] AvailableCellsSearch(Stone stone)
    {
        bool[,] cells = new bool[Board.GetLength(0), Board.GetLength(1)];
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(0); k++)
            {
                cells[i, k] = AvailableCheck(k, i, stone);
            }
        }
        return cells;
    }

    /// <summary>
    /// (col, row)のセルにstoneの石を置けるか
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <param name="stone"></param>
    /// <returns></returns>
    bool AvailableCheck(int row, int col, Stone stone)
    {
        if (EreaChack(row, col))
        {
            if (TryGetCell(row, col, out Cell cell))
            {
                if (cell.Stone != Stone.Blank)
                {
                    return false;
                }
            }
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

    public void Highlight(Stone turn)
    {
        bool[,] availableCells = AvailableCellsSearch(turn);
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(1); k++)
            {
                if (availableCells[i, k])
                {
                    Board[i, k].State = CellState.Highlight;
                }
                else
                {
                    Board[i, k].State = CellState.Nomal;
                }

            }
        }
    }

    public void OffHighlight()
    {
        foreach (Cell c in _board)
        {
            c.State = CellState.Nomal;
        }
    }


    Cell GetCell(int row, int col)
    {
        Cell cell = null;
        if (EreaChack(row, col))
        {
            cell = Board[col, row];
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

    public bool Place(int row, int col, Stone player)
    {
        if (!EreaChack(row, col))
        {
            return false;
        }
        Board[col, row].Stone = player;
        if(ConcolorSearchVec(row, col, new Vector2Int(-1, -1), player) > 1)
        {
            Revaers(row, col, new Vector2Int(-1, -1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(0, -1), player) > 1)
        {
            Revaers(row, col, new Vector2Int(0, -1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(1, -1), player) > 1)
        {
            Revaers(row, col, new Vector2Int(1, -1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(1, 0), player) > 1)
        {
            Revaers(row, col, new Vector2Int(1, 0), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(1, 1), player) > 1)
        {
            Revaers(row, col, new Vector2Int(1, 1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(0, 01), player) > 1)
        {
            Revaers(row, col, new Vector2Int(0, 1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(-1, 1), player) > 1)
        {
            Revaers(row, col, new Vector2Int(-1, 1), player);
        }
        if (ConcolorSearchVec(row, col, new Vector2Int(-1, 0), player) > 1)
        {
            Revaers(row, col, new Vector2Int(-1, 0), player);
        }
        return true;
    }

    public bool Place(Cell cell, Stone player)
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int k = 0; k < Board.GetLength(1); k++)
            {
                return Place(k, i, player);
            }
        }
        return false;
    }

    int Revaers(int row, int col, Vector2Int vec, Stone player)
    {
        if (!EreaChack(row, col))
        {
            return -1;
        }
        if(TryGetCell(row + vec.x, col + vec.y , out Cell cell))
        {
            Stone stone = cell.Stone;
            if(cell.Stone == player)
            {
                return 0;
            }
            else if(stone == Stone.Blank)
            {
                return -2;
            }
            else
            {
                cell.Stone = player;
                int num = Revaers(row + vec.x, col + vec.y, vec, player);
                if (num >= 0)
                {
                    return num + 1;
                }
                else
                {
                    cell.Stone = stone;
                    return num;
                }
            }
        }
        return -1;
    }


    bool EreaChack(int row, int col)
    {
        if (row >= 0 && col >= 0)
        {
            if (Board.GetLength(0) > col && Board.GetLength(1) > row)
            {
                return true;
            }
        }
        return false;
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