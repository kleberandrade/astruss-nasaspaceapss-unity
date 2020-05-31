﻿using System;
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

    private string _channelName = "A";
    private string _appId = "705fb58352d04b4894149b862d278fb6";
    private IRtcEngine _mrRtcEngine = null;
    private uint _myId = 0;

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
        // _channelName = PlayerPrefs.GetString("channelName");
        JoinChannel();
    }

    private void JoinChannel()
    {
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
        var go = Instantiate(videoObject, grid);
        var playerVideo = go.GetComponent<PlayerVideo>();
        playerVideos.Add(_myId, playerVideo);
        playerVideo.Set(0);
    }
    
    private void OnUserJoined(uint uid, int elapsed)
    {
        if (uid == _myId || _myId == 0)
        {
            return;
        }

        _mrRtcEngine.EnableAudio();
        var go = Instantiate(videoObject, grid);
        var playerVideo = go.GetComponent<PlayerVideo>();
        playerVideos.Add(uid, playerVideo);
        playerVideo.Set(uid);
        playerVideo.gameObject.SetActive(true);
    }

    private void OnLeaveChannel(RtcStats stats)
    {
        _mrRtcEngine.DisableAudio();
        playerVideos[_myId].Clear();
        _mrRtcEngine.DisableVideoObserver();
    }

    private void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        _mrRtcEngine.DisableAudio();
        var playerVideo = playerVideos[uid];
        playerVideo.Clear();
        Destroy(playerVideo.gameObject);
    }

    private void OnApplicationQuit()
    {
            _mrRtcEngine.DisableAudio();
            _mrRtcEngine.DisableVideoObserver();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (_mrRtcEngine == null) return;

        if (hasFocus)
        {
            _mrRtcEngine.EnableVideoObserver();
            _mrRtcEngine.EnableAudio();
        }

        else
        {
            _mrRtcEngine.DisableAudio();
            _mrRtcEngine.DisableVideoObserver();
        }
    }
}
