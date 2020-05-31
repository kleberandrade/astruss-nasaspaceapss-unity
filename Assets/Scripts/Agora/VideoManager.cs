using System;
using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public RectTransform grid;
    [SerializeField] private GameObject videoObject;
    public Button button;
    public Text buttonText;
    public Text textLog;
    
    private string _channelName = "a";
    private string _appId = "a68474b928f24df18adfa37e67e0d6cc";
    private IRtcEngine _mrRtcEngine = null;
    private uint _myId = 0;

    //public List<PlayerVideo> playerVideos;
    public Dictionary<uint, PlayerVideo> playerVideos = new Dictionary<uint, PlayerVideo>();
    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
        
        _mrRtcEngine = IRtcEngine.GetEngine(_appId);
        button.onClick.AddListener(JoinChannel);
    }

    private void JoinChannel()
    {
        button.onClick.RemoveListener(JoinChannel);
        button.onClick.AddListener(LeaveChannel);
        buttonText.text = "Leave";
        _mrRtcEngine.EnableVideoObserver();

        _mrRtcEngine.OnJoinChannelSuccess += OnJoinChannelSuccess;
        _mrRtcEngine.OnUserJoined += OnUserJoined;
        _mrRtcEngine.OnUserOffline += OnUserOffline;
        _mrRtcEngine.OnLeaveChannel += OnLeaveChannel;

        if (string.IsNullOrEmpty(_channelName))
            return;

        _mrRtcEngine.JoinChannel(_channelName, null, 0);
        
    }

    private void LeaveChannel()
    {
        button.onClick.RemoveListener(LeaveChannel);
        button.onClick.AddListener(JoinChannel);
        buttonText.text = "Join";

        playerVideos.Clear();


        _mrRtcEngine.LeaveChannel();
        _mrRtcEngine.OnJoinChannelSuccess -= OnJoinChannelSuccess;
        _mrRtcEngine.OnUserJoined -= OnUserJoined;
        _mrRtcEngine.OnUserOffline -= OnUserOffline;
        _mrRtcEngine.OnLeaveChannel -= OnLeaveChannel;
    }
    
    private void OnJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        _myId = uid;
        _mrRtcEngine.EnableVideo();
        _mrRtcEngine.EnableAudio();
        //NULL REFERENCE AQUI. PROVAVELMENTE ALGUM ERRO NO DICTIONARY.
        var go = Instantiate(videoObject, grid);
        var playerVideo = go.GetComponent<PlayerVideo>();
        playerVideos.Add(_myId, playerVideo);
        playerVideo.Set(0);
        textLog.color = Color.blue;
        textLog.text = $"{playerVideo.gameObject.name}";
    }
    
    private void OnUserJoined(uint uid, int elapsed)
    {
        if (uid == _myId || _myId == 0)
            return;

        //NULL REFERENCE AQUI. PROVAVELMENTE ALGUM ERRO NO DICTIONARY.
        var go = Instantiate(videoObject, grid);
        var playerVideo = go.GetComponent<PlayerVideo>();
        playerVideos.Add(uid, playerVideo);
        playerVideo.Set(uid);
        playerVideo.gameObject.SetActive(true);
        textLog.color = Color.red;
        textLog.text = $"{playerVideo.gameObject.name}";
    }

    private void OnLeaveChannel(RtcStats stats)
    {
        playerVideos[_myId].Clear();
        _mrRtcEngine.DisableVideoObserver();
    }

    private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        var playerVideo = playerVideos[uid];
        playerVideo.Clear();
        Destroy(playerVideo.gameObject);
    }

    private void OnApplicationQuit()
    {
        try
        {
            playerVideos[_myId].Clear();
            _mrRtcEngine.DisableVideoObserver();
        }
        catch
        {
            // ignored
        }
    }
}
