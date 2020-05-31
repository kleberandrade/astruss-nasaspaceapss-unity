using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitManager : MonoBehaviourPunCallbacks
{
    public Text m_RoomNameText;
    public Text m_RoomAmountText;
    public GameObject m_CheckImage;
    public Button m_ReadyButton;
    public float m_WaitTime = 3.0f;

    public void LeaveRoom()
    {
        if (!PhotonNetwork.InRoom)
            return;

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    public void CanLoadLevel()
    {
        UpdateUI();

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat($"PhotonNetwork : Loading Level : {PhotonNetwork.CurrentRoom.PlayerCount}");
            if (PhotonNetwork.CurrentRoom.PlayerCount == RoomManager.m_MaxPlayers)
            {
                LoadLevel(m_WaitTime);
            }
        }
    }

    public void LoadLevel(float waitTime)
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        StartCoroutine(Loading(waitTime));
    }

    public IEnumerator Loading(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PhotonNetwork.LoadLevel("Gameplay");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        UpdateUI();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat($"OnPlayerEnteredRoom() {other.NickName}");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat($"OnPlayerEnteredRoom IsMasterClient {PhotonNetwork.IsMasterClient}");
            CanLoadLevel();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat($"OnPlayerLeftRoom() {other.NickName}");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            CanLoadLevel();
        }
    }

    private void UpdateUI()
    {
        m_CheckImage.gameObject.SetActive(PhotonNetwork.CurrentRoom.PlayerCount == RoomManager.m_MaxPlayers);
        m_RoomNameText.text = $"{PhotonNetwork.CurrentRoom.Name}";
        m_RoomAmountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {RoomManager.m_MaxPlayers}";
        m_ReadyButton.interactable = PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1;
    }
}
