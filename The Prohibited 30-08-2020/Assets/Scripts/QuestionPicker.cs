using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker
{
    public static Question Pick()
    {
        if (GlobalVariables.Inistance.AvailableQuestions.Count == 0)
            return null;

        int Length = GlobalVariables.Inistance.AvailableQuestions.Count;

        int R = Random.Range(0, Length);

        Question Choosed = GlobalVariables.Inistance.AvailableQuestions[R];

        GlobalVariables.Inistance.AvailableQuestions.Remove(Choosed);

        GlobalVariables.Inistance.AnsweredQuestions.Add(Choosed);

        return Choosed;

        //if (MoreThanTwo(Choosed))
        //{
        //    GlobalVariables.Inistance.AvailableQuestions.Remove(Choosed);

        //    GlobalVariables.Inistance.AnsweredQuestions.Add(Choosed);

        //    return Choosed;
        //}
        //else
        //    return Pick();
    }

    private static bool MoreThanTwo(Question Q)
    {
        if (Q.Word.Split().Length < 3)
            return false;
        else
            return true;
    }
}
