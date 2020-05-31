using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
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

    [Header("Footer")]
    public GameObject m_PilotFooter;
    public GameObject m_TeamFooter;

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
    private string m_PlayerSelected = "";

    private HashSet<string> m_WordSelectedList = new HashSet<string>();
    private int m_WordSelected = 0;

    private void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();

        var words = m_TextFile.text.Split('\n');
        m_Words = new List<string>(words);

        if (PhotonNetwork.IsMasterClient)
        {
            m_PhotonView.RPC("OnSelectPlayer", RpcTarget.AllBuffered);
            m_PhotonView.RPC("OnSelectWord", RpcTarget.AllBuffered);
        }
    }

    private void Update()
    {
        if (m_PlayerSelected != "")
        {
            ///Debug.Log($"Update {m_PhotonView.Owner.NickName} / {m_PlayerSelected}");
            m_ScoreTextUI.text = $"{m_MaxQuestions - m_QuestionUsed}/{m_MaxQuestions}";
            //m_WordTextUI.gameObject.SetActive(m_PhotonView.Owner.NickName == m_PlayerSelected);
            //m_PilotFooter.SetActive(m_PhotonView.Owner.NickName == m_PlayerSelected);
            //m_TeamFooter.SetActive(m_PhotonView.Owner.NickName != m_PlayerSelected);
        }
    }

    [PunRPC]
    private void OnSelectPlayer()
    {
        Debug.Log("OnSelectPlayer");
        var index = Random.Range(0, PhotonNetwork.CurrentRoom.Players.Count);
        var amount = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}");
            if (index == amount)
            {
                m_PlayerSelected = player.Value.NickName;
                Debug.Log($"Random {index}/{PhotonNetwork.CurrentRoom.Players.Count} = Pilot is {m_PlayerSelected}");
                break;
            }

            amount++;
        }

        m_TutorialDialog.SetActive(true);
    }

    [PunRPC]
    private void OnSelectWord()
    {
        Debug.Log("OnSelectWord");
        m_WordSelectedList.Clear();
        /*
        while (m_WordSelectedList.Count < m_NumberWordSelect)
        {
            var index = Random.Range(0, m_WordSelectedList.Count);
            m_WordSelectedList.Add(m_Words[index]);
            Debug.Log($"Word selected: {m_Words[index]}");
        }
        */
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
}
