using UnityEngine;

public class triggerAnimaEvents : MonoBehaviour
{
    public void ResetHitstun()
    {
        GetComponentInParent<hitstun>().ResetState();
    }

    public void ResetAction()
    {
        GetComponentInParent<actionControll>().ResetAction();
    }

    public void MakePowerSkill()
    {
        GetComponentInParent<actionControll>().RespawPower();
    }
}
