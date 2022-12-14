using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CellData
{
    public CellData(Stone stone, CellState state)
    {
        Stone = stone;
        State = state;
    }

    /// <summary>セルの状態</summary>
    public Stone Stone;
    /// <summary>セルの状態</summary>
    public CellState State;
}
