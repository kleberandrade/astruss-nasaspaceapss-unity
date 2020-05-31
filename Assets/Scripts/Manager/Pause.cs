using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool m_IsPaused {get; set;}
    public GameObject m_PausePanel;

    public void Show(){
        m_PausePanel.SetActive(true);
        m_IsPaused = true;
        Time.timeScale = 0.0f;
    }

    public void Hide(){
        m_PausePanel.SetActive(false);
        m_IsPaused = false;
        Time.timeScale = 1.0f;
    }

    public void Toggle()
    {
        if(m_IsPaused)
            Hide();
        else
            Show();
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)) Toggle();
    }
}
