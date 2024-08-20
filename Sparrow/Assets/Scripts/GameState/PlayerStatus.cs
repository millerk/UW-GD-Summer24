using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int totalGold = 0;
    public Text currencyText;
    public Text healthText;
    public void OnGoldCollection(GameObject gold)
    {
        totalGold += gold.GetComponent<Gold>().value;
        currencyText.text = totalGold.ToString();
    }

    public void OnHealthUpdate(GameObject player)
    {
        int health = player.GetComponent<HealthManager>().HitPoints;
        healthText.text = health.ToString();
    }
}
