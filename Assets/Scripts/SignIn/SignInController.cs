using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInController : MonoBehaviourPunCallbacks
{
    public AudioSource Sfx_Click;
    public string m_LobySceneName = "Lobby";
    public InputField m_PlayerNameInputField;

    private string m_PlayerNamePrefs = "PLAYER_NAME";

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey(m_PlayerNamePrefs))
        {
            m_PlayerNameInputField.text = PlayerPrefs.GetString(m_PlayerNamePrefs);
        }
    }

    public void Login()
    {
        Sfx_Click.Play();
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene(){
        yield return new WaitForSeconds(0.25f);
        if (m_PlayerNameInputField.text != string.Empty)
        {
            PhotonNetwork.NickName = m_PlayerNameInputField.text;
            PlayerPrefs.SetString(m_PlayerNamePrefs, m_PlayerNameInputField.text);
            PlayerPrefs.Save();
        } 
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        }

        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene(m_LobySceneName);
        yield return null;
    }
}
