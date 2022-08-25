using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("セルの状態")]
    [SerializeField] CellState _state = CellState.Blank;
    [Tooltip("ボードの色")]
    [SerializeField] Color _boardColor = new Color(0, 115, 74);
    [Tooltip("プレイヤー1の石の色")]
    [SerializeField] Color _player1Color = Color.black;
    [Tooltip("プレイヤー2の石の色")]
    [SerializeField] Color _player2Color = Color.white;

    Image _image;

    private void Awake()
    {
        SetUp();
        Debug.Log("Awake");
    }

    private void OnValidate()
    {
        Debug.Log("OnValidate");
        Transcription();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }


    public void SetUp()
    {
        _image = GetComponent<Image>();
        Debug.Log("SetUp");
    }

    void Transcription()
    {
        if (_image == null)
        {
            SetUp();
        }
        switch (_state)
        {
            case CellState.Blank:
                _image.color = _boardColor;
                break;
            case CellState.Player1:
                _image.color = _player1Color;
                break;
            case CellState.Player2:
                _image.color = _player2Color;
                break;
            default:
                break;
        }
        Debug.Log("Transcription");
    }
}


/// <summary>
/// セルの状態
/// </summary>
public enum CellState
{
    /// <summary>空き</summary>
    Blank = 0,
    /// <summary>プレイヤー1</summary>
    Player1 = 1,
    /// <summary>プレイヤー2</summary>
    Player2 = 2,
}
