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
    public GameObject m_ReadyButton;
    public float m_WaitTime = 3.0f;

    public void LeaveRoom()
    {
        if (!PhotonNetwork.InRoom)
            return;

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Lobby");
    }

    public void CanLoadLevel()
    {
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
    }

    private void UpdateUI()
    {
        if (!PhotonNetwork.InRoom)
            return;

        m_CheckImage.gameObject.SetActive(PhotonNetwork.CurrentRoom.PlayerCount == RoomManager.m_MaxPlayers);
        m_RoomNameText.text = $"{PhotonNetwork.CurrentRoom.Name}";
        m_RoomAmountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {RoomManager.m_MaxPlayers}";
        m_ReadyButton.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1);
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }
}
