using UnityEngine;

public class RestorePurchases : MonoBehaviour
{
	public bool isAndroid = true;
	
    void Start()
    {
        if (isAndroid) {
            gameObject.SetActive(false);
        }
		else {
			gameObject.SetActive(true);
		}
    }

    public void ClickRestorePurchaseButton() {
        IAPManager.instance.RestorePurchases();
    }
}
