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
    [SerializeField] Player _turn = Player.Player1;

    /// <summary>盤面</summary>
    Cell[,] _board;
    /// <summary>グリッドレイアウトグループ</summary>
    GridLayoutGroup _group;


    // Start is called before the first frame update
    void Start()
    {
        _group = GetComponent<GridLayoutGroup>();
        _group.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _group.startCorner = GridLayoutGroup.Corner.UpperLeft;
        _group.startAxis = GridLayoutGroup.Axis.Horizontal;
        _group.childAlignment = TextAnchor.MiddleCenter;
        SetUp();
    }

    static public Cell[,] BoardSetUp(int capaciousness, Cell prefab)
    {
        Cell[,] board = new Cell[capaciousness * 2, capaciousness * 2];
        for (int i = 0; i < capaciousness * 2; i++)
        {
            for (int k = 0; k < capaciousness * 2; k++)
            {
                board[i, k] = Instantiate(prefab);
            }
        }
        return board;
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetUp()
    {
        _group.constraintCount = _capaciousness * 2;
        if (!_cellPrefab)
        {
            Debug.LogError($"{nameof(_cellPrefab)}がnullです");
            return;
        }
        _board = BoardSetUp(_capaciousness, _cellPrefab);
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.GetLength(1); k++)
            {
                _board[i, k].transform.SetParent(transform);
                if (k == _capaciousness - 1 && i == _capaciousness - 1 || k == _capaciousness && i == _capaciousness)
                {
                    _board[i, k].ChangeState(CellStone.Player2);
                }
                else if (k == _capaciousness && i == _capaciousness - 1 || k == _capaciousness - 1 && i == _capaciousness)
                {
                    _board[i, k].ChangeState(CellStone.Player1);
                }
            }
        }
    }

    bool[,] AvailableCellsSearch()
    {
        bool[,] cells = new bool[_board.GetLength(0), _board.GetLength(1)];
        return cells;
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
        if(!EreaChack(row, col))
        {
            return false;
        }
        _board[col, row].ChangeState(CellStone.Player1);
        return false;
    }

    bool EreaChack(int row, int col)
    {
        if(row >= 0 || col >= 0)
        {
            if(_board.GetLength(0) > col && _board.GetLength(1) > row)
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
        if(!eventData.pointerCurrentRaycast.gameObject.TryGetComponent<Cell>(out cell))
        {
            cell = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Cell>();
        }
        if (cell)
        {
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                for (int k = 0; k < _board.GetLength(1); k++)
                {
                    if(cell == _board[i, k])
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