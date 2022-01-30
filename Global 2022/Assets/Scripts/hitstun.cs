using UnityEngine;

public class hitstun : MonoBehaviour
{
    private animatorControll2D animatorC;
    public string animaHitstun;

    // Start is called before the first frame update
    void Awake()
    {
        animatorC = GetComponent<animatorControll2D>();
    }

    public void TriggerHitstun()
    {
        movement2D mv = GetComponent<movement2D>();
        mv.SetCanMove(false);
        mv.SetActivedAnimations(false);
        GetComponent<actionControll>().SetCanAction(false);
        if (animatorC)
        {
            animatorC.SetActionAnimation(animaHitstun, true);
        }
    }

    public void ResetState()
    {
        movement2D mv = GetComponent<movement2D>();
        mv.SetCanMove(true);
        mv.SetActivedAnimations(true);
        GetComponent<actionControll>().SetCanAction(true);
    }
}
