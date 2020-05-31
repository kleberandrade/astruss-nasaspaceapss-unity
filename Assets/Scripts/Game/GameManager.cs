using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Rules")]
    public int m_MaxQuestions = 50;

    [Header("Points")]
    public int m_NextQuestionPoint = 1;
    public int m_WrongAnswerPoint = 5;

    [Header("Words")]
    public int m_NumberWordSelect = 3;
    public TextAsset m_TextFile;
    public List<string> m_Words = new List<string>();

    [Header("UI")]
    public Text m_WordTextUI;
    public Text m_ScoreTextUI;

    [Header("Dialogs")]
    public GameObject m_TutorialDialog;
    public GameObject m_SelectWordDialog;
    public GameObject m_QuitDialog;
    public GameObject m_GameoverDialog;

    private PhotonView m_PhotonView;
    private int m_QuestionUsed = 0;
    private int m_PlayeSelected;

    private List<string> m_WordSelectedList = new List<string>();
    private int m_WordSelected = 0;

    private void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_PhotonView.RPC("OnSelectPlayer", RpcTarget.AllBuffered);
        }


        //var words = m_TextFile.text.Split('\n');
        //m_Words = new List<string>(words);

        //
        //m_PlayerList = PhotonNetwork.CurrentRoom.Players;
        //foreach (KeyValuePair<int, Player> player in m_PlayerList)
        //{
        //     Debug.Log(player.Value.NickName);
        //}

        //if (PhotonNetwork.IsMasterClient)
        //{
            //m_PhotonView.RPC("OnSelectPlayer", RpcTarget.AllBuffered);
            //m_PhotonView.RPC("OnSelectWord", RpcTarget.AllBuffered);
        //}

        UpdateUI();
    }

    [PunRPC]
    private void OnSelectPlayer()
    {
        m_PlayeSelected = Random.Range(0, PhotonNetwork.CurrentRoom.Players.Count);
        Debug.Log($"Pilot is {m_PlayeSelected}");

        m_TutorialDialog.SetActive(true);
    }

    [PunRPC]
    private void OnSelectWord()
    {
        Debug.Log("OnSelectWord");
        m_WordSelectedList.Clear();
        while (m_WordSelectedList.Count < m_NumberWordSelect)
        {
            var index = Random.Range(0, m_WordSelectedList.Count);
            var word = m_Words[index];
            if (!m_WordSelectedList.Contains(word))
            {
                m_WordSelectedList.Add(word);
                Debug.Log($"Word selected: {word}");
            }
        }
    }

    public void NextAnswer()
    {
        m_PhotonView.RPC("OnNextAnswer", RpcTarget.AllBuffered, m_NextQuestionPoint);
    }

    public void WrongAnswer()
    {
        m_PhotonView.RPC("OnNextAnswer", RpcTarget.AllBuffered, m_WrongAnswerPoint);
    }

    [PunRPC]
    private void OnNextAnswer(int points)
    {
        m_QuestionUsed += points;
        UpdateUI();

        if (m_QuestionUsed >= m_MaxQuestions)
        {
            m_GameoverDialog.SetActive(true);
        }
    }

    public void RightAnswer()
    {
        m_PhotonView.RPC("OnRightAsked", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void OnRightAsked()
    {
        m_GameoverDialog.SetActive(true);
    }

    private void UpdateUI()
    {
        m_ScoreTextUI.text = $"{m_MaxQuestions - m_QuestionUsed}/{m_MaxQuestions}";
    }
}
