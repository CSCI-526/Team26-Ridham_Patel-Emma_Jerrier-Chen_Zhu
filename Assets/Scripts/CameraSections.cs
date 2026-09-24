using UnityEngine;

public class CameraSections : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        if (player == null) return;

        float x = 4.5f;

        if (player.position.x >= 44f) x = 48.5f;
        else if (player.position.x >= 34.5f) x = 40.5f;
        else if (player.position.x >= 28f) x = 29.5f;
        else if (player.position.x >= 19f) x = 24f;
        else if (player.position.x >= 9f) x = 14f;

        Vector3 destination = new Vector3(x, 0.8f, -10f);
        transform.position = Vector3.Lerp(
            transform.position, destination, 5f * Time.deltaTime
        );
    }
}