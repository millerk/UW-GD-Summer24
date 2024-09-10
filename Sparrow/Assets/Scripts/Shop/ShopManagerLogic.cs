using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManagerLogic : MonoBehaviour
{
    public int numCannonsToChoose;
    public Cannon[] cannonsList;
    public Cannon[] availableCannons;
    public int totalGold;
    public Cannon[] playerCannons;
    public Text goldText;
    public GameEvent cannonsLoadedEvent;
    public GameEvent itemPurchaseEvent;

    void Start()
    {
        if (totalGold == 0)
        {
            totalGold = GlobalVariables.Get<int>(PlayerStatus.CURRENT_GOLD);
        }
        UpdateGold(totalGold);

        playerCannons = GlobalVariables.Get<Cannon[]>(ShipConfiguration.PLAYER_CANNON_DEF);
        if (playerCannons == null || playerCannons.Length == 0)
        {
            playerCannons = new Cannon[5];
        }
        SetAvailableCannons();
        cannonsLoadedEvent.TriggerEvent(gameObject);
    }

    private void SetAvailableCannons()
    {
        // Very dumb sample implementation of selecting numCannons from total possible list
        availableCannons = new Cannon[numCannonsToChoose];
        int ind = 0;
        while (ind < numCannonsToChoose)
        {
            availableCannons[ind] = cannonsList[Random.Range(0, cannonsList.Length)];
            ind++;
        }
    }

    private void UpdateGold(int amount)
    {
        totalGold = amount;
        goldText.text = "Available Gold: " + totalGold;
    }

    // Another very dumb sample implementation where we just add the cannon to the next available slot
    private void AddCannonToPlayerList(Cannon newCannon)
    {
        for (int i = 0; i < playerCannons.Length; i++)
        {
            if (playerCannons[i] == null)
            {
                playerCannons[i] = newCannon;
                return;
            }
        }
    }

    public void Buy(GameObject button)
    {
        Cannon requestedToBuy = availableCannons[button.GetComponent<ButtonInfo>().ItemId];
        if (totalGold >= requestedToBuy.shopCost)
        {
            UpdateGold(totalGold - requestedToBuy.shopCost);
            AddCannonToPlayerList(requestedToBuy);
            itemPurchaseEvent.TriggerEvent(gameObject);
        }
    }

    public void LoadLevel()
    {
        GlobalVariables.Set(PlayerStatus.CURRENT_GOLD, totalGold);
        GlobalVariables.Set(ShipConfiguration.PLAYER_CANNON_DEF, playerCannons);
        LevelDefinition nextScreen = GlobalVariables.Get<LevelDefinition>(LevelManager.NEXT_LEVEL);
        if (nextScreen is DialogueLevelDefinition)
        {
            GlobalVariables.Set(LevelManager.NEXT_LEVEL, nextScreen.nextLevel);
            SceneManager.LoadScene((nextScreen as DialogueLevelDefinition).DialogueScene);
        }
        else
        {
            SceneManager.LoadScene("Scenes/Battlefield");
        }
    }
}
