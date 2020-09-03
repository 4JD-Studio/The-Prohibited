using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Inistance;


    [Header("Database Files Names")]
    public string QuestionsFileName = "Questions";
    public string CategoriesFileName = "Categories";


    [Header("Common Variables")]
    public bool Premium = false;
    public bool ShowAds = true;
    [HideInInspector]
    public float MusicLevel, SfxLevel, NarrationLevel;
    [HideInInspector]
    public int RoundsCount, RoundDuration;
    

    [HideInInspector]
    public List<Question> AllQuestions, FreeQuestions, AvailableQuestions, AnsweredQuestions;
    [HideInInspector]
    public List<Category> AllCategories, FreeCategories, SelectedCategories;
    [HideInInspector]
    public int FreeStatCategoriesCount = 2;

    [HideInInspector]
    public Question CurrentQuestion;
    [HideInInspector]
    public List<string> CurrentPannedWords;
    [HideInInspector]
    public Team Team1, Team2;

    private void Awake()
    {
        if (Inistance == null)
        {
            Inistance = this;

            AllQuestions = new List<Question>();
            FreeQuestions = new List<Question>();
            AvailableQuestions = new List<Question>();
            AnsweredQuestions = new List<Question>();
            AllCategories = new List<Category>();
            FreeCategories = new List<Category>();
            SelectedCategories = new List<Category>();
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    private void GetPlayerPrefsData()
    {
        MusicLevel = PlayerPrefs.GetFloat("MusicLevel", 0.5f);
        SfxLevel = PlayerPrefs.GetFloat("SfxLevel", 0.5f);
        RoundsCount = PlayerPrefs.GetInt("RoundsCount", 4);
        RoundDuration = PlayerPrefs.GetInt("RoundDuration", 60);
        //NarrationLevel  = PlayerPrefs.GetFloat("NarrationLevel", 0.5f);

        UIController.Inistance.MusicSlider.value = MusicLevel;
        UIController.Inistance.SfxSlider.value = SfxLevel;
        UIController.Inistance.MiniMusicSlider.value = MusicLevel;
        UIController.Inistance.MiniSfxSlider.value = SfxLevel;
        UIController.Inistance.RoundsCountText.text = RoundsCount.ToString();
        UIController.Inistance.RoundDurationText.text = RoundDuration.ToString();
        //UIController.Inistance.MiniNarrationSlider.value = NarrationLevel;

        AudioController.Inistance.SetMusicSourceLevel(MusicLevel);
        AudioController.Inistance.SetSFXSourceLevel(SfxLevel);
        //AudioController.Inistance.SetNarrationSourceLevel(NarrationLevel);

        if (RoundsCount == 6)
            UIController.Inistance.RoundsCountButtonPlus.interactable = false;
        else if (RoundsCount == 4)
            UIController.Inistance.RoundsCountButtonMinus.interactable = false;

        if (RoundDuration == 60)
            UIController.Inistance.RoundDurationButtonPlus.interactable = false;
        else if (RoundDuration == 45)
            UIController.Inistance.RoundDurationButtonMinus.interactable = false;
    }

    void Start()
    {
        GetPlayerPrefsData();

        //DatabaseReader.ReadDatabase(QuestionsFileName, CategoriesFileName);

        //TextFileController.getOpenProducts();

        StartCoroutine(Init());

        ClearData();
    }

    public void ClearData()
    {
        AnsweredQuestions = new List<Question>();
        SelectedCategories = new List<Category>();
        CurrentQuestion = new Question();
        Team1 = new Team();
        Team2 = new Team();

        UIController.Inistance.Field1.text = null;
        UIController.Inistance.Field2.text = null;

        TimerController.Inistance.Active = false;
        TimerController.Inistance.Seconds = 0;

        //should be populated again after premuim
        if(AllCategories != null && AllCategories.Count > 0)
            CategoriesPopulator.Start();//depend

        UIController.Inistance.StartFaceAnimation();

        AudioController.Inistance.MusicSource.Play();

        UIController.Inistance.ResetScoreQuestionsHolderPanel();

        UIController.Inistance.HowToPlayPanel.GetComponent<Image>().sprite = UIController.Inistance.InstructionsImages[0];

        //ScorePopulator.Init(
        //    UIController.Inistance.ScoreTextPrefab,
        //    UIController.Inistance.ScoreTeam1Content,
        //    UIController.Inistance.ScoreTeam2Content);
    }

    public IEnumerator Init()
    {
        DatabaseReader.ReadDatabase(GlobalVariables.Inistance.QuestionsFileName, GlobalVariables.Inistance.CategoriesFileName);

        ProductsSaver.getOpenProducts();

        UIController.Inistance.ButtonMenuStart.interactable = true;

        CategoriesPopulator.Start();

        yield return null;
    }
}
