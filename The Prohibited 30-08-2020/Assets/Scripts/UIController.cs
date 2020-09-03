using ArabicSupport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{

    public static UIController Inistance;

    [Header("Splash Screen")]
    public GameObject SplashScreenPanel;

    [Header("Menu")]
    public GameObject MainPanel;
    public Animator StartFaceAnimator;
    public Button ButtonMenuStart;

    [Header("Notifications")]
    public GameObject MessagePanel;

    [Header("How To Play")]
    public GameObject HowToPlayPanel;
    public List<Sprite> InstructionsImages;
    private int LastInstructionIndex = 0;

    [Header("Settings")]
    public Slider MusicSlider;
    public Slider SfxSlider;
    public Slider MiniSfxSlider, MiniMusicSlider, MiniNarrationSlider;
    public Text RoundsCountText, RoundDurationText;
    public Button RoundsCountButtonPlus, RoundsCountButtonMinus, RoundDurationButtonPlus, RoundDurationButtonMinus;

    [Header("Categories")]
    public GameObject CategoriesPanel;
    public GameObject MainContent, CategoriesContentPrefab, CategoriesHeaderPrefab, CategoryObjectPrefab, CategoriesFooterPrefab;
    public GameObject SuggestPremuimPanel, CategoryInfoPanel;
    public Sprite LockImage, SelectedImage;
    

    [Header("Names")]
    public GameObject NamesPanel;
    public InputField Field1, Field2;

    [Header("PreGame")]
    public GameObject PreGamePanel;
    public GameObject CountDownPanel;
    public Text /*CurrentTeamLabel, */CurrentTeamName, RoundsCount;
    public ArabicLineFixer TextHandPhone;
    public Sprite PregameTeam1Background, PregameTeam2Background;

    [Header("Game")]
    public GameObject GamePanel;
    public Image GameCategoryImage;
    public Text GameCategotyNameText, GameTeamNameText;
    public Text GameWordName;
    public List<Text> GameWordDetailsTextList = new List<Text>(5);
    public Button GameTrueButton, GameFalseButton, GameSkipButton, GameNinjaButton, GameKingButton, GameInformerButton;
    public Sprite SchollershipImageInfo, InformerImageInfo, JokerImageInfo;
    public Animator GameKingAnimator, GameNinjaAnimator, OldManAnimation;
    [HideInInspector]
    bool SkipPaused = false;
    IEnumerator CO;
    List<string> CurrentPannedWordsList;

    [Header("Game Help Buttons")]
    public GameObject PannelPannedWords;
    public GameObject ImageHelpIcon;
    public GameObject PanelHelpDetails;
    public Sprite SpriteScholership, SpriteJoker;

    [Header("Score")]
    public GameObject PanelScore;
    public ArabicLineFixer ScoreTeam1NameText, ScoreTeam2NameText;
    public Text ScoreCurrentRoundText;
    //public GameObject ScoreTeam1Content, ScoreTeam2Content, ScoreTextPrefab;
    public Text ScoreTeam1Score, ScoreTeam2Score;
    public GameObject ScoreTeam1QuestionsHolder, ScoreTeam2QuestionsHolder;
    public Sprite ScoreSpriteCorrect, ScoreSpriteWrong, ScoreSpritePass;
    public Button ScoreScholershipButton, ScoreOldmanButton, ScoreJokerButton;

    [Header("Winning")]
    public GameObject WinningPannel;
    public ArabicLineFixer WinningTeam1Name, WinningTeam2Name, WinningWinningName;
    public Text WinningTeam1Score, WinningTeam2Score;
    [HideInInspector]
    public bool AdDisplayed = false;
    public List<Category> TempSelectedCategoriesList;
    public string TempTeam1Name, TempTeam2Name;

    private void Awake()
    {
        if (Inistance == null)
            Inistance = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    private IEnumerator IEShowMessage(string Message, float Seconds)
    {
        MessagePanel.SetActive(true);
        MessagePanel.transform.GetChild(0).GetComponent<ArabicLineFixer>().SetArabicText(Message);
        yield return new WaitForSecondsRealtime(Seconds);
        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Message, float Seconds = 3f)
    {
        StartCoroutine(IEShowMessage(Message, Seconds));
    }

    //Splash Screen
    private void Start()
    {
        StartCoroutine(SplashCoroutine());
    }

    public IEnumerator SplashCoroutine()
    {
        //PanelSplash.SetActive(true);
        //yield return new WaitForSecondsRealtime(4.2f);
        //PanelSplash.GetComponent<Animator>().SetTrigger("Fade");
        //yield return new WaitForSecondsRealtime(1f);
        //PanelSplash.SetActive(false);
        //PanelMainMenu.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        SplashScreenPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    //Menu
    public void StartFaceAnimation()
    {
        StartFaceAnimator.SetTrigger("GO");
    }

    //How To Play
    public void OnHowToPlayNextClick(bool isNext)
    {
        if (isNext)
        {
            LastInstructionIndex++;
            if (LastInstructionIndex >= InstructionsImages.Count)
                LastInstructionIndex = 0;
        }
        else
        {
            LastInstructionIndex--;
            if (LastInstructionIndex < 0)
                LastInstructionIndex = InstructionsImages.Count - 1;
        }

        HowToPlayPanel.GetComponent<Image>().sprite = InstructionsImages[LastInstructionIndex];
    }

    //Settings
    public void OnSettingsMusicSliderValueChange(bool IsMainSettings)
    {
        if (IsMainSettings)
        {
            GlobalVariables.Inistance.MusicLevel = MusicSlider.value;
            MiniMusicSlider.value = MusicSlider.value;
        }
        else
        {
            GlobalVariables.Inistance.MusicLevel = MiniMusicSlider.value;
            MusicSlider.value = MiniMusicSlider.value;
        }
        PlayerPrefs.SetFloat("MusicLevel", GlobalVariables.Inistance.MusicLevel);

        AudioController.Inistance.SetMusicSourceLevel(GlobalVariables.Inistance.MusicLevel);
    }

    public void OnSettingsSfxSliderValueChange(bool IsMainSettings)
    {
        if (IsMainSettings)
        {
            GlobalVariables.Inistance.SfxLevel = SfxSlider.value;
            MiniSfxSlider.value = SfxSlider.value;
        }
        else
        {
            GlobalVariables.Inistance.SfxLevel = MiniSfxSlider.value;
            SfxSlider.value = MiniSfxSlider.value;
        }
        PlayerPrefs.SetFloat("SfxLevel", GlobalVariables.Inistance.SfxLevel);

        AudioController.Inistance.SetSFXSourceLevel(GlobalVariables.Inistance.SfxLevel);
    }

    public void OnMiniSettingsNarrationSliderValueChange()
    {
        GlobalVariables.Inistance.NarrationLevel = MiniNarrationSlider.value;
        PlayerPrefs.SetFloat("NarrationLevel", GlobalVariables.Inistance.NarrationLevel);

        AudioController.Inistance.SetNarrationSourceLevel(GlobalVariables.Inistance.NarrationLevel);
    }

    public void OnSettingsRoundsCountButtonClick(bool add)
    {
        RoundsCountButtonPlus.interactable = true;
        RoundsCountButtonMinus.interactable = true;

        if (add)
        {
            GlobalVariables.Inistance.RoundsCount++;

            if (GlobalVariables.Inistance.RoundsCount == 6)
                RoundsCountButtonPlus.interactable = false;
        }
        else
        {
            GlobalVariables.Inistance.RoundsCount--;

            if (GlobalVariables.Inistance.RoundsCount == 4)
                RoundsCountButtonMinus.interactable = false;
        }

        RoundsCountText.text = GlobalVariables.Inistance.RoundsCount.ToString();
        PlayerPrefs.SetInt("RoundsCount", GlobalVariables.Inistance.RoundsCount);
    }

    public void OnSettingsRoundDurationButtonClick(bool add)
    {
        RoundDurationButtonPlus.interactable = true;
        RoundDurationButtonMinus.interactable = true;

        if (add)
        {
            GlobalVariables.Inistance.RoundDuration++;

            if (GlobalVariables.Inistance.RoundDuration == 60)
                RoundDurationButtonPlus.interactable = false;
        }
        else
        {
            GlobalVariables.Inistance.RoundDuration--;

            if(GlobalVariables.Inistance.RoundDuration == 45)
                RoundDurationButtonMinus.interactable = false;
        }

        RoundDurationText.text = GlobalVariables.Inistance.RoundDuration.ToString();
        PlayerPrefs.SetInt("RoundDuration", GlobalVariables.Inistance.RoundDuration);
    }

    //Categories
    public void OnCategoriesNextButtonClick()
    {
        if(GlobalVariables.Inistance.SelectedCategories.Count <= 0)
            ShowMessage("يجب اختيار مجموعة واحدة" + "\n" + " على الأقل");
        else
        {
            NamesPanel.SetActive(true);
            PrepareAvailableQuestions();
        }
    }

    public void PrepareAvailableQuestions()
    {
        GlobalVariables.Inistance.AvailableQuestions = new List<Question>();

        List<Question> Temp;
        TempSelectedCategoriesList = new List<Category>();
        foreach (Category category in GlobalVariables.Inistance.SelectedCategories)
        {
            TempSelectedCategoriesList.Add(category);

            Temp = new List<Question>();
            Temp = GlobalVariables.Inistance.AllQuestions.FindAll(Q => Q.Category.Equals(category));
            GlobalVariables.Inistance.AvailableQuestions.AddRange(Temp);
        }

        GlobalVariables.Inistance.SelectedCategories = new List<Category>();
    }

    //Names
    public void OnNamesFieldsValueChange(GameObject F)
    {
        F.transform.GetChild(F.transform.childCount - 1).GetComponent<Text>().text =
            ArabicFixer.Fix(
                F.GetComponent<InputField>().text);
    }

    public void OnNamesButtonStartClick()
    {
        if(Field1.text == null || Field1.text.Length == 0)
            ShowMessage("يجب إدخال الفريق الأول");
        else if (Field2.text == null || Field2.text.Length == 0)
            ShowMessage("يجب إدخال الفريق الثاني");
        else if(Field1.text.Equals(Field2.text))
            ShowMessage("يجب أن يكون الاسمان مختلفان");
        else
        {
            GlobalVariables.Inistance.Team1 = new Team(Field1.text);
            GlobalVariables.Inistance.Team2 = new Team(Field2.text);

            //reset attack buttons
            ScoreScholershipButton.interactable = true;
            ScoreOldmanButton.interactable = true;
            ScoreJokerButton.interactable = true;

            SetDateOnPreGamePanel(true);
        }
    }

    //PreGame
    public void SetDateOnPreGamePanel(bool FirstTeam)
    {
        if ((FirstTeam && GlobalVariables.Inistance.Team1.CurrentRound + 1 <= GlobalVariables.Inistance.RoundsCount) ||
            !FirstTeam && GlobalVariables.Inistance.Team2.CurrentRound + 1 <= GlobalVariables.Inistance.RoundsCount)
        {
            PreGamePanel.SetActive(true);
            //RoundsCount.text = GlobalVariables.Inistance.RoundsCount.ToString();

            if (FirstTeam)
            {

                //clear only for first team so it will visible for the second team
                ResetScoreQuestionsHolderPanel();

                RoundsCount.text = (++GlobalVariables.Inistance.Team1.CurrentRound).ToString();

                //CurrentTeamLabel.text = ArabicFixer.Fix("الفريق الأول");
                CurrentTeamName.text = ArabicFixer.Fix(GlobalVariables.Inistance.Team1.Name);
                TextHandPhone.SetArabicText("أعطي الجوال لفريق");// + "\n" + GlobalVariables.Inistance.Team1.Name);
                GlobalVariables.Inistance.Team1.IsPlaying = true;
                GlobalVariables.Inistance.Team2.IsPlaying = false;
   
                PreGamePanel.GetComponent<Image>().sprite = PregameTeam1Background;

                GlobalVariables.Inistance.Team1.Score.Add(0);

                GlobalVariables.Inistance.Team1.GameAnswerdQuestions.Add(new List<AnswerdQuestion>());
            }
            else
            {
                RoundsCount.text = (++GlobalVariables.Inistance.Team2.CurrentRound).ToString();

                //CurrentTeamLabel.text = ArabicFixer.Fix("الفريق الثاني");
                CurrentTeamName.text = ArabicFixer.Fix(GlobalVariables.Inistance.Team2.Name);
                TextHandPhone.SetArabicText("أعطي الجوال لفريق");// + "\n" + GlobalVariables.Inistance.Team2.Name);
                GlobalVariables.Inistance.Team1.IsPlaying = false;
                GlobalVariables.Inistance.Team2.IsPlaying = true;

                PreGamePanel.GetComponent<Image>().sprite = PregameTeam2Background;

                GlobalVariables.Inistance.Team2.Score.Add(0);

                GlobalVariables.Inistance.Team2.GameAnswerdQuestions.Add(new List<AnswerdQuestion>());
            }
        }
        else
            OnGameOver();
    }

    public void OnPreGamePanelClick()
    {
        StartCoroutine(StartPreGameCountDown());
    }

    public IEnumerator StartPreGameCountDown()
    {
        AudioController.Inistance.MusicSource.Stop();

        CountDownPanel.SetActive(true);

        CountDownPanel.transform.GetChild(0).GetComponent<Text>().text = "3";
        CountDownPanel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Go");
        AudioController.Inistance.PlayOneTechSound();
        yield return new WaitForSecondsRealtime(1f);

        CountDownPanel.transform.GetChild(0).GetComponent<Text>().text = "2";
        CountDownPanel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Go");
        AudioController.Inistance.PlayOneTechSound();
        yield return new WaitForSecondsRealtime(1f);

        CountDownPanel.transform.GetChild(0).GetComponent<Text>().text = "1";
        CountDownPanel.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Go");
        AudioController.Inistance.PlayOneTechSound();
        yield return new WaitForSecondsRealtime(1f);

        CountDownPanel.SetActive(false);

        AfterCountDown();
    }

    private void AfterCountDown()
    {
        CategoriesPanel.SetActive(false);
        NamesPanel.SetActive(false);
        PreGamePanel.SetActive(false);
        GamePanel.SetActive(true);

        //set Buttons
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            //GlobalVariables.Inistance.Team1.SkipCount = 0;
            GlobalVariables.Inistance.Team1.SkippedQuestions.Clear();
            GlobalVariables.Inistance.Team1.CurrentRoundQuestions = new List<Question>();
            GameSkipButton.interactable = true;

            if (GlobalVariables.Inistance.Team1.NinjaUsed)
                GameNinjaButton.interactable = false;
            else
                GameNinjaButton.interactable = true;

            if (GlobalVariables.Inistance.Team1.KingUsed)
                GameKingButton.interactable = false;
            else
                GameKingButton.interactable = true;

            if (GlobalVariables.Inistance.Team1.InformerUsed)
                GameInformerButton.interactable = false;
            else
                GameInformerButton.interactable = true;
        }

        if (GlobalVariables.Inistance.Team2.IsPlaying)
        {
            //GlobalVariables.Inistance.Team2.SkipCount = 0;
            GlobalVariables.Inistance.Team2.SkippedQuestions.Clear();
            GlobalVariables.Inistance.Team2.CurrentRoundQuestions = new List<Question>();
            GameSkipButton.interactable = true;

            if (GlobalVariables.Inistance.Team2.NinjaUsed)
                GameNinjaButton.interactable = false;
            else
                GameNinjaButton.interactable = true;

            if (GlobalVariables.Inistance.Team2.KingUsed)
                GameKingButton.interactable = false;
            else
                GameKingButton.interactable = true;

            if (GlobalVariables.Inistance.Team2.InformerUsed)
                GameInformerButton.interactable = false;
            else
                GameInformerButton.interactable = true;
        }


        GetNewQuestion();

        TimerController.Inistance.StartTimer();

        if (GlobalVariables.Inistance.Team1.IsPlaying &&
            GlobalVariables.Inistance.Team2.OldmanUsed &&
            !GlobalVariables.Inistance.Team1.OldmanRecieved)
        {
            GlobalVariables.Inistance.Team1.OldmanRecieved = true;
            StartCoroutine(OldManAnimationPlayer());
        }
        else if (GlobalVariables.Inistance.Team2.IsPlaying &&
            GlobalVariables.Inistance.Team1.OldmanUsed &&
            !GlobalVariables.Inistance.Team2.OldmanRecieved)
        {
            GlobalVariables.Inistance.Team2.OldmanRecieved = true;
            StartCoroutine(OldManAnimationPlayer());
        }
    }

    private IEnumerator OldManAnimationPlayer()
    {
        OldManAnimation.gameObject.SetActive(true);
        OldManAnimation.SetTrigger("GO");
        yield return new WaitForSecondsRealtime(3f);
        OldManAnimation.gameObject.SetActive(false);
        TimerController.Inistance.DecreaseTime();
    }

    //Game
    public void OnGamePauseClick()
    {
        TimerController.Inistance.PauseTimer();
    }

    public void OnGameResumeClick()
    {
        TimerController.Inistance.ResumeTimer();
    }
    
    public void GetNewQuestion()
    {

        int roundCorrectQuestionsCount = 0;
        if (GlobalVariables.Inistance.Team1.IsPlaying && GlobalVariables.Inistance.Team1.CurrentRoundQuestions.Count >= 6)
        {
            foreach (AnswerdQuestion question in GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1])
            {
                if (question.answerState != AnswerState.Wrong)
                    roundCorrectQuestionsCount++;
            }
            if(roundCorrectQuestionsCount >= 3)
                AudioController.Inistance.PlayQuestionsFinishedSound();
            else
                AudioController.Inistance.PlayTimeUpSound();
            SetScorePanel();
        }
        else if (GlobalVariables.Inistance.Team2.IsPlaying && GlobalVariables.Inistance.Team2.CurrentRoundQuestions.Count >= 6)
        {
            foreach (AnswerdQuestion question in GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1])
            {
                if (question.answerState != AnswerState.Wrong)
                    roundCorrectQuestionsCount++;
            }
            if (roundCorrectQuestionsCount >= 3)
                AudioController.Inistance.PlayQuestionsFinishedSound();
            else
                AudioController.Inistance.PlayTimeUpSound();
            SetScorePanel();
        }
        else
        {
            //to fix ninja effect
            for (int i = 0; i < GameWordDetailsTextList.Count; i++)
            {
                GameWordDetailsTextList[i].GetComponent<Animator>().enabled = false;
                GameWordDetailsTextList[i].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            }


            SkipPaused = false;
            if(CO != null)
            {
                StopCoroutine(CO);
                GameSkipButton.interactable = true;
            }

            if (CheckHelpQuestions())
                return;

            //stop skip in loast question
            if (GlobalVariables.Inistance.Team1.IsPlaying && GlobalVariables.Inistance.Team1.CurrentRoundQuestions.Count >= 5)
                GameSkipButton.interactable = false;
            else if (GlobalVariables.Inistance.Team2.IsPlaying && GlobalVariables.Inistance.Team2.CurrentRoundQuestions.Count >= 5)
                GameSkipButton.interactable = false;


            if (GlobalVariables.Inistance.Team1.IsPlaying &&
                (GlobalVariables.Inistance.Team1.SkippedQuestions.Count + GlobalVariables.Inistance.Team1.CurrentRoundQuestions.Count >= 6))
            {
                GlobalVariables.Inistance.CurrentQuestion = GlobalVariables.Inistance.Team1.SkippedQuestions[0].Q;
                GlobalVariables.Inistance.CurrentPannedWords = GlobalVariables.Inistance.Team1.SkippedQuestions[0].PannedWords;
                GlobalVariables.Inistance.Team1.SkippedQuestions.RemoveAt(0);

                //if (GlobalVariables.Inistance.Team1.SkippedQuestions.Count == 0)
                //    GameSkipButton.interactable = false;

                if (GlobalVariables.Inistance.Team1.InformerRecieved &&
                    GlobalVariables.Inistance.Team2.InformerQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(1, false);
                    return;
                }
                else if (GlobalVariables.Inistance.Team1.JokerRecieved &&
                    GlobalVariables.Inistance.Team1.OwnedJokerQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(2, false);
                    return;
                }
                else if (GlobalVariables.Inistance.Team1.ScholarshipRecieved &&
                    GlobalVariables.Inistance.Team1.OwnedScholerShipQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(3, false);
                    return;
                }
            }
            else if (GlobalVariables.Inistance.Team2.IsPlaying &&
                (GlobalVariables.Inistance.Team2.SkippedQuestions.Count + GlobalVariables.Inistance.Team2.CurrentRoundQuestions.Count >= 6))
            {
                GlobalVariables.Inistance.CurrentQuestion = GlobalVariables.Inistance.Team2.SkippedQuestions[0].Q;
                GlobalVariables.Inistance.CurrentPannedWords = GlobalVariables.Inistance.Team2.SkippedQuestions[0].PannedWords;
                GlobalVariables.Inistance.Team2.SkippedQuestions.RemoveAt(0);

                //if (GlobalVariables.Inistance.Team2.SkippedQuestions.Count == 0)
                //    GameSkipButton.interactable = false;

                if (GlobalVariables.Inistance.Team2.InformerRecieved &&
                    GlobalVariables.Inistance.Team1.InformerQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(1, false);
                    return;
                }
                else if (GlobalVariables.Inistance.Team2.JokerRecieved &&
                    GlobalVariables.Inistance.Team2.OwnedJokerQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(2, false);
                    return;
                }
                else if (GlobalVariables.Inistance.Team2.ScholarshipRecieved &&
                    GlobalVariables.Inistance.Team2.OwnedScholerShipQuestion.Equals(GlobalVariables.Inistance.CurrentQuestion))
                {
                    DisplayHelpQuestion(3, false);
                    return;
                }
            }

            else
            {
                GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                //validate question
                //bool isValid = DatabaseReader.ValidateQuestion(GlobalVariables.Inistance.CurrentQuestion);
                //if (!isValid)
                //{
                //    Debug.Log("Invalid Question Found: " + GlobalVariables.Inistance.CurrentQuestion.ToString());
                //    GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                //    isValid = DatabaseReader.ValidateQuestion(GlobalVariables.Inistance.CurrentQuestion);
                //    if (!isValid)
                //        Debug.Log("Invalid QSecond Time WOW: " + GlobalVariables.Inistance.CurrentQuestion.ToString());
                //}


                GlobalVariables.Inistance.CurrentPannedWords = null;
            }
            if (GlobalVariables.Inistance.CurrentQuestion == null)
            {
                ShowMessage("لقد قمت بالإجابة على" + "\n" + "كل الأسئلة");
                return;
            }

            ShowHideOnHelp(false);

            DisplayQuestion();
        }
    }

    private void DisplayQuestion()
    {

        //set team name
        //ArabicLineFixer Problem >> Text >> Category, Word, Team Names
        if (GlobalVariables.Inistance.Team1.IsPlaying)
            GameTeamNameText.text = ArabicFixer.Fix(GlobalVariables.Inistance.Team1.Name);
        else
            GameTeamNameText.text = ArabicFixer.Fix(GlobalVariables.Inistance.Team2.Name);

        //set main word & category
        GameCategoryImage.sprite = Resources.Load<Sprite>("Images/" + GlobalVariables.Inistance.CurrentQuestion.Category.ImageWithoutText);

        string[] nameSplited = GlobalVariables.Inistance.CurrentQuestion.Category.Name.Split();
        string name = "";
        for (int i = 0; i < nameSplited.Length; i++)
        {
            if (i == nameSplited.Length * 1 / 2)
                name += "\n";
            name += nameSplited[i] + " ";
        }
        GameCategotyNameText.text = ArabicFixer.Fix(name.Trim());

        if(GlobalVariables.Inistance.CurrentQuestion.Word.Length >= 14)
        {
            string[] splited = GlobalVariables.Inistance.CurrentQuestion.Word.Split();
            string SW = "";
            for (int i = 0; i < splited.Length; i++)
            {
                if (i == splited.Length / 2)
                    SW += "\n";
                SW += splited[i] + " ";
            }
            GameWordName.text = ArabicFixer.Fix(SW.Trim());
        }
        else
            GameWordName.text = ArabicFixer.Fix(GlobalVariables.Inistance.CurrentQuestion.Word);


        //randomlly select panned words and shuffle them
        List<string> CurrentPannedWords = new List<string>();
        List<int> PreviousIndex = new List<int>();

        if (GlobalVariables.Inistance.CurrentPannedWords == null)
        {
            //must take first and second word
            CurrentPannedWords.Add(GlobalVariables.Inistance.CurrentQuestion.PannedWords[0]);
            PreviousIndex.Add(0);
            CurrentPannedWords.Add(GlobalVariables.Inistance.CurrentQuestion.PannedWords[1]);
            PreviousIndex.Add(1);

            int R = -1;

            while (PreviousIndex.Count < 5)
            {
                R = Random.Range(2, GlobalVariables.Inistance.CurrentQuestion.PannedWords.Length);

                while (PreviousIndex.Contains(R))
                {
                    R = Random.Range(2, GlobalVariables.Inistance.CurrentQuestion.PannedWords.Length);
                }

                CurrentPannedWords.Add(GlobalVariables.Inistance.CurrentQuestion.PannedWords[R]);
                PreviousIndex.Add(R);
            }
        }
        else
        {
            CurrentPannedWords = GlobalVariables.Inistance.CurrentPannedWords;
        }
        CurrentPannedWords.Shuffle();
        CurrentPannedWordsList = CurrentPannedWords;

        for (int i = 0; i < CurrentPannedWords.Count; i++)
        {
            GameWordDetailsTextList[i].text = ArabicFixer.Fix(CurrentPannedWords[i]);
        }
    }

    private bool CheckHelpQuestions()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            if(GlobalVariables.Inistance.Team2.InformerUsed && !GlobalVariables.Inistance.Team1.InformerRecieved)
            {
                GlobalVariables.Inistance.Team1.InformerRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = GlobalVariables.Inistance.Team2.InformerQuestion;
                GlobalVariables.Inistance.CurrentPannedWords = null;

                DisplayHelpQuestion(1, true);

                return true;
            }
            else if (GlobalVariables.Inistance.Team2.JokerUsed && !GlobalVariables.Inistance.Team1.JokerRecieved)
            {
                GlobalVariables.Inistance.Team1.JokerRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                GlobalVariables.Inistance.CurrentPannedWords = null;
                GlobalVariables.Inistance.Team1.OwnedJokerQuestion = GlobalVariables.Inistance.CurrentQuestion;

                DisplayHelpQuestion(2, true);

                return true;
            }
            else if (GlobalVariables.Inistance.Team2.ScholarshipUsed && !GlobalVariables.Inistance.Team1.ScholarshipRecieved)
            {
                GlobalVariables.Inistance.Team1.ScholarshipRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                GlobalVariables.Inistance.CurrentPannedWords = null;
                GlobalVariables.Inistance.Team1.OwnedScholerShipQuestion = GlobalVariables.Inistance.CurrentQuestion;

                DisplayHelpQuestion(3, true);

                return true;
            }
            else
                return false;
        }
        else
        {
            if(GlobalVariables.Inistance.Team1.InformerUsed && !GlobalVariables.Inistance.Team2.InformerRecieved)
            {
                GlobalVariables.Inistance.Team2.InformerRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = GlobalVariables.Inistance.Team1.InformerQuestion;
                GlobalVariables.Inistance.CurrentPannedWords = null;

                DisplayHelpQuestion(1, true);

                return true;
            }
            else if (GlobalVariables.Inistance.Team1.JokerUsed && !GlobalVariables.Inistance.Team2.JokerRecieved)
            {
                GlobalVariables.Inistance.Team2.JokerRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                GlobalVariables.Inistance.CurrentPannedWords = null;
                GlobalVariables.Inistance.Team2.OwnedJokerQuestion = GlobalVariables.Inistance.CurrentQuestion;

                DisplayHelpQuestion(2, true);

                return true;
            }
            else if (  GlobalVariables.Inistance.Team1.ScholarshipUsed && !GlobalVariables.Inistance.Team2.ScholarshipRecieved)
            {
                GlobalVariables.Inistance.Team2.ScholarshipRecieved = true;
                GlobalVariables.Inistance.CurrentQuestion = QuestionPicker.Pick();
                GlobalVariables.Inistance.CurrentPannedWords = null;
                GlobalVariables.Inistance.Team2.OwnedScholerShipQuestion = GlobalVariables.Inistance.CurrentQuestion;

                DisplayHelpQuestion(3, true);

                return true;
            }
            else
                return false;
        }
    }

    private void DisplayHelpQuestion(int Type, bool IsFirstTime)
    {
        DisplayQuestion();

        ShowHideOnHelp(true);

        if (IsFirstTime)
        {
            SkipPaused = true;
            CO = PauseSkipUsage();
            StartCoroutine(CO);
        }

        switch (Type)
        {
            case 1:
                PanelHelpDetails.GetComponent<Image>().sprite = InformerImageInfo;
                PannelPannedWords.SetActive(true);
                ImageHelpIcon.SetActive(false);
                break;
            case 2:
                PanelHelpDetails.GetComponent<Image>().sprite = JokerImageInfo;
                ImageHelpIcon.GetComponent<Image>().sprite = SpriteJoker;
                break;
            case 3:
                PanelHelpDetails.GetComponent<Image>().sprite = SchollershipImageInfo;
                ImageHelpIcon.GetComponent<Image>().sprite = SpriteScholership;
                break;
        }
    }

    private IEnumerator PauseSkipUsage(float Time = 10)
    {
        GameSkipButton.interactable = false;

        while (SkipPaused)
        {
            if (Time <= 0)
            {
                SkipPaused = false;
                GameSkipButton.interactable = true;
                break;
            }

            yield return new WaitForSecondsRealtime(1);
            Time--;
        }
        if(!SkipPaused)
            GameSkipButton.interactable = true;

        //GameSkipButton.interactable = false;
        //yield return new WaitForSecondsRealtime(Time);
        //GameSkipButton.interactable = true;
    }

    private void ShowHideOnHelp(bool IsHelp)
    {
        ImageHelpIcon.SetActive(IsHelp);
        PanelHelpDetails.SetActive(IsHelp);
        PannelPannedWords.SetActive(!IsHelp);
        GameInformerButton.gameObject.SetActive(!IsHelp);
        GameNinjaButton.gameObject.SetActive(!IsHelp);
        GameKingButton.gameObject.SetActive(!IsHelp);
    }

    public void OnTimeFinished()
    {
        SetScorePanel();
    }

    public void OnGameTrueButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            GlobalVariables.Inistance.Team1.Score[GlobalVariables.Inistance.Team1.CurrentRound]++;

            GlobalVariables.Inistance.Team1.CurrentRoundQuestions.Add(GlobalVariables.Inistance.CurrentQuestion);

            GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Correct));
        }
        else
        {
            GlobalVariables.Inistance.Team2.Score[GlobalVariables.Inistance.Team2.CurrentRound]++;

            GlobalVariables.Inistance.Team2.CurrentRoundQuestions.Add(GlobalVariables.Inistance.CurrentQuestion);

            GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Correct));
        }


        GetNewQuestion();
    }

    public void OnGameFalseButtonClick()
    {
        TimerController.Inistance.DecreaseTime(5);

        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            GlobalVariables.Inistance.Team1.CurrentRoundQuestions.Add(GlobalVariables.Inistance.CurrentQuestion);

            GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Wrong));
        }
        else
        {
            GlobalVariables.Inistance.Team2.CurrentRoundQuestions.Add(GlobalVariables.Inistance.CurrentQuestion);

            GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Wrong));
        }

        GetNewQuestion();
    }

    public void OnGameSkipButtonClick()
    {
        SkippableQuestion SK = new SkippableQuestion(GlobalVariables.Inistance.CurrentQuestion, CurrentPannedWordsList);
        if (GlobalVariables.Inistance.Team1.IsPlaying)
            GlobalVariables.Inistance.Team1.SkippedQuestions.Add(SK);
        else
            GlobalVariables.Inistance.Team2.SkippedQuestions.Add(SK);

        GetNewQuestion();
    }

    public void OnGameInformerButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            GlobalVariables.Inistance.Team1.InformerUsed = true;
            GlobalVariables.Inistance.Team1.InformerQuestion = GlobalVariables.Inistance.CurrentQuestion;

            GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Passed));
        }
        else
        {
            GlobalVariables.Inistance.Team2.InformerUsed = true;
            GlobalVariables.Inistance.Team2.InformerQuestion = GlobalVariables.Inistance.CurrentQuestion;

            GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1]
                .Add(new AnswerdQuestion(GlobalVariables.Inistance.CurrentQuestion, AnswerState.Passed));
        }

        GameInformerButton.interactable = false;

        OnGameTrueButtonClick();
    }

    public void OnGameNinjaButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
            GlobalVariables.Inistance.Team1.NinjaUsed = true;
        else
            GlobalVariables.Inistance.Team2.NinjaUsed = true;

        GameNinjaButton.interactable = false;

        StartCoroutine(HideNinjaWord());
    }

    IEnumerator HideNinjaWord()
    {
        int R = Random.Range(0, GameWordDetailsTextList.Count);

        GameWordDetailsTextList[R].GetComponent<Animator>().enabled = true;
        //GameWordDetailsTextList[R].GetComponent<Animator>().SetTrigger("GO");

        float Height = GameWordDetailsTextList[0].GetComponent<RectTransform>().rect.height * R;
        GameNinjaAnimator.GetComponent<RectTransform>().offsetMin = 
            new Vector2(GameNinjaAnimator.GetComponent<RectTransform>().offsetMin.x,
            -Height);
        GameNinjaAnimator.GetComponent<RectTransform>().offsetMax = 
            new Vector2(GameNinjaAnimator.GetComponent<RectTransform>().offsetMax.x,
            -Height);

        GameNinjaAnimator.gameObject.SetActive(true);
        GameNinjaAnimator.SetTrigger("GO");

        yield return new WaitForSecondsRealtime(2f);

        if (GameWordDetailsTextList[R].GetComponent<Animator>().enabled)
        {
            GameWordDetailsTextList[R].text = ("");
            GameWordDetailsTextList[R].GetComponent<Animator>().enabled = false;
            //GameWordDetailsTextList[R].GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void OnGameKingButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
            GlobalVariables.Inistance.Team1.KingUsed = true;
        else
            GlobalVariables.Inistance.Team2.KingUsed = true;

        GameKingButton.interactable = false;

        GameKingAnimator.gameObject.SetActive(true);
        GameKingAnimator.SetTrigger("GO");

        TimerController.Inistance.IncreaseTime();
    }

    //Score
    public void SetScorePanel()
    {
        GameKingAnimator.gameObject.SetActive(false);
        GameNinjaAnimator.gameObject.SetActive(false);

        TimerController.Inistance.PauseTimer();

        PanelScore.SetActive(true);
        ScoreTeam1NameText.SetArabicText(GlobalVariables.Inistance.Team1.Name);
        ScoreTeam2NameText.SetArabicText(GlobalVariables.Inistance.Team2.Name);

        ScoreTeam1Score.text = GlobalVariables.Inistance.Team1.CalculateScore().ToString();
        ScoreTeam2Score.text = GlobalVariables.Inistance.Team2.CalculateScore().ToString();

        //reset buttons higlight
        ScoreScholershipButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        ScoreOldmanButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        ScoreJokerButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        //set Buttons
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            if (GlobalVariables.Inistance.Team1.ScholarshipUsed)
                ScoreScholershipButton.interactable = false;
            else
                ScoreScholershipButton.interactable = true;

            if (GlobalVariables.Inistance.Team1.OldmanUsed)
                ScoreOldmanButton.interactable = false;
            else
                ScoreOldmanButton.interactable = true;

            if (GlobalVariables.Inistance.Team1.JokerUsed)
                ScoreJokerButton.interactable = false;
            else
                ScoreJokerButton.interactable = true;
        }

        if (GlobalVariables.Inistance.Team2.IsPlaying)
        {
            if (GlobalVariables.Inistance.Team2.ScholarshipUsed)
                ScoreScholershipButton.interactable = false;
            else
                ScoreScholershipButton.interactable = true;

            if (GlobalVariables.Inistance.Team2.OldmanUsed)
                ScoreOldmanButton.interactable = false;
            else
                ScoreOldmanButton.interactable = true;

            if (GlobalVariables.Inistance.Team2.JokerUsed)
                ScoreJokerButton.interactable = false;
            else
                ScoreJokerButton.interactable = true;
        }


        if (GlobalVariables.Inistance.Team1.IsPlaying) {
            ScoreCurrentRoundText.text = GlobalVariables.Inistance.Team1.CurrentRound.ToString();
            //ScorePopulator.Populate(true , GlobalVariables.Inistance.Team1.Score[GlobalVariables.Inistance.Team1.CurrentRound]);

            for(int i = 0; i < ScoreTeam1QuestionsHolder.transform.childCount; i++)
            {
                if (GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1].Count <= i)
                    break;
                AnswerdQuestion item = GlobalVariables.Inistance.Team1.GameAnswerdQuestions[GlobalVariables.Inistance.Team1.CurrentRound - 1][i];
                switch (item.answerState)
                {
                    case AnswerState.Correct:
                        ScoreTeam1QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpriteCorrect;
                        break;
                    case AnswerState.Wrong:
                        ScoreTeam1QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpriteWrong;
                        break;
                    case AnswerState.Passed:
                        ScoreTeam1QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpritePass;
                        break;
                }
                
                ScoreTeam1QuestionsHolder.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = ArabicFixer.Fix(item.question.Word);
            }
        }
        else
        {
            ScoreCurrentRoundText.text = GlobalVariables.Inistance.Team2.CurrentRound.ToString();
            //ScorePopulator.Populate(false, GlobalVariables.Inistance.Team2.Score[GlobalVariables.Inistance.Team2.CurrentRound]);

            for (int i = 0; i < ScoreTeam2QuestionsHolder.transform.childCount; i++)
            {
                if (GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1].Count <= i)
                    break;

                AnswerdQuestion item = GlobalVariables.Inistance.Team2.GameAnswerdQuestions[GlobalVariables.Inistance.Team2.CurrentRound - 1][i];
                switch (item.answerState)
                {
                    case AnswerState.Correct:
                        ScoreTeam2QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpriteCorrect;
                        break;
                    case AnswerState.Wrong:
                        ScoreTeam2QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpriteWrong;
                        break;
                    case AnswerState.Passed:
                        ScoreTeam2QuestionsHolder.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = ScoreSpritePass;
                        break;
                }

                ScoreTeam2QuestionsHolder.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = ArabicFixer.Fix(item.question.Word);
            }
        }

        //stop attack cards at last round
        if (!(GlobalVariables.Inistance.Team2.CurrentRound + 1 <= GlobalVariables.Inistance.RoundsCount))
        {
            ScoreScholershipButton.interactable = false;
            ScoreOldmanButton.interactable = false;
            ScoreJokerButton.interactable = false;
        }
    }

    public void ResetScoreQuestionsHolderPanel()
    {
        foreach (Transform item in ScoreTeam1QuestionsHolder.transform)
        {
            item.GetChild(0).GetComponent<Image>().sprite = null;
            item.GetChild(1).GetComponent<Text>().text = null;
        }
        foreach (Transform item in ScoreTeam2QuestionsHolder.transform)
        {
            item.GetChild(0).GetComponent<Image>().sprite = null;
            item.GetChild(1).GetComponent<Text>().text = null;
        }
    }

    public void OnScoreSkipButtonClick()
    {
        GamePanel.SetActive(false);
        PanelScore.SetActive(false);
        SetDateOnPreGamePanel(!GlobalVariables.Inistance.Team1.IsPlaying);
    }

    public void OnScoreJokerButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            if (GlobalVariables.Inistance.Team1.JokerUsed)
            {
                ScoreJokerButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team1.JokerUsed = false;
            }
            else
            {
                ScoreJokerButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team1.JokerUsed = true;
            }
        }
        else
        {
            if (GlobalVariables.Inistance.Team2.JokerUsed)
            {
                ScoreJokerButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team2.JokerUsed = false;
            }
            else
            {
                ScoreJokerButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team2.JokerUsed = true;
            }
        }

        //if (GlobalVariables.Inistance.Team1.IsPlaying)
        //    GlobalVariables.Inistance.Team1.JokerUsed = true;
        //else
        //    GlobalVariables.Inistance.Team2.JokerUsed = true;

        //ScoreJokerButton.interactable = false;
    }

    public void OnScoreOldmanButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            if (GlobalVariables.Inistance.Team1.OldmanUsed)
            {
                ScoreOldmanButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team1.OldmanUsed = false;
            }
            else
            {
                ScoreOldmanButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team1.OldmanUsed = true;
            }
        }
        else
        {
            if (GlobalVariables.Inistance.Team2.OldmanUsed)
            {
                ScoreOldmanButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team2.OldmanUsed = false;
            }
            else
            {
                ScoreOldmanButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team2.OldmanUsed = true;
            }
        }

        //if (GlobalVariables.Inistance.Team1.IsPlaying)
        //    GlobalVariables.Inistance.Team1.OldmanUsed = true;
        //else
        //    GlobalVariables.Inistance.Team2.OldmanUsed = true;

        //ScoreOldmanButton.interactable = false;
    }

    public void OnScoreScholershipButtonClick()
    {
        if (GlobalVariables.Inistance.Team1.IsPlaying)
        {
            if (GlobalVariables.Inistance.Team1.ScholarshipUsed)
            {
                ScoreScholershipButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team1.ScholarshipUsed = false;
            }
            else
            {
                ScoreScholershipButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team1.ScholarshipUsed = true;
            }
        }
        else
        {
            if (GlobalVariables.Inistance.Team2.ScholarshipUsed)
            {
                ScoreScholershipButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                GlobalVariables.Inistance.Team2.ScholarshipUsed = false;
            }
            else
            {
                ScoreScholershipButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
                GlobalVariables.Inistance.Team2.ScholarshipUsed = true;
            }
        }

        //if (GlobalVariables.Inistance.Team1.IsPlaying)
        //    GlobalVariables.Inistance.Team1.ScholarshipUsed = true;
        //else
        //    GlobalVariables.Inistance.Team2.ScholarshipUsed = true;

        //ScoreScholershipButton.interactable = false;
    }

    //Winning
    public void OnGameOver()
    {
        AdDisplayed = false;

        GamePanel.SetActive(false);
        PreGamePanel.SetActive(false);
        WinningPannel.SetActive(true);

        int Team1Score = GlobalVariables.Inistance.Team1.CalculateScore();
        int Team2Score = GlobalVariables.Inistance.Team2.CalculateScore();

        WinningTeam1Name.SetArabicText(GlobalVariables.Inistance.Team1.Name);
        WinningTeam2Name.SetArabicText(GlobalVariables.Inistance.Team2.Name);
        WinningTeam1Score.text = Team1Score.ToString();
        WinningTeam2Score.text = Team2Score.ToString();
        if (Team1Score > Team2Score)
            WinningWinningName.SetArabicText(GlobalVariables.Inistance.Team1.Name);
        else if(Team1Score < Team2Score)
            WinningWinningName.SetArabicText(GlobalVariables.Inistance.Team2.Name);
        else
            WinningWinningName.SetArabicText("التعادل عادل");
        //WinningWinningName.SetArabicText(Team1Score >= Team2Score? GlobalVariables.Inistance.Team1.Name: GlobalVariables.Inistance.Team2.Name);

        TempTeam1Name = GlobalVariables.Inistance.Team1.Name;
        TempTeam2Name = GlobalVariables.Inistance.Team2.Name;

        GlobalVariables.Inistance.ClearData();

        StartCoroutine(DsiplayAdd(false));
    }

    public void OnWinningPlayAgainClick()
    {
        StartCoroutine(DsiplayAdd(true));


        WinningPannel.SetActive(false);
        //CategoriesPanel.SetActive(true);

        GlobalVariables.Inistance.SelectedCategories = TempSelectedCategoriesList;
        PrepareAvailableQuestions();
        GlobalVariables.Inistance.Team1 = new Team(TempTeam1Name);
        GlobalVariables.Inistance.Team2 = new Team(TempTeam2Name);

        //reset attack buttons
        ScoreScholershipButton.interactable = true;
        ScoreOldmanButton.interactable = true;
        ScoreJokerButton.interactable = true;

        SetDateOnPreGamePanel(true);
    }

    public IEnumerator DsiplayAdd(bool Immediate)
    {
        if (GlobalVariables.Inistance.ShowAds && !AdDisplayed)
        {
            if (!Immediate)
                yield return new WaitForSecondsRealtime(3f);
            else
                yield return null;
            AdController.Inistance.PlayInterstitialAd();
            AdDisplayed = true;
        }
        else
            yield return null;
    }
}
