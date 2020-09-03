using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController Inistance;

    public bool Active;
    public int Seconds = 60;
    public Text Display;

    private void Awake()
    {
        if (Inistance == null)
            Inistance = this;

        Active = false;
        Seconds = 0;
    }

    IEnumerator Updating()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (Active && Seconds > 0)
        {
            Seconds--;
            Display.text = Seconds.ToString();
            //if(/*Seconds <= 5 && */Seconds > 0)
                //AudioController.Inistance.PlayCountDownSound();

            if (Seconds <= 0)
            {
                Active = false;
                UIController.Inistance.OnTimeFinished();
                AudioController.Inistance.SFXClockSource.Stop();
                AudioController.Inistance.PlayTimeUpSound();
            }
            else
                StartCoroutine(Updating());
        }
        else
        {
            Active = false;
        }
    }

    public void StartTimer()
    {
        Seconds = GlobalVariables.Inistance.RoundDuration;

        Active = true;

        Display.text = Seconds.ToString();

        AudioController.Inistance.SFXClockSource.clip = AudioController.Inistance.TikTokAudio;
        AudioController.Inistance.SFXClockSource.Play();

        StartCoroutine(Updating());
    }

    public void PauseTimer()
    {
        Active = false;
        AudioController.Inistance.SFXClockSource.Pause();
        //Seconds++;
        //Display.text = Seconds.ToString();
    }

    public void ResumeTimer()
    {
        Active = true;
        AudioController.Inistance.SFXClockSource.Play();
        StartCoroutine(Updating());
    }

    public void IncreaseTime(int Amount = 10)
    {
        Seconds += Amount;
        Display.text = Seconds.ToString();
    }

    public void DecreaseTime(int Amount = 10)
    {
        Seconds -= Amount;
        Display.text = Seconds.ToString();

        if (Seconds <= 0)
        {
            Seconds = 0;
            Display.text = Seconds.ToString();
            Active = false;
            UIController.Inistance.OnTimeFinished();
        }
    }
}
