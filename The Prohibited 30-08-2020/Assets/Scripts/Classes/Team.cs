using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string Name;
    public int CurrentRound;//, SkipCount;
    public bool IsPlaying;
    public bool NinjaUsed, KingUsed;
    public bool InformerUsed, InformerRecieved;
    public bool ScholarshipUsed, ScholarshipRecieved;
    public bool OldmanUsed, OldmanRecieved;
    public bool JokerUsed, JokerRecieved;

    public List<int> Score;
    public List<Question> CurrentRoundQuestions;
    public List<SkippableQuestion> SkippedQuestions;
    public Question InformerQuestion;//sent to enemy

    public Question OwnedScholerShipQuestion, OwnedJokerQuestion;//recieved from enemy

    public List<List<AnswerdQuestion>> GameAnswerdQuestions;

    public Team() { }

    public Team(string name)
    {
        Name = name;
        Score = new List<int>();
        Score.Add(0);
        SkippedQuestions = new List<SkippableQuestion>();
        CurrentRoundQuestions = new List<Question>();

        GameAnswerdQuestions = new List<List<AnswerdQuestion>>();
    }

    public override string ToString()
    {
        return "Name = " + Name + "\n" +
            "CurrentRound = " + CurrentRound + "\n" +
            "Score Now = " + CalculateScore();
    }

    public int CalculateScore()
    {
        int Total = 0;
        foreach (int item in Score)
        {
            Total += item;
        }
        return Total;
    }
}
