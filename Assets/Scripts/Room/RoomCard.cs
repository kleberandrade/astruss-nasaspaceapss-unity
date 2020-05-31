using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomCard : MonoBehaviour
{
    public Text m_RoomName;
    public Text m_RoomPlayers;

    private RoomModel m_Room;
    
    public void SetRoom(RoomModel room)
    {
        m_Room = room;
        m_RoomName.text = room.roomName;
        m_RoomPlayers.text = $"{room.amount} / {room.maxSize}";
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinRoom(m_Room.roomName);
        }
    }
}
