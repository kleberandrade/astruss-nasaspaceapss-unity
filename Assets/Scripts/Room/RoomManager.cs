﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static byte m_MaxPlayers = 6;

    public Transform m_Content;
    public GameObject m_RoomCard;
    public InputField m_RoomNameInputField;
    public string m_LobySceneName = "Waiting";

    public void JoinRoom()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.InLobby)
            return;

        if (m_RoomNameInputField.text != string.Empty)
        {
            RoomOptions option = new RoomOptions { MaxPlayers = m_MaxPlayers };
            PhotonNetwork.CreateRoom(m_RoomNameInputField.text, option);
            SceneManager.LoadScene(m_LobySceneName);
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

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string room = $"Room {Random.Range(1000, 9999)}";
        PhotonNetwork.JoinLobby();
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("OnPlayerLeftRoom");
        if (!PhotonNetwork.CurrentRoom.IsOpen && !PhotonNetwork.CurrentRoom.IsVisible && PhotonNetwork.CurrentRoom.MaxPlayers > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
        }
    }
}