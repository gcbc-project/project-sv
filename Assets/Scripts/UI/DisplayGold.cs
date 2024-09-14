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
        m_TextMeshProUGUI.text = "100" + "G";
        //추후 GetGold() 생기면 "100"대신 집어넣기
    }
}
