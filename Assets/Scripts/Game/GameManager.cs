using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null) { 
            Instance = this; 
        }

        m_CarManager = GetComponent<CardManager>();
        m_PhotonView = GetComponent<PhotonView>();
    }

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
    public GameObject m_TeamGameoverDialog;
    public GameObject m_PilotGameoverDialog;

    private PhotonView m_PhotonView;
    private int m_QuestionUsed = 0;
    private string m_PlayerSelected = "";

    private HashSet<string> m_WordSelectedList = new HashSet<string>();
    private string m_WordSelected = "";
    private CardManager m_CarManager;

    private void Start()
    {
        var words = m_TextFile.text.Split('\n');
        m_Words = new List<string>(words);
        RandomPlayerSelect();
    }

    private void RandomPlayerSelect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("OnSelectPlayer");
            var index = Random.Range(0, PhotonNetwork.CurrentRoom.Players.Count);
            var amount = 0;
            string selectedPlayer = "";
            foreach (var player in PhotonNetwork.CurrentRoom.Players)
            {
                Debug.Log($"{player.Value.NickName}");
                if (index == amount)
                {
                    selectedPlayer = player.Value.NickName;
                    Debug.Log($"Random {index}/{PhotonNetwork.CurrentRoom.Players.Count} = Pilot is {m_PlayerSelected}");
                    break;
                }

                amount++;
            }

            m_PhotonView.RPC("SetPlayerSelected", RpcTarget.AllBuffered, selectedPlayer as object);
            m_PhotonView.RPC("OnSelectWord", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void SetPlayerSelected(object seletedPlayer)
    {
        m_PlayerSelected = (string)seletedPlayer;
    }

    private void Update()
    {
        m_ScoreTextUI.text = $"{m_MaxQuestions - m_QuestionUsed}/{m_MaxQuestions}";
        m_WordTextUI.text = m_WordSelected;
        m_WordTextUI.gameObject.SetActive(PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected));
        m_PilotFooter.SetActive(PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected));
        m_TeamFooter.SetActive(!PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected));
    }

    public void ShowChooseWordDialog()
    {
        if (PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected))
        {
            Debug.Log("ShowChooseWordDialog");
            Debug.Log($"ShowChooseWordDialog: {PhotonNetwork.LocalPlayer.NickName}/{m_PlayerSelected}");
            m_SelectWordDialog.SetActive(true);
        }
    }

    public void SelectWord(string word)
    {
        if (PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected))
        {
            Debug.Log("SelectWord");
            Debug.Log($"SelectWord: {PhotonNetwork.LocalPlayer.NickName}/{m_PlayerSelected}");
            if (PhotonNetwork.LocalPlayer.NickName.Equals(m_PlayerSelected))
            {
                m_WordSelected = word;
                m_SelectWordDialog.SetActive(false);
            }
        }
    }

    [PunRPC]
    private void OnSelectWord()
    {
        Debug.Log("OnSelectWord");
        m_WordSelectedList.Clear();
        while (m_WordSelectedList.Count < m_NumberWordSelect)
        {
            var index = Random.Range(0, m_Words.Count);
            m_WordSelectedList.Add(m_Words[index]);
            Debug.Log($"Word selected: {m_Words[index]}");
        }

        m_CarManager.SetWords(m_WordSelectedList.ToList());

        m_TutorialDialog.SetActive(true);
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
            m_PilotGameoverDialog.SetActive(true);
    }

    public void RightAnswer()
    {
        m_PhotonView.RPC("OnRightAsked", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void OnRightAsked()
    {
        m_TeamGameoverDialog.SetActive(true);
    }

    public void TryAgain()
    {
        RandomPlayerSelect();
    }
}
