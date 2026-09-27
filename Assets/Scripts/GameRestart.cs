using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public static bool SkipOverviewOnce { get; private set; }

    public void RestartGame()
    {
        SkipOverviewOnce = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static bool TakeSkipOverview()
    {
        bool skip = SkipOverviewOnce;
        SkipOverviewOnce = false;
        return skip;
    }
}