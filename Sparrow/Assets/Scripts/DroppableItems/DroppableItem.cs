using UnityEngine;

public class DroppableItem : MonoBehaviour
{
    public GameEvent pickupEvent;
    public void OnCollisionEnter2D(Collision2D other)
    {
        // Double check this is the player ship
        if (other.gameObject.CompareTag("Player"))
        {
            pickupEvent.TriggerEvent(gameObject);
            Destroy(gameObject);
        }
    }
}
