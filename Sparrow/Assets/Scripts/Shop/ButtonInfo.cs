using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemId;
    public Text PriceText;
    public Text DescriptionText;
    public GameObject ShopManager;
    public int cost;
    public Image displayIcon;
    public Color32 tooExpensiveColor;

    void OnEnable()
    {
        tooExpensiveColor = new Color32(115, 23, 45, 225);
    }

    public void CannonsLoaded(GameObject source)
    {
        Cannon cannon = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId];
        cost = cannon.shopCost;
        // Check if text fits in display, scale down font size if it doesn't
        if (cost > 99 && cost <= 999)
        {
            PriceText.fontSize = 14;
        }
        if (cost > 999)
        {
            PriceText.fontSize = 11;
        }
        PriceText.text = cannon.shopCost.ToString();
        DescriptionText.text = cannon.description;
        CheckIfEnoughGoldToBuy();
        displayIcon.sprite = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId].shopIcon;
    }

    public void OnItemPurchase(GameObject source)
    {
        CheckIfEnoughGoldToBuy();
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
            PriceText.color = tooExpensiveColor;
            SuppressButton();
        }
    }
}
