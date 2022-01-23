using UnityEngine;

public class controllMovement : MonoBehaviour
{
    public actions actionSpike, actionSpindDash, actionSpikes360;
    private movement2D movement;
    public LayerMask maskPush;
    public Transform transformPushPos;
    private Vector3 posOffsetAux;
    private Collider2D collPush = null;
    public float pushVelocity = 2f;
    private actionControll actionC;
    private float countPressJumpTime = 0f, coutAuxTimeJump = 0f, countPressAction = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<movement2D>();
        actionC = GetComponent<actionControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coutAuxTimeJump > 0f)
        {
            coutAuxTimeJump -= Time.deltaTime;
        }

        Vector2 inputMv = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButton("Action 1"))
        {
            countPressAction += Time.deltaTime;
            if (countPressAction > 0.5f)
            {
                actionC.TryAction(actionSpikes360, inputMv);
                countPressAction = -0.5f;
            }
        }
        else if (Input.GetButtonUp("Action 1"))
        {
            if (countPressAction > 0f)
            {
                actionC.TryAction(actionSpike, inputMv);
            }
            countPressAction = 0f;
        }
        else
        {
            countPressAction = 0f;
        }

        if (collPush)
        {
            Vector2 mv = Quaternion.Euler(Vector3.forward * movement.AngGround()) * Vector2.right * inputMv.x;
            movement.GetRigidbody2D().velocity = mv * pushVelocity;
            collPush.transform.localPosition = posOffsetAux;
        }
        else
        {
            //Debug.Log(inputMv);
            movement.MakeMove(inputMv);

            if (Input.GetButton("Jump"))
            {
                countPressJumpTime += Time.deltaTime;
                if (countPressJumpTime >= 0.5f)
                {
                    movement.MakeJump();
                    countPressJumpTime = 0f;
                    coutAuxTimeJump = 0.5f;
                }
            }
            else if(countPressJumpTime > 0f)
            {
                if (Input.GetButtonUp("Jump") && coutAuxTimeJump <= 0f)
                {
                    float forceJump = countPressJumpTime + 0.5f;
                    forceJump = Mathf.Sqrt(forceJump);
                    movement.MakeJump(forceJump);
                    coutAuxTimeJump = 0.2f;
                }
                countPressJumpTime = 0f;
            }
        }

        if (Input.GetButton("Push") && movement.DistancieGround() == 0f && actionC.GetCanAction())
        {
            if (!collPush)
            {
                Bounds b = movement.GetCollider().bounds;
                RaycastHit2D hit2d = Physics2D.BoxCast(b.center, b.size / 2f, movement.AngGround(), transform.right, 0.5f, maskPush);

                if (hit2d.collider)
                {
                    collPush = hit2d.collider;
                    collPush.transform.SetParent(transformPushPos);
                    Physics2D.IgnoreCollision(movement.GetCollider(), collPush);
                    movement.SetCanMove(false);
                    posOffsetAux = collPush.transform.localPosition;
                }
            }
        }
        else if(collPush)
        {
            Physics2D.IgnoreCollision(movement.GetCollider(), collPush, false);
            collPush.transform.SetParent(null);
            collPush = null;
            movement.SetCanMove(true);
        }
    }
}
