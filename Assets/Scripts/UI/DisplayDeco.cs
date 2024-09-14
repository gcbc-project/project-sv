using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayDeco : MonoBehaviour
{
    TextMeshProUGUI m_TextMeshProUGUI;
    private void Awake()
    {
        m_TextMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        m_TextMeshProUGUI.text = "10";
        //추후 GetDeco() 생기면 "10"대신 집어넣기
    }
}
