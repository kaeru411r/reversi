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

    /// <summary>�Ֆ�</summary>
    Cell[,] _board;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
