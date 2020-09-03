using System;
using System.Collections.Generic;

[System.Serializable]
public class Question : IEquatable<Question>
{
    public string Word;
    public string[] PannedWords = new string[6];
    public Category Category;

    public Question() { }

    public Question(string word, string[] pannedWords, Category category)
    {
        Word = word;
        PannedWords = pannedWords;
        Category = category;
    }

    public override string ToString()
    {
        return "Word = " + Word + "\n" +
            "Panned 1 = " + PannedWords[0] + "\n" +
            "Panned 2 = " + PannedWords[1] + "\n" +
            "Panned 3 = " + PannedWords[2] + "\n" +
            "Panned 4 = " + PannedWords[3] + "\n" +
            "Panned 5 = " + PannedWords[4] + "\n" +
            "Panned 6 = " + PannedWords[5] + "\n" +
            "Category = " + Category.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Question Q = obj as Question;
        if (Q == null)
            return false;
        else
            return Equals(Q);
    }

    public override int GetHashCode()
    {
        var hashCode = 2050590918;
        hashCode = hashCode * -1521134295 + 
            EqualityComparer<string>.Default.GetHashCode(Word) + 
            EqualityComparer<Category>.Default.GetHashCode(Category);

        return hashCode;
    }

    public bool Equals(Question Q)
    {
        if (Q == null)
            return false;
        return (Word.Equals(Q.Word) && PannedWords[0].Equals(Q.PannedWords[0]) && Category.Name.Equals(Q.Category.Name));
    }
}
