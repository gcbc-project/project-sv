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
        GameData data = GameManager.Get().Data;
        m_TextMeshProUGUI.text =  $"{data.Humans.Count}/{data.GetMaxHumans()}";
    }
}
