using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSections : MonoBehaviour
{
    public Transform player;
    public GameObject startMenu;
    public bool testMenuInEditor = false;

    public float overviewStartX = 0f;
    public float overviewEndX = 52f;

    private Camera gameCamera;
    private bool playing;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        Time.timeScale = 1f;

        bool showMenu = testMenuInEditor;

#if UNITY_WEBGL && !UNITY_EDITOR
        showMenu = true;
#endif

        bool skipOverview = GameRestart.TakeSkipOverview();
        showMenu = showMenu && startMenu != null && !skipOverview;

        if (showMenu)
        {
            playing = false;
            startMenu.SetActive(true);

            float middleX = (overviewStartX + overviewEndX) / 2f;
            transform.position = new Vector3(middleX, 0.8f, -10f);
            gameCamera.orthographicSize =
                (overviewEndX - overviewStartX + 4f) / (2f * gameCamera.aspect);

            Time.timeScale = 0f;
        }
        else
        {
            playing = true;
            if (startMenu != null) startMenu.SetActive(false);
            gameCamera.orthographicSize = 4.5f;
        }
    }

    public void PlayGame()
    {
        playing = true;
        Time.timeScale = 1f;
        if (startMenu != null) startMenu.SetActive(false);
    }

    private void LateUpdate()
    {
        if (!playing || player == null) return;

        float x = 4.5f;

        if (player.position.x >= 44f) x = 48.5f;
        else if (player.position.x >= 34.5f) x = 40.5f;
        else if (player.position.x >= 28f) x = 29.5f;
        else if (player.position.x >= 19f) x = 24f;
        else if (player.position.x >= 9f) x = 14f;

        float y = Mathf.Clamp(player.position.y - 1.7f, 0.8f, 5f);
        float speed = Mathf.Clamp01(5f * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(x, y, -10f),
            speed
        );

        gameCamera.orthographicSize =
            Mathf.Lerp(gameCamera.orthographicSize, 4.5f, speed);
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}