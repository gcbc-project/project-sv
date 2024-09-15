using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayGold : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshProUGUI;
    private void Awake()
    {
        m_TextMeshProUGUI=GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        m_TextMeshProUGUI.text = $"{GameManager.Get().Data.Gold.Value}G";
        //���� GetGold() ����� "100"��� ����ֱ�
    }
}
