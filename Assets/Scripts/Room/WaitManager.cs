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
        StartCoroutine(Coroutine_LeaveRoom());
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
        StartCoroutine(Coroutine_LoadLevel(waitTime));
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

        PhotonNetwork.CurrentRoom.IsOpen = PhotonNetwork.CurrentRoom.PlayerCount != RoomManager.m_MaxPlayers;
        PhotonNetwork.CurrentRoom.IsVisible = PhotonNetwork.CurrentRoom.PlayerCount != RoomManager.m_MaxPlayers;
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }

    private IEnumerator Coroutine_LeaveRoom(){
        yield return new WaitForSeconds(0.25f);
        if (!PhotonNetwork.InRoom)
            yield return null;
        else{
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
            SceneManager.LoadScene("Lobby");
            yield return null;
        }
    }

    private IEnumerator Coroutine_LoadLevel(float waitTime){
        yield return new WaitForSeconds(0.25f);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        StartCoroutine(Loading(waitTime));
        yield return null;
    }
}
