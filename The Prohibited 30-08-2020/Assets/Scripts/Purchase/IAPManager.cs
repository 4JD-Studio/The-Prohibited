using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    public string ProductCartoon = "product_cartoon";
    public string ProductFood = "product_food";
    public string ProductSport = "product_sport";
    public string ProductRelegion= "product_relegion";
    public string ProductcTv = "product_tv";
    public string ProductBrands = "product_brands";
    public string ProductTravel = "product_travel"; //for android
	//public string ProductTravel = "product_travel_second"; //for ios
    public string ProductEdu = "product_edu";
    public string ProductAll = "product_all";



    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(ProductCartoon, ProductType.NonConsumable);
        builder.AddProduct(ProductFood, ProductType.NonConsumable);
        builder.AddProduct(ProductSport, ProductType.NonConsumable);
        builder.AddProduct(ProductRelegion, ProductType.NonConsumable);
        builder.AddProduct(ProductcTv, ProductType.NonConsumable);
        builder.AddProduct(ProductBrands, ProductType.NonConsumable);
        builder.AddProduct(ProductTravel, ProductType.NonConsumable);
        builder.AddProduct(ProductEdu, ProductType.NonConsumable);
        builder.AddProduct(ProductAll, ProductType.NonConsumable);


        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public void BuyProductCatroon()
    {
        BuyProductID(ProductCartoon);
    }

    public void BuyProductFood()
    {
        BuyProductID(ProductFood);
    }

    public void BuyProductSport()
    {
        BuyProductID(ProductSport);
    }

    public void BuyProductRelegion()
    {
        BuyProductID(ProductRelegion);
    }

    public void BuyProductcTv()
    {
        BuyProductID(ProductcTv);
    }

    public void BuyProductBrands()
    {
        BuyProductID(ProductBrands);
    }

    public void BuyProductTravel()
    {
        BuyProductID(ProductTravel);
    }

    public void BuyProductEdu()
    {
        BuyProductID(ProductEdu);
    }

    public void BuyProductAll()
    {
        BuyProductID(ProductAll);
    }




    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (UIController.Inistance.SuggestPremuimPanel.activeSelf)
            UIController.Inistance.SuggestPremuimPanel.SetActive(false);


        if (String.Equals(args.purchasedProduct.definition.id, ProductCartoon, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductCartoon);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductFood, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductFood);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductSport, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductSport);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductRelegion, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductRelegion);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductcTv, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductcTv);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductBrands, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductBrands);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductTravel, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductTravel);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductEdu, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductEdu);
        }
        else if (String.Equals(args.purchasedProduct.definition.id, ProductAll, StringComparison.Ordinal))
        {
            afterBoughtSuccessfully(ProductAll);
        }
        else
        {
            Debug.Log("Purchase Failed");
            UIController.Inistance.ShowMessage("Purchase Failed");
        }

        GlobalVariables.Inistance.ClearData();

        return PurchaseProcessingResult.Complete;
    }

    private void afterBoughtSuccessfully(string productID)
    {
        Debug.Log(productID + " Bought Successfully");

        if (productID.Equals(ProductAll))
        {
            GlobalVariables.Inistance.Premium = true;
        }
        else
        {
            Category Current = GlobalVariables.Inistance.AllCategories.Find(C => C.PurchaseLabel.Equals(productID));
            Current.IsFree = true;
        }

        ProductsSaver.addProduct(productID);
        GlobalVariables.Inistance.ClearData();
    }









    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}