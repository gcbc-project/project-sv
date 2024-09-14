using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDate : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshProUGUI;
    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        m_TextMeshProUGUI.text = "1"+"년 "+"1"+"월 "+"1"+"주";
        //추후 Get~~() 생기면 숫자대신 집어넣기
    }
}
