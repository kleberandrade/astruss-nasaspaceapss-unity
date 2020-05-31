using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignInController : MonoBehaviourPunCallbacks
{
    public string m_LobySceneName = "Lobby";
    public InputField m_PlayerNameInputField;

    private string m_PlayerNamePrefs = "PLAYER_NAME";

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PlayerPrefs.HasKey(m_PlayerNamePrefs))
        {
            m_PlayerNameInputField.text = PlayerPrefs.GetString(m_PlayerNamePrefs);
        }
    }

    public void Login()
    {
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
    }
}
