using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    public GameObject[] Ballon;
    public GameObject HowToPlay;
    public GameObject TapControl;
    public int Index = 0;

    public void NextBalloon()
    {
        Index++;
        if(Ballon.Length <= Index){
            HowToPlay.SetActive(false);
            TapControl.SetActive(false);
        }else{
            Ballon[(Index - 1)].SetActive(false);
            Ballon[Index].SetActive(true);
        }
    }
}
