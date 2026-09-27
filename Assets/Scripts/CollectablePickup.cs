using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class CollectablePickup : MonoBehaviour
{
    public CollectableType type;
    private bool collected;

    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        var inventory = other.GetComponentInParent<CollectableInventory>();
        if (inventory == null) return;

        collected = true;
        inventory.Collect(type);
        // Disable immediately so multiple colliders cannot collect it twice.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
