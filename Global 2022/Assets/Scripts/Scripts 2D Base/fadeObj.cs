using UnityEngine;
using UnityEngine.UI;

public class fadeObj : MonoBehaviour
{
    public static fadeObj fadeObjInScene = null;
    private Image imageFade;
    private float countTime, speedFade;
    private bool fadeIn;

    private void Awake()
    {
        fadeObjInScene = this;
        imageFade = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void FadeGo(bool fadeIn, float duration)
    {
        countTime = 0f;
        speedFade = 1f / duration;
        this.fadeIn = fadeIn;
        gameObject.SetActive(true);

        if (fadeIn)
        {
            imageFade.color = Color.black;
        }
        else
        {
            imageFade.color = Color.clear;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (countTime < 1f)
        {
            countTime += Time.fixedDeltaTime * speedFade;
            Color colorAux = Color.Lerp(Color.clear, Color.black, countTime);
            if (fadeIn)
            {
                colorAux = Color.Lerp(Color.black, Color.clear, countTime);
            }
            imageFade.color = colorAux;
        }
        else if(fadeIn)
        {
            gameObject.SetActive(false);
        }
    }

    public bool FadeEventIsFinish()
    {
        return countTime >= 1f;
    }
}
