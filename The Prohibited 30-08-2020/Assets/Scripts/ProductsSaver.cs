using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductsSaver
{
    static string KEY = "SavedProducts";

    public static void getOpenProducts()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string products = PlayerPrefs.GetString(KEY);
            string[] list = products.Trim().Split(',');

            foreach (string item in list)
            {
                if (item.Trim().Length < 1)
                    break;

                if (item.Trim().Equals(IAPManager.instance.ProductAll))
                    GlobalVariables.Inistance.Premium = true;

                else
                {
                    foreach (Category category in GlobalVariables.Inistance.AllCategories)
                    {
                        if (category.PurchaseLabel.Equals(item.Trim()))
                        {
                            category.IsFree = true;
                            break;
                        }
                    }
                }
            }
        }

        //if bought any products >> stop ads
        GlobalVariables.Inistance.ShowAds = !boughtAnyThingBefore();
    }

    public static void addProduct(string productName)
    {
        string products = "";
        if (PlayerPrefs.HasKey(KEY))
            products = PlayerPrefs.GetString(KEY);
        products = products.Trim() + productName + ",";
        PlayerPrefs.SetString(KEY, products);

        //if bought any products >> stop ads
        GlobalVariables.Inistance.ShowAds = false;
    }

    public static void clearAll()
    {
        if (PlayerPrefs.HasKey(KEY))
            PlayerPrefs.DeleteKey(KEY);
    }

    public static bool boughtAnyThingBefore()
    {
        bool bought = false;

        if (PlayerPrefs.HasKey(KEY))
        {
            string products = PlayerPrefs.GetString(KEY);
            string[] list = products.Trim().Split(',');

            foreach (string item in list)
            {
                if (item.Trim().Length < 1)
                {
                    bought = false;
                    break;
                }

                if (item.Trim().Equals(IAPManager.instance.ProductAll))
                {
                    bought = true;
                    break;
                }

                else
                {
                    foreach (Category category in GlobalVariables.Inistance.AllCategories)
                    {
                        if (category.PurchaseLabel.Equals(item.Trim()))
                        {
                            bought = true;
                            break;
                        }
                    }
                    if (bought)
                        break;
                }
            }
        }

        return bought;
    }
}
