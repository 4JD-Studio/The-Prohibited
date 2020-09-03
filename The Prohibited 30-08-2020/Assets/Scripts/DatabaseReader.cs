using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatabaseReader
{

    public static void ReadDatabase(string QuestionsFileName, string CategoriesFileName)
    {

        ReadCategories(CategoriesFileName);
        ReadQuestions(QuestionsFileName);
    }

    private static void ReadCategories(string CategoriesFileName)
    {

        //using (StreamReader reader = new StreamReader("Assets/Exel/" + CategoriesFileName + ".csv"))
        string Line;
        string[] Columns;
        Category C;

        using (StreamReader reader = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Database/" + CategoriesFileName).bytes)))
        {
            //header
            reader.ReadLine().Split(',');
            while ((Line = reader.ReadLine()) != null)
            {
                Columns = Line.Split(',');

                C = new Category(Columns[0], Columns[1], Columns[2], Columns[3], Columns[4], Columns[5].Equals("Y"), Columns[6]);

                C = FixIosConflect(C);

                GlobalVariables.Inistance.AllCategories.Add(C);

                if (C.IsFree)
                    GlobalVariables.Inistance.FreeCategories.Add(C);
            }
        }
    }

    private static void ReadQuestions(string QuestionsFileName)
    {
        //using (StreamReader reader = new StreamReader("Assets/Exel/" + QuestionsFileName + ".csv"))
        string Line;
        string[] Columns;
        string[] Panned;
        Question W;

        using (StreamReader reader = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Database/" + QuestionsFileName).bytes)))
        {
            //header
            reader.ReadLine().Split(',');
            while ((Line = reader.ReadLine()) != null)
            {
                Columns = Line.Split(',');

                Panned = new string[] { Columns[1], Columns[2], Columns[3], Columns[4], Columns[5], Columns[6] };

                W = new Question(Columns[0], Panned, FindCategory(Columns[7]));

                GlobalVariables.Inistance.AllQuestions.Add(W);

                if (W.Category.IsFree)
                    GlobalVariables.Inistance.FreeQuestions.Add(W);
            }
        }
    }

    private static Category FindCategory(string CategoryName)
    {
        return GlobalVariables.Inistance.AllCategories.Find(C => C.Name.Equals(CategoryName));
    }

    //public static bool ValidateQuestion(Question question)
    //{
    //    bool valid = false;
    //    using (StreamReader reader = new StreamReader(new MemoryStream(Resources.Load<TextAsset>("Database/" + GlobalVariables.Inistance.QuestionsFileName).bytes)))
    //    {
    //        reader.ReadLine();
    //        string Line;
    //        while ((Line = reader.ReadLine()) != null)
    //        {
    //            if(Line.Contains(question.Word) && Line.Contains(question.Word) && Line.Contains(question.Word) && Line.Contains(question.Word))
    //            {
    //                valid =  true;
    //                break;
    //            }
    //        }
    //    }
    //    return valid;
    //}

    private static Category FixIosConflect(Category category)
    {
        //required for ios product_travel confilct
        if (!AdController.Inistance.isTargetGoolePlayStore && category.PurchaseLabel.Equals("product_travel"))
            category.PurchaseLabel = "product_travel_second";
        return category;
    }
}
