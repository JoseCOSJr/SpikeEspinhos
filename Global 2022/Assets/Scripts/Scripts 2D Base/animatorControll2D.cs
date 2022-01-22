using UnityEngine;

public class animatorControll2D : MonoBehaviour
{
    private Animator animator;
    private bool resetTimeTrigger = false;
    private string animaNow = "Idle";

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        //Debug.Log(animator.GetFloat("Agility"));
    }

    private void SetAnimation(string idAnimation, bool resetTime)
    {
        if (animator.enabled && gameObject.activeInHierarchy && idAnimation != "")
        {
            float timeN = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            if (resetTime || resetTimeTrigger)
            {
                timeN = 0f;
                if (!resetTimeTrigger)
                {
                    resetTimeTrigger = true;
                }
            }

            if (timeN > 1f)
            {
                timeN = timeN - Mathf.Floor(timeN);
            }

            //if(!normalState)
            //Debug.Log(idAnimation+" n "+timeN);
            animaNow = idAnimation;
            animator.Play(idAnimation, 0, timeN);
        }
    }

    private void LateUpdate()
    {
        resetTimeTrigger = false;
    }

    public string GetIdAnimaNow()
    {
        return animaNow;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
    public void SetActionAnimation(string id, bool repeat, bool resetTime = true)
    {
        if (id != animaNow || repeat)
        {
            //Debug.Log(id);
            SetAnimation(id, resetTime);
        }
    }
}
