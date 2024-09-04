using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public int totalGold = 0;
    public Text currencyText;
    
    public Slider healthBarSlider;
    public Slider dashCooldownSlider;
    public GameObject dashCooldownText;

    public static string CURRENT_GOLD = "Player Gold";

    public void OnGoldCollection(GameObject gold)
    {
        UpdateGold(gold.GetComponent<Gold>().value);
    }

    public void SetMaxHealth(int health)
    {
        healthBarSlider.maxValue = health;
        healthBarSlider.value = health;
    }

    public void OnHealthUpdate(GameObject player)
    {
        HealthManager healthManager = player.GetComponent<HealthManager>();
        if (healthBarSlider.maxValue == 1)
        {
            SetMaxHealth(healthManager.maxHealth);
        }
        int health = healthManager.hitPoints;
        healthBarSlider.value = health;
    }

    public void OnDashCooldownUpdate(GameObject player)
    {
        ShipMovement shipMovement = player.GetComponent<ShipMovement>();
        float maxCooldown = shipMovement.dashCooldown;
        float remaining = shipMovement.dashCooldownRemaining;
        dashCooldownText.SetActive(remaining > 0f);
        dashCooldownSlider.value = remaining / maxCooldown;
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
