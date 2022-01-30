using UnityEngine;
using UnityEngine.SceneManagement;

public class winScreem : MonoBehaviour
{
    public static bool gameIsWinned = false;
    public bool canGo = false;

    private void Update()
    {
        if (canGo && Input.anyKeyDown)
        {
            enabled = false;
            SceneManager.LoadSceneAsync(0);
        }
    }
}
