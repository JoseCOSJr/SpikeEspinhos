using UnityEngine;

public class iaDistAction : MonoBehaviour
{
    public actions action;
    public float dist = 1f;
    public LayerMask maskHit;
    private Collider2D coll;
    private actionControll actionC;
    public bool needFront = false;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        actionC = GetComponent<actionControll>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] collsArray = Physics2D.OverlapCircleAll(coll.bounds.center, dist, maskHit);

        for(int i = 0; i < collsArray.Length; i++)
        {
            bool infront;

            if(transform.eulerAngles.y == 0f)
            {
                infront = transform.position.x <= collsArray[i].transform.position.x;
            }
            else
            {
                infront = transform.position.x >= collsArray[i].transform.position.x;
            }

            if (!collsArray[i].CompareTag(tag) && (!needFront || infront))
            {
                Vector2 dire = Vector2.right;

                if(transform.position.x > collsArray[i].transform.position.x)
                {
                    dire *= -1f;
                }

                actionC.TryAction(action, dire);
            }
        }
    }
}
