using UnityEngine;

public class damageTrigger : MonoBehaviour
{
    public int damage = 1;
    public bool hitstunAtived = true;
    public Vector2 forcePush = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && !collision.CompareTag(gameObject.tag))
        {
            bool inv = false;
            life lfObj = collision.GetComponentInParent<life>();

            if (damage != 0)
            {
                if (lfObj)
                {
                    inv = lfObj.GetInvulnerable();
                    lfObj.AddLife(-damage);
                }
            }

            if (hitstunAtived && !inv)
            {
                hitstun htScript = collision.GetComponentInParent<hitstun>();
                
                if (htScript)
                {
                    htScript.TriggerHitstun();
                    lfObj.SetInvulnerable();
                }
            }

            if(forcePush != Vector2.zero && !inv)
            {
                Vector2 pushX = forcePush;
                if (transform.eulerAngles.y == 180f)
                {
                    pushX.x *= -1f;
                }

                movement2D mv = collision.GetComponentInParent<movement2D>();
                if (mv)
                {
                    mv.AddForceInBody(pushX, 0f);
                }
            }
        }
    }
}
