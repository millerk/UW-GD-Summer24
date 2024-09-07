using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemId;
    public Text PriceText;
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
        cost = ShopManager.GetComponent<ShopManagerLogic>().availableCannons[ItemId].shopCost;
        PriceText.text = cost.ToString();
        CheckIfEnoughGoldToBuy();
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
            PriceText.color = tooExpensiveColor;
            SuppressButton();
        }
    }
}
