using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public bool m_UseTouch;
    public bool m_UseTimeToChangeScene;
    public float m_Time = 3.0f;
    public string m_SceneName;
    public AudioClip m_Clip;

    public bool m_Used;

    public void Start()
    {
        if (m_UseTimeToChangeScene)
        {
            Invoke("LoadLevel", m_Time);
        }
    }

    public void Update()
    {
        if (m_UseTouch)
        {
            if (Input.anyKeyDown)
            {
                LoadLevel(m_SceneName);
            }
        }
    }

    public void LoadLevelWithTime()
    {
        if (m_Used)
            return;
		
		if(m_Clip)
			AudioSource.PlayClipAtPoint(m_Clip, Camera.main.transform.position);
        m_Used = true;
        ScreenManager.Instance.LoadLevel(m_SceneName);
    }

    public void LoadLevel(string sceneName)
    {
        if (m_Used)
            return;
		
		if(m_Clip)
			AudioSource.PlayClipAtPoint(m_Clip, Camera.main.transform.position);
        m_Used = true;
        ScreenManager.Instance.LoadLevel(sceneName);
    }

    public void LoadLevelWithLoading(string sceneName)
    {
        if (m_Used)
            return;
		
		if(m_Clip)
			AudioSource.PlayClipAtPoint(m_Clip, Camera.main.transform.position);
        m_Used = true;
        ScreenManager.Instance.LoadLevelLoading(sceneName);
    }
}
