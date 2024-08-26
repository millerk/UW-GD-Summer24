using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int totalGold = 0;
    public Text currencyText;
    public Text healthText;

    private static string CURRENT_GOLD = "Player Gold";

    public void OnGoldCollection(GameObject gold)
    {
        UpdateGold(gold.GetComponent<Gold>().value);
    }

    public void OnHealthUpdate(GameObject player)
    {
        int health = player.GetComponent<HealthManager>().hitPoints;
        healthText.text = health.ToString();
    }

    public void OnLevelLoad(GameObject levelDef)
    {
        if (GlobalVariables.Get<int>(CURRENT_GOLD) != 0)
        {
            UpdateGold(GlobalVariables.Get<int>(CURRENT_GOLD));
        }
    }

    public void OnLevelComplete(GameObject obj)
    {
        GlobalVariables.Set(CURRENT_GOLD, totalGold);
    }

    private void UpdateGold(int amount)
    {
        totalGold += amount;
        currencyText.text = totalGold.ToString();
    }
}
