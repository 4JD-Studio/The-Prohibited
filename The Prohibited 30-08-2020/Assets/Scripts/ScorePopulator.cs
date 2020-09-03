using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopulator : MonoBehaviour
{
    static GameObject ContentTeam1, ContentTeam2, TextPrefab;

    public static void Init(GameObject textPrefab, GameObject contentTeam1, GameObject contentTeam2)
    {
        TextPrefab = textPrefab;
        ContentTeam1 = contentTeam1;
        ContentTeam2 = contentTeam2;


        if (ContentTeam1.transform.childCount > 0)
        {
            foreach (Transform child in ContentTeam1.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (ContentTeam2.transform.childCount > 0)
        {
            foreach (Transform child in ContentTeam2.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public static void Populate(bool isTeam1End, int currentScore)
    {
        if (isTeam1End)
        {
            float CellWidth = ContentTeam1.GetComponent<RectTransform>().rect.width;
            float ParentHeight = ContentTeam1.transform.parent.GetComponent<RectTransform>().rect.height;
            float TitleHeight = ContentTeam1.transform.parent.GetChild(0).GetComponent<RectTransform>().rect.height;
            float CellHeight = (ParentHeight - TitleHeight) / GlobalVariables.Inistance.RoundsCount;


            ContentTeam1.GetComponent<GridLayoutGroup>().cellSize = new Vector2(CellWidth, CellHeight);
            GameObject Team1Text = Instantiate(TextPrefab, ContentTeam1.transform);
            Team1Text.transform.GetComponent<Text>().text = currentScore.ToString();

            ContentTeam2.GetComponent<GridLayoutGroup>().cellSize = new Vector2(CellWidth, CellHeight);
            GameObject Team2Text = Instantiate(TextPrefab, ContentTeam2.transform);
            Team2Text.transform.GetComponent<Text>().text = "0";
        }
        else
        {
            Transform Team2Text = ContentTeam2.transform.GetChild(ContentTeam2.transform.childCount - 1);
            Team2Text.GetComponent<Text>().text = currentScore.ToString();
        }
    }
}
