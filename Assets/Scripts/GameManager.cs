using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("ボード")]
    [SerializeField] BoardManager _board;
    [Tooltip("ターンプレイヤー")]
    [SerializeField] Stone _turn = Stone.Player1;
    [Tooltip("棋譜")]
    [SerializeField] string _record = "";


    /// <summary>配置可能なセル</summary>
    bool[,] _availableCells;


    // Start is called before the first frame update
    void Start()
    {
        _board.SetUp();
        _board.Highlight(_turn);
        _availableCells = _board.AvailableCellsSearch(_turn);
        //_board.OffHighlight();
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (_board.IsActive)
            {
                TurnChange(_turn, 0);
                RecordReproduction();
            }
        }
    }
#endif


    bool RecordReproduction()
    {
        _board.Delete();
        _board.SetUp();
        _record.Replace(' ', '\0');
        string[] data = _record.Split(',');
        List<int> nums = new List<int>();
        for (int i = 0; i < data.Length; i++)
        {
            try
            {
                nums.Add(int.Parse(data[i]));
            }
            catch(FormatException e)
            {

            }
        }
        for(int i = 0; i < nums.Count - 1; i += 2)
        {
            if(_board.Place(nums[i], nums[i + 1], _turn))
            {
                TurnChange();
            }
            else
            {
                return false;
            }
        }
        return true;
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
            if (Place(cell))
            {
                TurnChange();
            }
        }
    }

    public bool Place(Cell cell)
    {
        for (int i = 0; i < _board.Board.GetLength(0); i++)
        {
            for (int k = 0; k < _board.Board.GetLength(1); k++)
            {
                if (cell == _board.Board[i, k] && _availableCells[i, k])
                {
                    if (_board.Place(k, i, _turn))
                    {
                        Record(k, i);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void Record(int row, int col)
    {
        string re = "";
        if (_record.Length > 0)
        {
            re = ",";
        }
        re += $"{row + 1},{col + 1}";
        _record += re;
    }

    void TurnChange()
    {
        Stone turn = _turn;
        if (_turn == Stone.Player1)
        {
            turn = Stone.Player2;
        }
        else if (_turn == Stone.Player2)
        {
            turn = Stone.Player1;
        }

        TurnChange(turn, 0);
    }

    void TurnChange(Stone turn, int number)
    {
        if(turn == Stone.Blank)
        {
            turn = Stone.Player1;
        }
        _turn = turn;
        _board.Highlight(_turn);
        _availableCells = _board.AvailableCellsSearch(_turn);
        if (!AvailableCellCheck())
        {
            if (number <= 0)
            {
                Debug.Log("Skip");
                if (turn == Stone.Player1)
                {
                    turn = Stone.Player2;
                }
                else if (turn == Stone.Player2)
                {
                    turn = Stone.Player1;
                }
                else { }
                TurnChange(turn, 1);
            }
            else
            {
                Debug.Log("Finish");
            }
        }
    }

    bool AvailableCellCheck()
    {
        foreach (bool b in _availableCells)
        {
            if (b)
            {
                return true;
            }
        }
        return false;
    }
}
