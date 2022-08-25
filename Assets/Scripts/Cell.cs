using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("�Z���̏��")]
    [SerializeField] CellState _state = CellState.Blank;
    [Tooltip("�{�[�h�̐F")]
    [SerializeField] Color _boardColor = new Color(0, 115, 74);
    [Tooltip("�v���C���[1�̐΂̐F")]
    [SerializeField] Color _player1Color = Color.black;
    [Tooltip("�v���C���[2�̐΂̐F")]
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
/// �Z���̏��
/// </summary>
public enum CellState
{
    /// <summary>��</summary>
    Blank = 0,
    /// <summary>�v���C���[1</summary>
    Player1 = 1,
    /// <summary>�v���C���[2</summary>
    Player2 = 2,
}
