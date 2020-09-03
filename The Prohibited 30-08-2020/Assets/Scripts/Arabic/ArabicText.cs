using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

public class ArabicText : MonoBehaviour
{
    public string Text;
    public bool Tashkeel = true;
    public bool HinduNumbers = true;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Text>().text = ArabicFixer.Fix(Text, Tashkeel, HinduNumbers);
    }
}
