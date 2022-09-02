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

    /// <summary>ƒZƒ‹‚Ìó‘Ô</summary>
    public Stone Stone;
    /// <summary>ƒZƒ‹‚Ìó‘Ô</summary>
    public CellState State;
}
