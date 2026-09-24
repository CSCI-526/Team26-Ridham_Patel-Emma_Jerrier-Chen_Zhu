using UnityEngine;

public class WorldSwitch : MonoBehaviour
{
    public GameObject p1Hint;
    public ColorBlock p1;

    private WorldColor playerColor = WorldColor.Black;
    private PlayerMovement movement;
    private SpriteRenderer playerPicture;
    private ColorBlock[] blocks;
    private int reachedPlatform = 0;
    private int switchCount = 0;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        playerPicture = GetComponent<SpriteRenderer>();
        blocks = FindObjectsByType<ColorBlock>(FindObjectsSortMode.None);

        RefreshWorld();
    }

    private void Update()
    {
        ColorBlock standingOn = movement.GetPlatform();

        if (standingOn != null && standingOn.order > reachedPlatform)
        {
            reachedPlatform = standingOn.order;
            RefreshWorld();
        }

        // No platform underfoot means no switching.
        if (!Input.GetKeyDown(KeyCode.LeftShift) || standingOn == null)
            return;

        playerColor = playerColor == WorldColor.Black
            ? WorldColor.White
            : WorldColor.Black;

        switchCount++;

            standingOn.BeginBreak(movement);

        RefreshWorld();
    }

    private void RefreshWorld()
    {
        playerPicture.color = playerColor == WorldColor.Black
            ? Color.black
            : Color.white;

        foreach (ColorBlock block in blocks)
        {
            bool closeEnough =
                block.order == -1 ||
                (block.order >= reachedPlatform &&
                 block.order <= reachedPlatform + 2);

            bool visible = block.blockColor == playerColor &&
                           (!block.isPlatform || closeEnough);

            block.Show(visible);
        }

        if (p1Hint != null && p1 != null)
            p1Hint.SetActive(switchCount < 3 && !p1.IsVisible);
    }

    private void OnGUI()
    {
        if (switchCount > 0) return;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        GUI.Label(
            new Rect(0, 25, Screen.width, 45),
            "PRESS LEFT SHIFT TO SWITCH COLORS",
            style
        );
    }
}