using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkippableQuestion
{
    public Question Q;
    public List<string> PannedWords;

    public SkippableQuestion() { }

    public SkippableQuestion(Question q, List<string> pannedWords)
    {
        Q = q;
        PannedWords = pannedWords;
    }

    public override string ToString()
    {
        return Q.ToString() + "   " + PannedWords.Count;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        SkippableQuestion Q = obj as SkippableQuestion;
        if (Q == null)
            return false;
        else
            return Equals(Q);
    }

    public override int GetHashCode()
    {
        var hashCode = 2050590918;
        hashCode = hashCode * -1521134295 +
            EqualityComparer<string>.Default.GetHashCode(Q.Word) +
            EqualityComparer<Category>.Default.GetHashCode(Q.Category);

        return hashCode;
    }

    public bool Equals(SkippableQuestion obj)
    {
        if (obj == null)
            return false;
        return (obj.Q.Word.Equals(Q.Word) && Q.Category.Name.Equals(obj.Q.Category.Name));
    }
}
