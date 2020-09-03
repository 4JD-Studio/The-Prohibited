using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Inistance;

    public AudioSource MusicSource, SFXSource, SFXClockSource, NarrationSource;
    public AudioClip ButtonClickAudio, CountDownAudio, TikTokAudio, MusicAudio, TrueAudio, FalseAudio, SkipAudio, OneTechAudio, TimeUpAudio, QuestionsFinishedAudio;
    public List<Button> ButtonsWithSound;

    private void Awake()
    {
        if (Inistance == null)
            Inistance = this;
    }
    
    void Start()
    {
        foreach (Button button in ButtonsWithSound)
        {
            AudioSource CurrentAudioSource = button.gameObject.AddComponent<AudioSource>();
            if (button.gameObject.tag.Equals("True"))
                button.onClick.AddListener(() => { OnButtonClick(TrueAudio); });
            else if (button.gameObject.tag.Equals("False"))
                button.onClick.AddListener(() => { OnButtonClick(FalseAudio); });
            else if (button.gameObject.tag.Equals("Skip"))
                button.onClick.AddListener(() => { OnButtonClick(SkipAudio); });
            else
                button.onClick.AddListener(() => { OnButtonClick(ButtonClickAudio); });
        }
    }

    public void ButtonSoundFromScript(Button button)
    {
        if(button.gameObject.tag.Equals("True"))
            button.onClick.AddListener(() => { OnButtonClick(TrueAudio); });
        else if (button.gameObject.tag.Equals("False"))
            button.onClick.AddListener(() => { OnButtonClick(FalseAudio); });
        else if (button.gameObject.tag.Equals("Skip"))
            button.onClick.AddListener(() => { OnButtonClick(SkipAudio); });
        else
            button.onClick.AddListener(() => { OnButtonClick(ButtonClickAudio); });
    }

    private void OnButtonClick(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.PlayOneShot(clip);
    }

    public void PlayCountDownSound()
    {
        SFXSource.clip = CountDownAudio;
        SFXSource.PlayOneShot(CountDownAudio);
    }

    public void PlayOneTechSound()
    {
        SFXSource.clip = OneTechAudio;
        SFXSource.PlayOneShot(OneTechAudio);
    }

    public void PlayTimeUpSound()
    {
        SFXSource.clip = TimeUpAudio;
        SFXSource.PlayOneShot(TimeUpAudio);
    }

    public void PlayQuestionsFinishedSound()
    {
        SFXSource.clip = QuestionsFinishedAudio;
        SFXSource.PlayOneShot(QuestionsFinishedAudio);
    }

    //used in global variables after getting recent values
    //in ui controller after slider value change
    public void SetMusicSourceLevel(float value)
    {
        MusicSource.volume = value;
    }

    public void SetSFXSourceLevel(float value)
    {
        SFXSource.volume = value;
        SFXClockSource.volume = value;
    }

    public void SetNarrationSourceLevel(float value)
    {
        NarrationSource.volume = value;
    }
}
