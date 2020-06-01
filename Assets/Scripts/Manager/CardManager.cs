using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [Header("Cards")]
    public GameObject m_CardButton;
    public List<GameObject> m_Cards = new List<GameObject>();
    public GameObject m_CardParent;

    public void SetWords(List<string> words)
    {
        foreach (var card in m_Cards)
            Destroy(card);

        m_Cards.Clear();
        foreach (var word in words)
        {
            GameObject button = Instantiate(m_CardButton);
            button.transform.parent = m_CardParent.transform;
            button.transform.localScale = Vector3.one;

            Card card = new Card()
            {
                label = word,
            };

            var script = button.GetComponent<CardButton>();
            script.SetCard(card);

            m_Cards.Add(button);
        }
    }
}

[System.Serializable]
public class Card
{
    public string label;
}