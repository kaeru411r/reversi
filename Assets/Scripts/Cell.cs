using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour
{
    [Tooltip("�Z���̏��")]
    [SerializeField] Stone _stone = Stone.Blank;
    [Tooltip("�Z���̏��")]
    [SerializeField] CellState _state = CellState.Nomal;
    [Tooltip("�{�[�h�̐F")]
    [SerializeField] Color _boardColor = new Color(0, 115, 74);
    [Tooltip("�v���C���[1�̐΂̐F")]
    [SerializeField] Color _player1Color = Color.black;
    [Tooltip("�v���C���[2�̐΂̐F")]
    [SerializeField] Color _player2Color = Color.white;
    [Tooltip("�n�C���C�g����Ă���Ƃ��ɂ�����}�X�N")]
    [SerializeField] Color _highlightMaskColor = new Color(50, 50, 50);

    Image _image;

    public Stone Stone
    {
        get => _stone;
        set
        {
            _stone = value;
            Transcription();
        }
    }
    public CellState State
    {
        get => _state;
        set
        {
            _state = value;
            Transcription();
        }
    }


    private void Awake()
    {
        SetUp();
    }

    private void OnValidate()
    {
        Transcription();
    }

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
        switch (_stone)
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
        if (_state == CellState.Highlight)
        {
            _image.color += _highlightMaskColor;
        }
    }
}


/// <summary>
/// �Z���̏��
/// </summary>
public enum Stone
{
    /// <summary>��</summary>
    Blank = 0,
    /// <summary>�v���C���[1</summary>
    Player1 = 1,
    /// <summary>�v���C���[2</summary>
    Player2 = 2,
}

public enum CellState
{
    Nomal,
    Highlight,
}
