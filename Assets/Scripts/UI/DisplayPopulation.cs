using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayPopulation : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshProUGUI;
    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        m_TextMeshProUGUI.text = "1"+"/"+"10";
        //���� GetNowPopul(),GetMaxPopul() ����� ���� ��� ����ֱ�
    }
}
