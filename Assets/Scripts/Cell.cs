using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("セルの状態")]
    [SerializeField] CellData _cellData = new CellData(Stone.Blank, CellState.Nomal);
    [Tooltip("ボードの色")]
    [SerializeField] Color _boardColor = new Color(0, 115, 74);
    [Tooltip("プレイヤー1の石の色")]
    [SerializeField] Color _player1Color = Color.black;
    [Tooltip("プレイヤー2の石の色")]
    [SerializeField] Color _player2Color = Color.white;
    [Tooltip("ハイライトされているときにかかるマスク")]
    [SerializeField] Color _highlightMaskColor = new Color(50, 50, 50);

    Image _image;

    public Stone Stone
    {
        get => _cellData.Stone;
        set
        {
            _cellData.Stone = value;
            Transcription();
        }
    }
    public CellState State
    {
        get => _cellData.State;
        set
        {
            _cellData.State = value;
            Transcription();
        }
    }


    private void Awake()
    {
        SetUp();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        Transcription();
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
    }


    public void SetUp()
    {
        _image = GetComponent<Image>();
    }

    void Transcription()
    {
        if (_image == null)
        {
            SetUp();
        }
        switch (_cellData.Stone)
        {
            case Stone.Blank:
                _image.color = _boardColor;
                break;
            case Stone.Player1:
                _image.color = _player1Color;
                break;
            case Stone.Player2:
                _image.color = _player2Color;
                break;
            default:
                break;
        }
        if (_cellData.State == CellState.Highlight)
        {
            _image.color += _highlightMaskColor;
        }
    }
}


/// <summary>
/// セルの状態
/// </summary>
public enum Stone
{
    /// <summary>空き</summary>
    Blank = 0,
    /// <summary>プレイヤー1</summary>
    Player1 = 1,
    /// <summary>プレイヤー2</summary>
    Player2 = 2,
}

public enum CellState
{
    Nomal,
    Highlight,
}
