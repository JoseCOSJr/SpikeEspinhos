using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class collectionChar : MonoBehaviour
{
    public Text finishPorcentText;
    public static int allScores = 0;
    private static int scores = 0;
    public Volume volumeModifie;
    public endWinScreem winScreem;
    public GameObject objJump;

    // Start is called before the first frame update
    void Awake()
    {
        objJump.SetActive(false);
        allScores = 0;
        scores = 0;
        volumeModifie.weight = 1f;
        finishPorcentText.text = "0%";
        endWinScreem.winGame = false;
    }

    private void Start()
    {
        stageMusic.stageMusicX.GetAudioSource().volume = 0.2f;
        repository.volumeGeral = 0.2f;
    }

    public void AddScores(int add)
    {
        scores += add;
        float percent = (float)scores / (0.8f*allScores);
        if (percent > 1f)
        {
            percent = 1f;
        }
        volumeModifie.weight = 1f - percent;
        int percentTextInt = Mathf.FloorToInt(percent*100f);
        finishPorcentText.text = percentTextInt + "%";

        int idMusic = Mathf.FloorToInt(percent * stageMusic.stageMusicX.clips.Length);
        stageMusic.stageMusicX.SetIdMusic(idMusic);

        if (percent >= 1f)
        {
            winScreem.gameObject.SetActive(true);
            endWinScreem.winGame = true;
        }

        if (percent >= 0.25f)
        {
            movement2D mv = GetComponent<movement2D>();
            if (mv.jumpInAir != 1)
            {
                mv.jumpInAir = 1;
                objJump.SetActive(true);
            }
        }

        float vlm = 0.25f + percent * 0.75f;
        repository.volumeGeral = vlm;
        stageMusic.stageMusicX.GetAudioSource().volume = vlm;

        //Debug.Log(allScores * 0.9f);
    }

    public float GetPercent()
    {
        float percent = (float)scores / (0.8f * allScores);
        if (percent > 1f)
        {
            percent = 1f;
        }

        return percent;
    }
}
