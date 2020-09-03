using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnswerState
{
    Correct,
    Wrong,
    Passed
}

public class AnswerdQuestion
{
    public Question question;
    public AnswerState answerState;

    public AnswerdQuestion() {}

    public AnswerdQuestion(Question Q, AnswerState AS)
    {
        question = Q;
        answerState = AS;
    }

    public override string ToString()
    {
        return question.ToString() + "   " + answerState.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        AnswerdQuestion Q = obj as AnswerdQuestion;
        if (Q == null)
            return false;
        else
            return Equals(Q);
    }

    public override int GetHashCode()
    {
        var hashCode = 2050590918;
        hashCode = hashCode * -1521134295 +
            EqualityComparer<string>.Default.GetHashCode(question.Word) +
            EqualityComparer<string>.Default.GetHashCode(answerState.ToString());

        return hashCode;
    }

    public bool Equals(AnswerdQuestion obj)
    {
        if (obj == null)
            return false;
        return (obj.question.Word.Equals(question.Word) && obj.answerState == answerState);
    }
}
