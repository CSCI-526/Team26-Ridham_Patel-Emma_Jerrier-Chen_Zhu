using UnityEngine;

public enum WorldColor { Black, White }

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class ColorBlock : MonoBehaviour
{
    public bool isPlatform = true;
    public int order = -1;
    public float breakTime = 5f;

    public WorldColor blockColor { get; private set; }
    public bool IsVisible => picture.enabled ||
                             (breakingPicture != null && breakingPicture.enabled);

    private SpriteRenderer picture;
    private SpriteRenderer breakingPicture;
    private BoxCollider2D solid;
    private PlayerMovement player;
    private CollectableInventory collectables;
    private Color originalColor;

    private Vector2 startingSize;
    private Vector2 startingOffset;
    private float timeLeft;
    private float duration;
    private bool breaking;
    private bool broken;

    private void Awake()
    {
        picture = GetComponent<SpriteRenderer>();
        solid = GetComponent<BoxCollider2D>();
        originalColor = picture.color;

        blockColor = picture.color.grayscale >= 0.5f
            ? WorldColor.White
            : WorldColor.Black;
    }

    public void Show(bool visible, bool revealHidden = false)
    {
        if (breaking || broken) return;

        picture.enabled = visible || revealHidden;
        Color displayColor = originalColor;
        // Revealed blocks are visual hints only; their collision stays unchanged.
        if (!visible && revealHidden) displayColor.a *= 0.4f;
        picture.color = displayColor;
        solid.enabled = visible;
    }

    public void BeginBreak(PlayerMovement standingPlayer)
    {
        if (!isPlatform || breaking || broken) return;

        player = standingPlayer;
        collectables = standingPlayer.GetComponent<CollectableInventory>();
        breaking = true;
        duration = Mathf.Max(0.05f, breakTime);
        timeLeft = duration;

        startingSize = solid.size;
        startingOffset = solid.offset;

        GameObject visual = new GameObject("BreakingVisual");
        visual.transform.SetParent(transform, false);

        breakingPicture = visual.AddComponent<SpriteRenderer>();
        breakingPicture.sprite = picture.sprite;
        breakingPicture.color = originalColor;
        breakingPicture.sortingLayerID = picture.sortingLayerID;
        breakingPicture.sortingOrder = picture.sortingOrder;

        picture.enabled = false;
        UpdateShape(0f);
    }

    private void Update()
    {
        if (!breaking) return;

        if (collectables == null || !collectables.IsFreezeActive)
            timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            breaking = false;
            broken = true;
            solid.enabled = false;
            breakingPicture.enabled = false;
            Destroy(breakingPicture.gameObject);
            return;
        }

        float progress = 1f - timeLeft / duration;
        UpdateShape(progress);

        // A block the player has left cannot catch them again.
        if (player.GetPlatform() != this)
            solid.enabled = false;
    }

    private void UpdateShape(float progress)
    {
        float remaining = Mathf.Max(0.01f, 1f - progress);

        // Shrink the square from left to right.
        breakingPicture.transform.localScale =
            new Vector3(remaining, 1f, 1f);

        breakingPicture.transform.localPosition =
            new Vector3(progress / 2f, 0f, 0f);

        // Keep the solid area exactly under the visible area.
        solid.size = new Vector2(startingSize.x * remaining, startingSize.y);
        solid.offset = new Vector2(
            startingOffset.x + startingSize.x * progress / 2f,
            startingOffset.y
        );
    }
}
