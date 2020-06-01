using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    public Text m_TextUI;
    public Card m_Card;
    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
    }

    public void SetCard(Card card)
    {
        m_Card = card;
        m_TextUI.text = card.label;
        m_Button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        GameManager.Instance.SelectWord(m_Card.label);
    }
}

