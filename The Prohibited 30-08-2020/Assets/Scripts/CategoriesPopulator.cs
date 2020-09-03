using ArabicSupport;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesPopulator: MonoBehaviour
{
    static GameObject MainContent, CategoriesHolderPrefab, CategoryObjectPrefab, HeaderPrefab, SuggestPremuimPanel, CategoriesPanel, CategoriesFooterPrefab,
        CategoryInfoPanel;
    static Sprite LockImage, SelectedImage;

    static GameObject CategoriesContainerObject, HeaderObject, FooterObject;

    static float HeaderWidth, HeaderHeight, CellWidth, CellHeight;

    public static void Start()
    {
        MainContent = UIController.Inistance.MainContent;
        CategoriesHolderPrefab = UIController.Inistance.CategoriesContentPrefab;
        HeaderPrefab = UIController.Inistance.CategoriesHeaderPrefab;
        CategoriesFooterPrefab = UIController.Inistance.CategoriesFooterPrefab;
        CategoryObjectPrefab = UIController.Inistance.CategoryObjectPrefab;
        CategoriesPanel = UIController.Inistance.CategoriesPanel;
        SuggestPremuimPanel = UIController.Inistance.SuggestPremuimPanel;
        CategoryInfoPanel = UIController.Inistance.CategoryInfoPanel;
        LockImage = UIController.Inistance.LockImage;
        SelectedImage = UIController.Inistance.SelectedImage;

        MainContent.transform.parent.parent.GetComponent <ScrollRect>().normalizedPosition = new Vector2(0, 1);

        if (MainContent.transform.childCount > 0)
        {
            foreach (Transform child in MainContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        Prepare();
    }

    private static void Prepare()
    {
        HeaderWidth = CategoriesPanel.GetComponent<RectTransform>().rect.width;
        HeaderHeight = CategoriesPanel.GetComponent<RectTransform>().rect.height * (1f - 0.62f);

        HeaderObject = Instantiate(HeaderPrefab, MainContent.transform);
        HeaderObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, -HeaderHeight);//left, bottom
        HeaderObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);//right, top

        HeaderObject.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        HeaderObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { OnHeaderButtonBackClick(); });
        AudioController.Inistance.ButtonSoundFromScript(HeaderObject.transform.GetChild(1).GetComponent<Button>());

        HeaderObject.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        HeaderObject.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { OnHeaderButtonNextClick(); });
        AudioController.Inistance.ButtonSoundFromScript(HeaderObject.transform.GetChild(3).GetComponent<Button>());

        CategoriesContainerObject = Instantiate(CategoriesHolderPrefab, MainContent.transform);
        CategoriesContainerObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);//left, bottom
        CategoriesContainerObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, -HeaderHeight);//right, top

        PopulateCategories();
    }

    private static void OnHeaderButtonBackClick()
    {
        CategoriesPanel.SetActive(false);
        GlobalVariables.Inistance.ClearData();
    }

    private static void OnHeaderButtonNextClick()
    {
        UIController.Inistance.OnCategoriesNextButtonClick();
    }

    public static void PopulateCategories()
    {
        GridLayoutGroup GLG = CategoriesContainerObject.GetComponent<GridLayoutGroup>();

        CellWidth = (MainContent.GetComponent<RectTransform>().rect.width / 2) - (GLG.spacing.x * 2);
        CellHeight = CellWidth * 1.125f;
        GLG.cellSize = new Vector2(CellWidth, CellHeight);

        foreach (Category category in GlobalVariables.Inistance.AllCategories)
        {
            GameObject Current = Instantiate(CategoryObjectPrefab, CategoriesContainerObject.transform);
            Current.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + category.ImageWithText);
            Current.GetComponent<Button>().onClick.RemoveAllListeners();
            AudioController.Inistance.ButtonSoundFromScript(Current.GetComponent<Button>());

            Current.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            

            if (category.IsFree || GlobalVariables.Inistance.Premium)
            {
                Current.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                Current.transform.GetChild(1).GetComponent<Image>().sprite = null;

                Current.GetComponent<Button>().onClick.AddListener(
                () => { AddOrRemoveCategory(category, Current.transform.GetChild(1).GetComponent<Image>()); }
                );

                Current.transform.GetChild(2).gameObject.SetActive(true);
                Current.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(
                () => { DisplayInfo(category); }
                );
                AudioController.Inistance.ButtonSoundFromScript(Current.transform.GetChild(2).GetComponent<Button>());
            }
            else
            {
                Current.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                Current.transform.GetChild(1).GetComponent<Image>().sprite = LockImage;

                Current.GetComponent<Button>().onClick.AddListener(
                () => { SuggestPremium(category); }
                );

                Current.transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        SetFooter(GLG);
    }

    private static void AddOrRemoveCategory(Category category, Image ActionImage)
    {
        if (GlobalVariables.Inistance.SelectedCategories.Contains(category))
        {
            GlobalVariables.Inistance.SelectedCategories.Remove(category);
            ActionImage.color = new Color(1, 1, 1, 0);
            ActionImage.sprite = null;
        }
        else
        {
            GlobalVariables.Inistance.SelectedCategories.Add(category);
            ActionImage.color = new Color(1, 1, 1, 1);
            ActionImage.sprite = SelectedImage;
        }
    }

    private static void SuggestPremium(Category category)
    {
        SuggestPremuimPanel.SetActive(true);

        string[] nameSplited = category.LongName.Split();
        string LongName = "";
        //for (int i = 0; i < nameSplited.Length; i++)
        //{
        //    LongName += nameSplited[i] + " ";
        //    if (i == nameSplited.Length / 2)
        //        LongName += "\n";
        //}
        if(nameSplited.Length == 2)
            LongName = "\n" + nameSplited[0] + " " + nameSplited[1] + "\n";
        else
            LongName = nameSplited[0] + " " + nameSplited[1] + "\n" + nameSplited[2];

        SuggestPremuimPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(LongName);


        string[] descSplited = category.Description.Split();
        string Desc = "";
        int wordsCountInLine = 0;
        for(int i = 0; i < descSplited.Length; i++)
        {
            Desc += descSplited[i] + " ";

            //max words per line is 4
            if(wordsCountInLine == 3)
            {
                wordsCountInLine = 0;
                Desc += "\n";
                continue;
            }
            wordsCountInLine++;

            //wordsCountInLine--;
            //if (descSplited.Length < 8 && i == descSplited.Length * 1 / 2)
            //    Desc += "\n";
            //else if (descSplited.Length <= 12 && (i == descSplited.Length * 1 / 3 || i == descSplited.Length * 2 / 3))
            //    Desc += "\n";
            //else if (descSplited.Length > 12 && (i == descSplited.Length * 1 / 4 || i == descSplited.Length * 2 / 4 || i == descSplited.Length * 3 / 4))
            //    Desc += "\n";
            //else
            //    wordsCountInLine += 2;
        }
        SuggestPremuimPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = ArabicFixer.Fix(Desc);
        SuggestPremuimPanel.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + category.ImageWithoutText);

        SuggestPremuimPanel.transform.GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
        //SuggestPremuimPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { OnButtonBuyClick(); });
        SuggestPremuimPanel.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { OnSingleCategoryBuy(category); });
        AudioController.Inistance.ButtonSoundFromScript(SuggestPremuimPanel.transform.GetChild(3).GetComponent<Button>());
    }

    private static void DisplayInfo(Category category)
    {
        CategoryInfoPanel.SetActive(true);

        string[] nameSplited = category.LongName.Split();
        string LongName = "";
        //for (int i = 0; i < nameSplited.Length; i++)
        //{
        //    LongName += nameSplited[i] + " ";
        //    if (i == nameSplited.Length / 2)
        //        LongName += "\n";
        //}
        if (nameSplited.Length == 2)
            LongName = "\n" + nameSplited[0] + " " + nameSplited[1] + "\n";
        else
            LongName = nameSplited[0] + " " + nameSplited[1] + "\n" + nameSplited[2];

        CategoryInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ArabicFixer.Fix(LongName);


        string[] descSplited = category.Description.Split();
        string Desc = "";
        int wordsCountInLine = 0;
        for (int i = 0; i < descSplited.Length; i++)
        {
            Desc += descSplited[i] + " ";

            //max words per line is 4
            if (wordsCountInLine == 3)
            {
                wordsCountInLine = 0;
                Desc += "\n";
                continue;
            }
            wordsCountInLine++;

            //wordsCountInLine--;
            //if (descSplited.Length < 8 && i == descSplited.Length * 1 / 2)
            //    Desc += "\n";
            //else if (descSplited.Length <= 12 && (i == descSplited.Length * 1 / 3 || i == descSplited.Length * 2 / 3))
            //    Desc += "\n";
            //else if (descSplited.Length > 12 && (i == descSplited.Length * 1 / 4 || i == descSplited.Length * 2 / 4 || i == descSplited.Length * 3 / 4))
            //    Desc += "\n";
            //wordsCountInLine += 2;
        }
        CategoryInfoPanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = ArabicFixer.Fix(Desc);
        CategoryInfoPanel.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + category.ImageWithoutText);
    }

    private static void SetFooter(GridLayoutGroup GLG)
    {
        float FotterHeight = HeaderHeight;// * 0.7f;

        FooterObject = Instantiate(CategoriesFooterPrefab, MainContent.transform);
        FooterObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, FotterHeight);

        FooterObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

        //TODO: 
        if(GlobalVariables.Inistance.Premium || isAllBought())
        {
            FooterObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            FooterObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OnButtonBuyClick(); });
            AudioController.Inistance.ButtonSoundFromScript(FooterObject.transform.GetChild(0).GetComponent<Button>());
        }

        //int freeCategoriesCount = 0;
        //foreach (Category item in GlobalVariables.Inistance.AllCategories)
        //{
        //    if (item.IsFree)
        //        freeCategoriesCount++;
        //}

        //if (!GlobalVariables.Inistance.Premium && freeCategoriesCount <= GlobalVariables.Inistance.FreeStatCategoriesCount)
        //{
        //    FooterObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { OnButtonBuyClick(); });
        //    AudioController.Inistance.ButtonSoundFromScript(FooterObject.transform.GetChild(0).GetComponent<Button>());
        //}
        //else
        //{
        //    FooterObject.transform.GetChild(0).GetComponent<Button>().interactable = false;
        //}
        

        FooterObject.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        FooterObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { OnFotterButtonHowToPlayClick(); });
        AudioController.Inistance.ButtonSoundFromScript(FooterObject.transform.GetChild(1).GetComponent<Button>());


        FooterObject.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        FooterObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { OnHeaderButtonNextClick(); });
        AudioController.Inistance.ButtonSoundFromScript(FooterObject.transform.GetChild(2).GetComponent<Button>());



        float ContainerHeight = (CellHeight/* + GLG.spacing.x*/) * (GlobalVariables.Inistance.AllCategories.Count / 2) + GLG.spacing.x;
        if (GlobalVariables.Inistance.AllCategories.Count % 2 != 0)
            ContainerHeight += (CellHeight + GLG.spacing.x);

        float FullHeight = ContainerHeight + HeaderObject.GetComponent<RectTransform>().rect.height + FooterObject.GetComponent<RectTransform>().rect.height;

        MainContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MainContent.GetComponent<RectTransform>().rect.width);
        MainContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, FullHeight);
    }

    private static void OnButtonBuyClick()
    {
        IAPManager.instance.BuyProductAll();
    }

    private static void OnFotterButtonHowToPlayClick()
    {
        CategoriesPanel.SetActive(false);
        GlobalVariables.Inistance.ClearData();
        UIController.Inistance.HowToPlayPanel.SetActive(true);
    }

    private static void OnSingleCategoryBuy(Category category)
    {
        if(category.PurchaseLabel.Equals(IAPManager.instance.ProductCartoon))
            IAPManager.instance.BuyProductCatroon();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductFood))
            IAPManager.instance.BuyProductFood();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductSport))
            IAPManager.instance.BuyProductSport();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductRelegion))
            IAPManager.instance.BuyProductRelegion();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductcTv))
            IAPManager.instance.BuyProductcTv();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductBrands))
            IAPManager.instance.BuyProductBrands();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductTravel))
            IAPManager.instance.BuyProductTravel();

        else if (category.PurchaseLabel.Equals(IAPManager.instance.ProductEdu))
            IAPManager.instance.BuyProductEdu();
    }

    private static bool isAllBought()
    {
        bool output = true;
        foreach (Category category in GlobalVariables.Inistance.AllCategories)
        {
            if (!category.IsFree)
            {
                output = false;
                break;
            }
        }
        return output;
    }
}
