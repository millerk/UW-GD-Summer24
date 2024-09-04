using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemId;
    public Text PriceText;
    public GameObject ShopManager;
    public int cost;
    public Image displayIcon;
    void Start()
    {
        cost = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId].shopCost;
        PriceText.text = "Price: " + cost + " gold";
        CheckIfEnoughGoldToBuy();
    }

    public void UpdateDisplaySprite(GameObject source)
    {
        Debug.Log("Updating display sprite");
        displayIcon.sprite = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId].shopIcon;
    }

    public void OnItemPurchase(GameObject source)
    {
        SuppressButton();
    }

    private void SuppressButton()
    {
        gameObject.GetComponent<Button>().interactable = false;
    }

    private void CheckIfEnoughGoldToBuy()
    {
        int totalGold = ShopManager.GetComponent<ShopManagerLogic>().totalGold;
        if (totalGold < cost)
        {
            SuppressButton();
        }
    }
}
