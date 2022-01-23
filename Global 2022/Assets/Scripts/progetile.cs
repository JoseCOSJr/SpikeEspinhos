using UnityEngine;

public class progetile : powers
{
    public float velocity;
    private SpriteRenderer rendererX;
    private float countInvisible;

    private void Awake()
    {
        rendererX = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * velocity;
        countInvisible = 0f;
    }

    private void FixedUpdate()
    {
        if (rendererX.isVisible)
        {
            countInvisible = 0f;
        }
        else
        {
            countInvisible += Time.fixedDeltaTime;
            if (countInvisible < 5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
   
