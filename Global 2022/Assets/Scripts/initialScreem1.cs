using UnityEngine;
using UnityEngine.SceneManagement;


public class initialScreem1 : MonoBehaviour
{
    public GameObject objCredits;
    public AudioClip clipClick;
    private AudioSource audioSourceX;

    private void Start()
    {
        audioSourceX = GetComponent<AudioSource>();
    }

    public void ButtonCredits()
    {
        if (enabled)
        {
            audioSourceX.PlayOneShot(clipClick);
            objCredits.SetActive(true);
        }
    }

    public void ButtonExitCredits()
    {
        if (enabled)
        {
            audioSourceX.PlayOneShot(clipClick);
            objCredits.SetActive(false);
        }
    }

    public void ButtonGoStage()
    {
        if (enabled)
        {
            audioSourceX.PlayOneShot(clipClick);
            SceneManager.LoadSceneAsync(1);
            enabled = false;
        }

    }
}
