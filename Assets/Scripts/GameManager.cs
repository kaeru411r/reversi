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
            Place(cell);
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
                    return _board.Place(k, i);
                }
            }
        }
        return false;
    }
}
