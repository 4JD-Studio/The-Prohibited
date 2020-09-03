using System;
using System.Collections.Generic;

[System.Serializable]
public class Category : IEquatable<Category>
{
    public string Name, LongName, Description, ImageWithText, ImageWithoutText, PurchaseLabel;
    public bool IsFree;

    public Category() { }

    public Category(string name, string longName, string description, string imageWithText, string imageWithoutText, bool isFree, string purchaseLabel)
    {
        Name = name;
        LongName = longName;
        ImageWithText = imageWithText;
        ImageWithoutText = imageWithoutText;
        IsFree = isFree;
        Description = description;
        PurchaseLabel = purchaseLabel;
    }

    public override string ToString()
    {
        return "Category = " + Name + "\n" +
            "LongName = " + LongName + "\n" +
            "Description = " + Description + "\n" +
            "ImageWithText = " + ImageWithText + "\n" +
            "ImageWithoutText = " + ImageWithoutText + "\n" +
            "IsFree = " + IsFree;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        Category C = obj as Category;
        if (C == null)
            return false;
        else
            return Equals(C);
    }

    public override int GetHashCode()
    {
        var hashCode = 753832385;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        return hashCode;
    }

    public bool Equals(Category C)
    {
        if (C == null)
            return false;
        return (Name.Equals(C.Name));
    }
}
