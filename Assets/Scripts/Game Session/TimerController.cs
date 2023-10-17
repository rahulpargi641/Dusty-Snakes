using UnityEngine;
using TMPro;
using System;

public class TimerController : MonoBehaviour
{
    [SerializeField] TMP_Text m_TimerText;
    [SerializeField] LevelController m_LevelController;
    [SerializeField] float m_MaxTime;
    float m_CurrentTime;

    private void Awake()
    {
        m_TimerText.text = m_MaxTime.ToString();
    }

    private void Update()
    {
        m_CurrentTime += Time.deltaTime;
        if(m_CurrentTime >= 1)
        {
            m_MaxTime -=1;
            if(m_MaxTime < 0)
            {
                m_LevelController.TimerExpired();
            }
            else
            {
                m_CurrentTime = 0;
                UpdateTimerText();
            }
        }
    }

    private void UpdateTimerText()
    {
        m_TimerText.text = m_MaxTime.ToString();
    }
}
