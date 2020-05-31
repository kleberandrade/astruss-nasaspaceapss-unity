using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text m_TextUI;
    public Text m_Amount;
    public Room m_Room;
    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
    }

    public void SetRoom(Room room)
    {
        m_Room = room;
        m_TextUI.text = room.room_name;
        m_Amount.text = room.amount.ToString() + "/" + room.max_size.ToString();
        // m_Button.onClick.AddListener(() => ShowDetails());
    }
}

