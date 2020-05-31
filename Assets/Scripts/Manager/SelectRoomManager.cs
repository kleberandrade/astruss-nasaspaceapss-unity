using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoomManager : MonoBehaviour
{
    public static SelectRoomManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Rooms")]
    public GameObject m_RoomButton;
    public GameObject m_RoomContent;
    public List<Room> m_rooms = new List<Room>();

    private void Start()
    {
        foreach (Room room in m_rooms)
        {
            GameObject button = Instantiate(m_RoomButton);
            button.transform.parent = m_RoomContent.transform;
            button.transform.localScale = Vector3.one;

            var script = button.GetComponent<RoomButton>();
            script.SetRoom(room);
        }
    }
}

[System.Serializable]
public class Room
{
    public string room_name;
    public int amount;
    public int max_size;
}