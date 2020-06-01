using System;
using System.Collections;
using System.Collections.Generic;
using agora_gaming_rtc;
using UnityEngine;

public class PlayerVideo : MonoBehaviour
{
    public VideoSurface videoSurface;

    public void Set(uint uid)
    {
        videoSurface.gameObject.SetActive(true);
        videoSurface.SetForUser(uid);
        videoSurface.SetGameFps(60);
        videoSurface.SetEnable(true);
    }

    public void Clear()
    {
        videoSurface.SetEnable(false);
        videoSurface.gameObject.SetActive(false);
    }
}
