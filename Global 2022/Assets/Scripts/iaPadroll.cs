using UnityEngine;

public class iaPadroll : MonoBehaviour
{
    public float distPatroll = 5f;
    private float posXMax, posXMin;
    private bool rightSide;
    private float countTimeWait = 0f;
    private movement2D movement;

    private void Awake()
    {
        movement = GetComponent<movement2D>();
        posXMax = transform.position.x + distPatroll;
        posXMin = transform.position.x - distPatroll;
        rightSide = Random.value > 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (countTimeWait > 0f)
        {
            countTimeWait -= Time.deltaTime;
            if (countTimeWait <= 0f)
            {
                rightSide = !rightSide;
            }
        }
        else
        {
            if (rightSide)
            {
                if (transform.position.x < posXMax && !movement.GetObstacle(Vector2.right))
                {
                    Vector2 move = Vector2.right;
                    movement.MakeMove(move);
                }
                else
                {
                    movement.StopMove();
                    countTimeWait = 3f;
                }
            }
            else
            {
                if (transform.position.x > posXMin  && !movement.GetObstacle(Vector2.left))
                {
                    Vector2 move = Vector2.left;
                    movement.MakeMove(move);
                }
                else
                {
                    movement.StopMove();
                    countTimeWait = 3f;
                }
            }
        }
    }
}
