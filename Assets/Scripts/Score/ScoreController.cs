using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] TMP_Text m_ScoreText;
    int m_Score;
    private void Awake()
    {
        m_ScoreText.text = m_Score.ToString();
    }
 
    public void UpdateScore(int pointGain)
    {
        m_Score += pointGain;
        m_ScoreText.text = m_Score.ToString();
    }
     
    public int GetCurrentScore()
    {
        return m_Score;
    }
}
