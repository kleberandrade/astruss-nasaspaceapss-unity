using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LocalMicrophoneManager : MonoBehaviour
{
    public AudioSource localAudio;
    private IEnumerator Start()
    {
        if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);
        
        while(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            yield return null;
        }
        
        localAudio.clip = Microphone.Start(Microphone.devices[0], true, 10, 44100);
        localAudio.loop = true;
        while (!(Microphone.GetPosition(null) > 0))
        {
            yield return null;
        }
        localAudio.Play();
    }
}
