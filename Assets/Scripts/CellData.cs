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

    /// <summary>�Z���̏��</summary>
    public Stone Stone;
    /// <summary>�Z���̏��</summary>
    public CellState State;
}
