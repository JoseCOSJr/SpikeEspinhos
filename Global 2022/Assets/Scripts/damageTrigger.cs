using UnityEngine;

public class damageTrigger : MonoBehaviour
{
    public int damage = 1;
    public bool hitstunAtived = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !collision.CompareTag(gameObject.tag))
        {
            if (damage != 0)
            {
                life lfObj = collision.GetComponentInParent<life>();

                if (lfObj)
                {
                    lfObj.AddLife(-damage);
                }
            }

            if (hitstunAtived)
            {
                hitstun htScript = collision.GetComponentInParent<hitstun>();
                
                if (htScript)
                {
                    htScript.TriggerHitstun();
                }
            }
        }
    }
}
