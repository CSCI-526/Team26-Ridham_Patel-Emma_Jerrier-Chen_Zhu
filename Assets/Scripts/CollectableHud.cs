using UnityEngine;
using UnityEngine.UI;

public class CollectableHud : MonoBehaviour
{
    public CollectableInventory inventory;
    public GameObject freezeSlot;
    public GameObject revealSlot;
    public Text freezeStatus;
    public Text revealStatus;

    private void LateUpdate()
    {
        if (inventory == null)
        {
            freezeSlot.SetActive(false);
            revealSlot.SetActive(false);
            return;
        }

        UpdateSlot(freezeSlot, freezeStatus, inventory.FreezeCount, inventory.FreezeRemaining);
        UpdateSlot(revealSlot, revealStatus, inventory.RevealCount, inventory.RevealRemaining);
    }

    private static void UpdateSlot(GameObject slot, Text status, int count, float remaining)
    {
        slot.SetActive(count > 0 || remaining > 0f);
        if (!slot.activeSelf) return;

        status.text = remaining > 0f
            ? "ACTIVE " + remaining.ToString("0.0") + "s" + (count > 0 ? "  x" + count : "")
            : "READY  x" + count;
    }
}
