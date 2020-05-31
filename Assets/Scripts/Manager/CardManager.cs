using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Cards")]
    public GameObject m_CardButton;
    public List<Card> m_Cards = new List<Card>();
    public GameObject m_CardParent;

    private void Start()
    {
        foreach (Card card in m_Cards)
        {
            GameObject button = Instantiate(m_CardButton);
            button.transform.parent = m_CardParent.transform;
            button.transform.localScale = Vector3.one;

            var script = button.GetComponent<CardButton>();
            script.SetCard(card);
        }
    }
}

[System.Serializable]
public class Card
{
    public string label;
}