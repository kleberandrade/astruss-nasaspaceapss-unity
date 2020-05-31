using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Transform m_Content;
    public GameObject m_RoomCard;
    public byte m_MaxPlayers = 6;
    public InputField m_RoomNameInputField;

    public void JoinRoom()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (m_RoomNameInputField.text != string.Empty)
        {
            RoomOptions option = new RoomOptions { MaxPlayers = m_MaxPlayers };
            PhotonNetwork.CreateRoom(m_RoomNameInputField.text, option);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform item in m_Content)
        {
            Destroy(item.gameObject);
        }

        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                continue;


            GameObject card = Instantiate(m_RoomCard);
            card.transform.parent = m_Content.transform;
            card.transform.localScale = Vector3.one;

            RoomModel room = new RoomModel
            {
                roomName = info.Name,
                maxSize = info.MaxPlayers,
                amount = info.PlayerCount
            };

            var script = card.GetComponent<RoomCard>();
            script.SetRoom(room);
        }
    }

    public void LeaveRoom()
    {
        if (!PhotonNetwork.InRoom)
            return;

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string room = $"Room {Random.Range(1000, 9999)}";
        PhotonNetwork.CreateRoom(room);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("OnConnectedToMaster");
        Debug.Log($"Server: {PhotonNetwork.CloudRegion} Ping {PhotonNetwork.GetPing()}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log($"Room name: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Current player in room: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
}