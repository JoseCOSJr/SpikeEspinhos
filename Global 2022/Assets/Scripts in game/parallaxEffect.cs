using UnityEngine;

public class parallaxEffect : MonoBehaviour
{
    public Vector2 effectParallax;
    public Vector2 moveVector2 = Vector2.zero;
    //[SerializeField]
    private Vector2 size;
    private float duration = 0f;
    private Vector2 posInititial;
    public bool repetionPosX = true, repetionPosY = false;

    // Start is called before the first frame update
    void Start()
    {
        size = GetComponent<SpriteRenderer>().bounds.size;
        posInititial = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 posLocalFinal = transform.localPosition;
        float repetition;
        duration += Time.deltaTime;

        if (effectParallax.x != 0f || moveVector2.x != 0f)
        {
            float dist = Camera.main.transform.position.x * -effectParallax.x + moveVector2.x * duration;

            if (repetionPosX)
            {
                repetition = dist / (size.x / 3f);
                posLocalFinal.x = (repetition - Mathf.FloorToInt(repetition)) * size.x / 3f + posInititial.x;
            }
            else
            {
                posLocalFinal.x = dist + posInititial.x;
            }
        }

        if (effectParallax.y != 0f || moveVector2.y != 0f)
        {
            float dist = Camera.main.transform.position.y * -effectParallax.y + moveVector2.y * duration;

            if (repetionPosY)
            {
                repetition = dist / (size.y / 3f);
                posLocalFinal.y = (repetition - Mathf.FloorToInt(repetition)) * size.y / 3f + posInititial.y;
            }
            else
            {
                posLocalFinal.y = dist + posInititial.y;
            }
        }

        transform.localPosition = posLocalFinal;
    }
}
