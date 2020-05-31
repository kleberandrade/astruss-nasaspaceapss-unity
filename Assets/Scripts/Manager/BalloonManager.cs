using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BalloonManager : MonoBehaviour
{
    public Text Ballon_Title;
    public Text Ballon_Text;
    public string[] Title;
    public string[] Text;
    public int Index = 0;

    [Header("Transition")]
    public FadeInOut m_Fader;

    [Header("Loading")]
    public GameObject m_LoadingPanel;
    public float m_DelayAfterLaoding = 2.0f;
    

    public void NextBalloon()
    {
        Index++;
        if(Title.Length <= Index){
            LoadLevel("SingIn");
        }else{
            Ballon_Title.text = Title[Index];
            Ballon_Text.text = Text[Index];
        }
    }

    public void LoadLevel(string nextSceneName)
    {
        StartCoroutine(ChangeScene(nextSceneName, false));
    }

    private IEnumerator ChangeScene(string nextSceneName, bool loading)
    {
        List<BehaviorUI> list = Helper.FindAll<BehaviorUI>();
        foreach (BehaviorUI ui in list)
            ui.Disable();

        m_Fader.Show();
        yield return new WaitForSeconds(m_Fader.m_Time);

        if (loading)
        {
            m_LoadingPanel.SetActive(true);

            m_Fader.Hide();
            yield return new WaitForSeconds(m_Fader.m_Time);
        }

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(nextSceneName);
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            if (asyncScene.progress >= 0.9f)
            {
                if (loading)
                {
                    yield return new WaitForSeconds(m_DelayAfterLaoding);

                    m_Fader.Show();
                    yield return new WaitForSeconds(m_Fader.m_Time);

                    m_LoadingPanel.SetActive(false);
                }

                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
