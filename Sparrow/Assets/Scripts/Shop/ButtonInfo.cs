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
        //displayIcon.sprite = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId].shopIcon;
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
        CheckIfEnoughGoldToBuy();
    }

    private void CheckIfEnoughGoldToBuy()
    {
        int totalGold = ShopManager.GetComponent<ShopManagerLogic>().totalGold;
        if (totalGold < cost)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
