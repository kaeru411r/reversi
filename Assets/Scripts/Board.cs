using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class Board : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("�Z���̃v���n�u")]
    [SerializeField] Cell _cellPrefab;
    [Tooltip("�{�[�h���ʂ̒���")]
    [SerializeField] int _capaciousness = 4;
    [Tooltip("�^�[���v���C���[")]
    [SerializeField] Player _turn = Player.Player1;

    /// <summary>�Ֆ�</summary>
    Cell[,] _board;
    /// <summary>�O���b�h���C�A�E�g�O���[�v</summary>
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
            Debug.LogError($"{nameof(_cellPrefab)}��null�ł�");
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
            Debug.LogWarning($"{nameof(_capaciousness)}��0�ȉ��ɐݒ肷�邱�Ƃ͏o���܂���");
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
/// �v���C���[�̎��ʎq
/// </summary>
public enum Player
{
    Player1,
    Player2,    
}