using UnityEngine;

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
            GameManager.Instance.ShowChooseWordDialog();
            HowToPlay.SetActive(false);
        }else{
            Ballon[(Index - 1)].SetActive(false);
            Ballon[Index].SetActive(true);
        }
    }
}
