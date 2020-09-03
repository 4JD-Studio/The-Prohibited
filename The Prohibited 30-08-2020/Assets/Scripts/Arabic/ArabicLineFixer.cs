using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArabicSupport;
using System.Linq;

[RequireComponent(typeof(Text))]
[ExecuteInEditMode]
public class ArabicLineFixer : MonoBehaviour
{
    [TextArea]
    public string ArabicText;

    public void SetArabicText(string text)
    {
        this.ArabicText = text;
        StartCoroutine(FixLineOrderCoroutine());
    }

    private void OnEnable()
    {
        StartCoroutine(FixLineOrderCoroutine());
    }

    //void OnValidate()
    //{
    //    StartCoroutine(FixLineOrderCoroutine());
    //}

    IEnumerator FixLineOrderCoroutine()
    {
        Text textComponent = this.GetComponent<Text>();
        List<string> resultText = new List<string>();
        RectTransform rt = textComponent.GetComponent<RectTransform>();
        List<string> paragraphList = ArabicText.Split('\n').ToList();

        foreach (string paragraph in paragraphList)
        {
            textComponent.text = ArabicFixer.Fix(paragraph, false, false);
            TextGenerationSettings tgs = textComponent.GetGenerationSettings(rt.rect.size);

            if (textComponent.text.IndexOf(' ') < 0)
            {
                resultText.Add(textComponent.text);

            }
            else
            {
                List<string> lineList = new List<string>();
                List<string> wordList = textComponent.text.Split(' ').ToList();
                string singleLine = "";

                while (wordList.Count > 0)
                {
                    string singleWord = wordList[wordList.Count - 1];
                    wordList.RemoveAt(wordList.Count - 1);

                    if (textComponent.cachedTextGenerator.GetPreferredWidth(singleWord + ' ' + singleLine, tgs) > rt.rect.width)
                    {
                        lineList.Add(singleLine);
                        singleLine = singleWord;
                    }
                    else
                    {
                        singleLine = (singleLine != "") ? singleWord + ' ' + singleLine : singleWord;
                    }
                }

                if (singleLine.Length > 0)
                    lineList.Add(singleLine);

                resultText.Add(String.Join(Environment.NewLine, lineList.ToArray()));
            }

            if (!Application.isEditor)
                yield return new WaitForEndOfFrame();
        }

        textComponent.text = String.Join(Environment.NewLine, resultText.ToArray());
    }
}