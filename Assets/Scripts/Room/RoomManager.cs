using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static string m_ChannelNamePrefs = "CHANNEL_NAME";
    public static string m_CreateChannelNamePrefs = "CREATE_CHANNEL_NAME";
    public static byte m_MaxPlayers = 6;

    public Transform m_Content;
    public GameObject m_RoomCard;
    public InputField m_RoomNameInputField;
    public string m_LobySceneName = "Waiting";

    private Dictionary<string, RoomInfo> m_CachedRoomList = new Dictionary<string, RoomInfo>();
    private Dictionary<string, GameObject> m_RoomList = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (PlayerPrefs.HasKey(m_CreateChannelNamePrefs))
        {
            m_RoomNameInputField.text = PlayerPrefs.GetString(m_CreateChannelNamePrefs);
        }
    }

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
            PlayerPrefs.SetString(m_CreateChannelNamePrefs, m_RoomNameInputField.text);
            PlayerPrefs.SetString(m_ChannelNamePrefs, m_RoomNameInputField.text);
            PlayerPrefs.Save();

            RoomOptions option = new RoomOptions { MaxPlayers = m_MaxPlayers };
            PhotonNetwork.CreateRoom(m_RoomNameInputField.text, option);
            SceneManager.LoadScene(m_LobySceneName);
        }
    }

    public void ClearRoomList()
    {
        foreach (Transform item in m_Content)
            Destroy(item.gameObject);

        m_RoomList.Clear();
    }

    public void UpdateRoomList()
    {
        foreach (RoomInfo info in m_CachedRoomList.Values)
        {
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

            m_RoomList.Add(info.Name, card);
        }
    }

    public void UpdateCacheRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (m_CachedRoomList.ContainsKey(info.Name))
                {
                    m_CachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (m_CachedRoomList.ContainsKey(info.Name))
            {
                m_CachedRoomList[info.Name] = info;
            } 
            else
            {
                m_CachedRoomList.Add(info.Name, info);
            }
        }
    }

    public override void OnLeftLobby()
    {
        m_CachedRoomList.Clear();
        ClearRoomList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();
        UpdateCacheRoomList(roomList);
        UpdateRoomList();
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