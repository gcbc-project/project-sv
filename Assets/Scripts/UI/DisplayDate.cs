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
        OnTimeChanged(GameManager.Get().Data.Time.Value);
    }

    private void OnTimeChanged(float newTime)
    {
        int year = (int)(newTime / GameData.Game_YearTime) + 1;
        int month = (int)(newTime / GameData.Game_MonthTime) % 12 + 1;
        int week = (int)(newTime / GameData.Game_WeekTime) % 4 + 1;

        m_TextMeshProUGUI.text = $"{year}년 {month}월 {week}주";
        //추후 Get~~() 생기면 숫자대신 집어넣기
    }
}
