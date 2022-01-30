using UnityEngine;
using UnityEngine.UI;

public class hudLife : MonoBehaviour
{
    public Image[] imageLifesArray = new Image[3];
    public Sprite spriteWithLife, spriteWithoutLife;
    private int lifes;

    // Start is called before the first frame update
    void Start()
    {
        lifes = imageLifesArray.Length;
    }

   public void AttLifesInfs(int lifeNow)
    {
        lifes = lifeNow;

        for(int i = 0; i < imageLifesArray.Length; i++)
        {
            if (i < lifes)
            {
                imageLifesArray[i].sprite = spriteWithLife;
            }
            else
            {
                imageLifesArray[i].sprite = spriteWithoutLife;
            }
        }
    }
}
