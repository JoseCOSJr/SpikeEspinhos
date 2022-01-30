using UnityEngine;
using UnityEngine.SceneManagement;

public class endWinScreem : MonoBehaviour
{
    public static bool winGame = false;
    public bool canSkip = false;

    // Update is called once per frame
    void Update()
    {
        if(canSkip && Input.anyKeyDown)
        {
            //GetComponent<Animator>().SetTrigger("Exit");
            ResetStage();
        }
    }

    public void ResetStage()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
