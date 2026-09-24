using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private bool completed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<PlayerMovement>() == null)
            return;

        completed = true;
        Debug.Log("Level complete!");
    }

    private void OnGUI()
    {
        if (!completed) return;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 32;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        GUI.Label(
            new Rect(0, Screen.height / 2f - 40f, Screen.width, 80f),
            "LEVEL COMPLETE",
            style
        );
    }
}