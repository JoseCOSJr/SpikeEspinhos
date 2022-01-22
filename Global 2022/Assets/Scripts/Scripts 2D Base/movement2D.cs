using UnityEngine;

public class movement2D : MonoBehaviour
{
    private static float powerJump = 0f;
    private float ghostJump = 0.1f;
    private bool canMakeMovement = true;
    private float multSpeed = 1f;
    //[SerializeField]
    private bool canJump = true, canTurn = true;
    [SerializeField]
    private float maxVelocity = 5;
    private Vector2 direSpeedGo = Vector2.zero;
    private Rigidbody2D body2D;
    private Collider2D coll2D = null, collAux = null;
    private bool flying = false;
    private float jumpingCount = 0f;
    //[SerializeField]
    private bool obstacleDown = true, obstacleUp = false, obstacleRight = false, obstacleLeft = false;
    private animatorControll2D animatorControll;
    //[SerializeField]
    private float angZ = 0f;
    private Vector2 oldVelocity = Vector2.zero, goToVelocity;
    public enum changeVelocity { keep, wait, swicht};
    private changeVelocity stateVelocity = changeVelocity.keep;
    private float timeMoveAgain = 0f;
    private Collider2D colliderIgnore = null, colliderGround = null;
    [Header("Animations")]
    public string idleAnimation, walkAnimation, jumpAnimation, fallAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        if (powerJump !=Physics2D.gravity.y/-2f)
        {
            powerJump = Physics2D.gravity.y / -2f;
        }

        body2D = GetComponent<Rigidbody2D>();
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        coll2D = GetComponent<Collider2D>();
        animatorControll = GetComponent<animatorControll2D>();
    }

    private void OnEnable()
    {
        timeMoveAgain = 0f;
    }

    public void SetCollAux(Collider2D coll)
    {
        collAux = coll;
    }

    public bool GetObstacle(Vector2 dire)
    {
        if (dire.x > 0f)
        {
            return obstacleRight;
        }
        else if (dire.x < 0f)
        {
            return obstacleLeft;
        }
        else if (dire.y > 0f)
        {
            return obstacleUp;
        }

        return obstacleDown;
    }

    public float GetMaxVelocityNow()
    {
        return multSpeed * maxVelocity * animatorControll.GetAnimator().speed;
    }

    public animatorControll2D GetAnimaControll()
    {
        return animatorControll;
    }

    public Collider2D GetCollider()
    {       
        return coll2D;
    }

    public void SetMultSpeed(float value)
    {
        multSpeed = value;
    }

    public void SetCanJump(bool can)
    {
        canJump = can;
    }

    public void SetCanTurn(bool can)
    {
        canTurn = can;
    }

    public bool GetCanMove()
    {
        return canMakeMovement && multSpeed != 0f;
    }

    public void SetCanMove(bool can)
    {
        /*if (CompareTag("Player"))
            Debug.Log(can+" "+name);*/
        canMakeMovement = can;
        if (!can)
        {
            timeMoveAgain = 0f;
            body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void SetGravity(float value)
    {
        body2D.gravityScale = value;
        if (value == 0f)
        {
            body2D.velocity = Vector2.right * body2D.velocity.x;
        }
    }

    private void CheckObstacles(ContactPoint2D point2D)
    {
        if (!obstacleDown)
        {
            obstacleDown = point2D.normal.y > 0.75f;
        }

        if (!obstacleLeft)
        {
            obstacleLeft = point2D.normal.x > 0.75f;
        }

        if (!obstacleRight)
        {
            obstacleRight = point2D.normal.x < -0.75f;
        }

        if (!obstacleUp)
        {
            obstacleUp = point2D.normal.y < -0.75f;
        }
    }

    public changeVelocity GetChangeVelocity()
    {
        return stateVelocity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        /*if (timeJumpDenfecive > 0f)
        {
            if (attributeX.GetInvulnerable())
            {
                timeJumpDenfecive -= Time.fixedDeltaTime * 2f;
                if (timeJumpDenfecive <= 0f)
                {
                    attributeX.SetInvulnerable(false);
                }
            }
            else
            {
                timeJumpDenfecive = 0f;
            }
        }*/

        if (timeMoveAgain > 0f)
        {
            if (canMakeMovement)
            {
                timeMoveAgain = 0f;
            }
            else
            {
                timeMoveAgain -= Time.fixedDeltaTime;
                if (timeMoveAgain <= 0f)
                {
                    canMakeMovement = true;
                }
            }
        }

        if (stateVelocity == changeVelocity.wait)
        {
            stateVelocity = changeVelocity.swicht;
            if (goToVelocity.y > 0f)
            {
                if (obstacleRight || obstacleLeft)
                {
                    if (obstacleRight)
                    {
                        transform.position += Vector3.left / 16f;
                    }
                    else if (obstacleLeft)
                    {
                        transform.position += Vector3.right / 16f;
                    }
                }
            }
        }else if(stateVelocity == changeVelocity.swicht)
        {
            stateVelocity = changeVelocity.keep;
            body2D.velocity = goToVelocity;
            //Debug.Log(goToVelocity);
        }

        oldVelocity = body2D.velocity;

        ContactPoint2D[] contactPoints = new ContactPoint2D[8];
        int maxContact = coll2D.GetContacts(contactPoints);
        obstacleDown = false;
        obstacleLeft = false;
        obstacleRight = false;
        obstacleUp = false;
        for(int i=0;i<maxContact; i++)
        {
            PlatformEffector2D effector2D = contactPoints[i].collider.GetComponent<PlatformEffector2D>();

            bool dontCount = false;
            if (effector2D)
            {
                float angSurfaceAux = effector2D.surfaceArc / 2f;
                Vector2 normalEffector = Quaternion.Euler(effector2D.transform.eulerAngles + Vector3.forward * (effector2D.sideArc+effector2D.rotationalOffset)) * Vector2.up;
                float angDif = Vector2.SignedAngle(normalEffector, contactPoints[i].normal);

                //Debug.Log(angDif+" angX " + normalEffector+" n "+contactPoints[i].normal);

                Bounds boundsHere = coll2D.bounds;
                Vector2 vector2Borders = (Vector2)boundsHere.center + new Vector2(-normalEffector.x * boundsHere.extents.x, boundsHere.extents.y * -normalEffector.y);

                bool passed = true;
                float offeset = Physics2D.defaultContactOffset;
                if (body2D.velocity.y != 0f /*&& !animatorControll.GetInGround()*/)
                {
                    offeset *= -2f;
                }

                if (normalEffector.x*normalEffector.x > 0.02f*0.02f)
                {
                    if (normalEffector.x > 0f)
                    {
                        if (vector2Borders.x+offeset >= contactPoints[i].point.x)
                        {
                            passed = true;
                        }
                        else
                        {
                            passed = false;
                        }
                    }
                    else if (vector2Borders.x - offeset <= contactPoints[i].point.x)
                    {
                        passed = true;
                    }
                    else
                    {
                        passed = false;
                    }
                }

                //Debug.Log(passed + " nX " + normalEffector.x * 100f); ;
                if (normalEffector.y*normalEffector.y > 0.02f * 0.02f && passed)
                {
                    if (normalEffector.y > 0f)
                    {
                        if (vector2Borders.y + offeset < contactPoints[i].point.y)
                        {
                            passed = false;
                        }
                    }
                    else if (vector2Borders.y - offeset > contactPoints[i].point.y)
                    {
                        passed = false;
                    }
                }

                //Debug.Log(vector2Borders+" contacs "+contactPoints[i].point+" passed "+passed+" n "+normalEffector);
                if (angDif * angDif > angSurfaceAux * angSurfaceAux || !passed || (/*!animatorControll.GetInGround() &&*/ body2D.velocity.y != 0f))
                {
                    dontCount = true;
                    //Debug.Log(angDif+" ang "+normalEffector);
                }
            }

            if (!dontCount)
            {
                CheckObstacles(contactPoints[i]);

                if(contactPoints[i].normal.y > 0f)
                {
                    colliderGround = contactPoints[i].collider;
                }
            }
        }

        if (!obstacleLeft)
        {
            RaycastHit2D ray2D = Physics2D.BoxCast(coll2D.bounds.center, coll2D.bounds.size, 0f, Vector2.left, 0.05f, repository.repositoryX.GetLayerObst());
            obstacleLeft = ray2D.collider && ray2D.normal.x > 0.75f;

            if (obstacleLeft)
            {
                PlatformEffector2D effector2D = ray2D.collider.GetComponent<PlatformEffector2D>();
                if (effector2D)
                {
                    Vector2 normalEffector = Quaternion.Euler(effector2D.transform.eulerAngles + Vector3.forward * (effector2D.sideArc + effector2D.rotationalOffset)) * Vector2.up;
                    float angDif = Vector2.SignedAngle(normalEffector, ray2D.normal);
                    float angSurfaceAux = effector2D.surfaceArc / 2f;

                    bool passed = true;
                    float offeset = Physics2D.defaultContactOffset;

                    Bounds boundsHere = coll2D.bounds;
                    Vector2 vector2Borders = (Vector2)boundsHere.center + new Vector2(-normalEffector.x * boundsHere.extents.x, boundsHere.extents.y * -normalEffector.y);


                    if (normalEffector.x * normalEffector.x > 0.02f * 0.02f)
                    {
                        if (normalEffector.x > 0f)
                        {
                            if (vector2Borders.x + offeset >= ray2D.point.x)
                            {
                                passed = true;
                            }
                            else
                            {
                                passed = false;
                            }
                        }
                        else if (vector2Borders.x - offeset <= ray2D.point.x)
                        {
                            passed = true;
                        }
                        else
                        {
                            passed = false;
                        }
                    }

                    obstacleLeft = passed && angDif * angDif <= angSurfaceAux * angSurfaceAux;
                }
            }
        }

        if (!obstacleRight)
        {
            RaycastHit2D ray2D = Physics2D.BoxCast(coll2D.bounds.center, coll2D.bounds.size, 0f, Vector2.right, 0.05f, repository.repositoryX.GetLayerObst());
            obstacleRight = ray2D.collider && ray2D.normal.x < -0.75f;

            if (obstacleRight)
            {
                PlatformEffector2D effector2D = ray2D.collider.GetComponent<PlatformEffector2D>();
                if (effector2D)
                {
                    Vector2 normalEffector = Quaternion.Euler(effector2D.transform.eulerAngles + Vector3.forward * (effector2D.sideArc + effector2D.rotationalOffset)) * Vector2.up;
                    float angDif = Vector2.SignedAngle(normalEffector, ray2D.normal);
                    float angSurfaceAux = effector2D.surfaceArc / 2f;

                    bool passed = true;
                    float offeset = Physics2D.defaultContactOffset;

                    Bounds boundsHere = coll2D.bounds;
                    Vector2 vector2Borders = (Vector2)boundsHere.center + new Vector2(-normalEffector.x * boundsHere.extents.x, boundsHere.extents.y * -normalEffector.y);


                    if (normalEffector.x * normalEffector.x > 0.02f * 0.02f)
                    {
                        if (normalEffector.x > 0f)
                        {
                            if (vector2Borders.x + offeset >= ray2D.point.x)
                            {
                                passed = true;
                            }
                            else
                            {
                                passed = false;
                            }
                        }
                        else if (vector2Borders.x - offeset <= ray2D.point.x)
                        {
                            passed = true;
                        }
                        else
                        {
                            passed = false;
                        }
                    }

                    obstacleRight = passed && angDif * angDif <= angSurfaceAux * angSurfaceAux;
                }
            }
        }

        if (collAux && collAux.enabled)
        {
            contactPoints = new ContactPoint2D[8];
            maxContact = collAux.GetContacts(contactPoints);

            for (int i = 0; i < maxContact; i++)
            {
                PlatformEffector2D effector2D = contactPoints[i].collider.GetComponent<PlatformEffector2D>();

                bool dontCount = false;
                if (effector2D)
                {
                    Vector2 v2Aux = Quaternion.Euler(Vector3.forward * effector2D.rotationalOffset) * Vector2.up;
                    float ang = Vector2.SignedAngle(v2Aux, contactPoints[i].normal), angSurface = effector2D.surfaceArc / 2f;
                    if (ang * ang >= angSurface * angSurface)
                    {
                        dontCount = true;
                    }
                }

                if (!dontCount)
                {
                    CheckObstacles(contactPoints[i]);
                }
            }
        }

        if (colliderIgnore && obstacleDown)
        {
            Physics2D.IgnoreCollision(coll2D, colliderIgnore, false);
            colliderIgnore = null;
        }

        if (canMakeMovement)
        {         
            if (coll2D.sharedMaterial != repository.repositoryX.physicsMaterialCharaterDafault)
            {
                coll2D.sharedMaterial = repository.repositoryX.physicsMaterialCharaterDafault;
            }
            //Debug.Log(direSpeedGo);
            Vector2 veloAux = direSpeedGo;
            if (obstacleDown)
            {
                veloAux = Quaternion.Euler(Vector3.forward * angZ) * veloAux;
                //Debug.Log(veloAux);
                if (transform.eulerAngles.y == 180f)
                {
                    veloAux.y *= -1f;
                }
            }
            //Debug.Log(direSpeedGo);
            if (direSpeedGo.sqrMagnitude > 0f)
            {
                Turn(direSpeedGo.x > 0f);

                if (flying)
                {
                    body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
                else
                {
                    if (obstacleDown && jumpingCount <= 0f)
                    {
                        if (veloAux.y != 0f)
                        {
                            body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        }
                        else
                        {
                            body2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
                        }
                    }
                    else
                    {                 
                        body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                        if (veloAux.x > 0f)
                        {
                            if (obstacleRight)
                            {
                                veloAux.x = 0f;
                            }
                        }
                        else if(veloAux.x < 0f)
                        {
                            if (obstacleLeft)
                            {
                                veloAux.x = 0f;
                            }
                        }
                        veloAux.y = body2D.velocity.y;
                    }
                }
            }
            else
            {
                if (flying)
                {
                    body2D.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                else
                {
                    if (obstacleDown && jumpingCount <= 0f)
                    {
                        body2D.constraints = RigidbodyConstraints2D.FreezeAll;
                    }
                    else
                    {
                        body2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                        veloAux.y = body2D.velocity.y;
                    }
                }
            }
            //Debug.Log(veloAux);
            body2D.velocity = veloAux;
        }
        else if (coll2D.sharedMaterial != repository.repositoryX.physicsMaterialDontMove)
        {
            coll2D.sharedMaterial = repository.repositoryX.physicsMaterialDontMove;
        }

        if (jumpingCount > 0f)
        {
            jumpingCount -= Time.fixedDeltaTime;
        }

        if (ghostJump > 0f)
        {
            ghostJump -= Time.fixedDeltaTime;
        }

        if (obstacleDown)
        {
            if (ghostJump != 0.1f)
            {
                ghostJump = 0.1f;
            }
            //Calcular angulo de movimento
            Vector2 vector2Aux = coll2D.bounds.center + Vector3.down*coll2D.bounds.extents.y, vector2Side = Vector2.right;
            Vector2 posOrig = coll2D.bounds.center + Vector3.down * (coll2D.bounds.extents.y - 0.5f);
            if (transform.eulerAngles.y == 0f)
            {
                posOrig += Vector2.right * coll2D.bounds.extents.x;
            }
            else
            {
                posOrig -= Vector2.right * coll2D.bounds.extents.x;
                vector2Side *= -1f;
            }

            Vector2 vector2Aux2 = coll2D.bounds.center + Vector3.down * coll2D.bounds.extents.y;
            RaycastHit2D[] hits2D = Physics2D.RaycastAll(posOrig, Vector2.down, 1.5f, repository.repositoryX.GetLayerMaskGround());
            for(int i=0; i<hits2D.Length; i++)
            {
                if (i == 0)
                {
                    vector2Aux2 = hits2D[i].point;
                }
                else
                {
                    float deltaYDist1 = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * vector2Aux2.y);
                    float deltaYDist2 = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * hits2D[i].point.y);

                    if (deltaYDist1 > deltaYDist2)
                    {
                        vector2Aux2 = hits2D[i].point;
                    }
                }
            }

            angZ = Vector2.SignedAngle(vector2Side, vector2Aux2 - vector2Aux);
            if (transform.eulerAngles.y == 180f)
            {
                angZ *= -1f;
            }
        }
        else if(angZ != 0f)
        {
            angZ = 0f;
        }

        if(DistancieGround() > 0f && colliderGround)
        {
            colliderGround = null;
        }
    }

    private void LateUpdate()
    {
        if (obstacleDown || ghostJump > 0f)
        {
            if (body2D.velocity.x == 0f)
            {
                animatorControll.SetActionAnimation(idleAnimation, false);
            }
            else
            {
                animatorControll.SetActionAnimation(walkAnimation, false);
            }
            /*animatorControll.NotInGround(body2D.velocity.y <= 0f, true);
            animatorControll.WalkAnimation(body2D.velocity.sqrMagnitude > 0f && direSpeedGo.sqrMagnitude > 0f);*/
        }
        else
        {
            if (body2D.velocity.y > 0f)
            {
                animatorControll.SetActionAnimation(jumpAnimation, false);
            }
            else
            {
                animatorControll.SetActionAnimation(fallAnimation, false);
            }
            //animatorControll.NotInGround(body2D.velocity.y <= 0f, false);
        }
    }

    public void MakeMove(Vector2 dire)
    {
        if (canMakeMovement)
        {
            if (!flying)
            {
                if (dire.y < 0f)
                {
                    GoingDown();
                }
                dire.y = 0f;
            }
            dire.Normalize();

            if (dire.x > 0f && ((!canTurn && transform.eulerAngles.y == 180f) || obstacleRight))
            {
                dire.x = 0f;
            }
            
            if (dire.x < 0f && ((!canTurn && transform.eulerAngles.y == 0f) || obstacleLeft))
            {
                dire.x = 0f;
            }

            direSpeedGo = dire * maxVelocity * multSpeed * animatorControll.GetAnimator().speed;
            /*if(!CompareTag("Player"))
            Debug.Log(direSpeedGo);*/

            if(dire.x != 0f && canTurn)
            {
                Turn(dire.x > 0f);
            }
        }
    }

    public bool IsMove()
    {
        return direSpeedGo.sqrMagnitude > 0f;
    }

    public void MakeJump()
    {
        /*attribute atb = GetComponent<attribute>();
        skill skillNow = atb.GetActionControll().GetSkillNow();*/

        if (jumpingCount <= 0f && canJump && obstacleDown)
        {
            if (canMakeMovement)
            {
                jumpingCount = 0.1f;
                Vector2 veloAux = body2D.velocity;

                veloAux.y = powerJump * Mathf.Sqrt(body2D.gravityScale);

                if (veloAux.x > 0f)
                {
                    if (obstacleRight)
                    {
                        veloAux.x = 1f;
                    }
                }
                else if (veloAux.x < 0f)
                {
                    if (obstacleLeft)
                    {
                        veloAux.x = -1f;
                    }
                }

                stateVelocity = changeVelocity.wait;
                goToVelocity = veloAux;
            }
        }
    }

    public Vector2 JumpPower()
    {
        return Vector2.up * powerJump;
    }

    public void AddForceInBody(Vector2 force)
    {
        SetCanMove(false);

        stateVelocity = changeVelocity.wait;
        goToVelocity = force;
        //body2D.velocity = force;
    }

    public void StopMove()
    {
        direSpeedGo = Vector2.zero;

        if (flying)
        {
            body2D.velocity = Vector2.zero;
        }
        else
        {
            Vector2 v = body2D.velocity;
            v.x = 0f;
            body2D.velocity = v;
        }
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return body2D;
    }

    public void Turn(bool right, bool forced = false)
    {
        if ((canTurn && canMakeMovement) || forced)
        {
            /*if(!CompareTag("Player"))
            Debug.Log(right+" "+name);*/

            Vector3 angV3 = transform.eulerAngles;

            if (right)
            {
                if(angV3.y != 0f)
                {
                    angV3.y = 0f;
                    transform.eulerAngles = angV3;
                }
            }
            else if(angV3.y != 180f)
            {
                angV3.y = 180f;
                transform.eulerAngles = angV3;
            }
        }
    }

    public float DistancieGround()
    {
        if (obstacleDown)
        {
            return 0f;
        }

        RaycastHit2D r = Physics2D.Raycast(transform.position, Vector2.down, 10f,repository.repositoryX.GetLayerMaskGround());

        if (r.collider)
        {
            return r.distance;
        }

        return 10f;
    }

    public bool IsJumpAnima()
    {
        return body2D.velocity.y > 0f;
    }

    public float AngGround()
    {
        float v = angZ;
        if(transform.eulerAngles.y == 180f)
        {
            v *= -1f;
        }
        return v;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canMakeMovement && collision.contacts[0].normal.y > 0f)
        {
            Vector2 speedX = oldVelocity;
            speedX.y = 0f;
            body2D.velocity = speedX;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 contactNormal = collision.contacts[0].normal;

        if(!obstacleDown && canMakeMovement && ghostJump <= 0f && contactNormal.y > 0f)
        {
            Vector2 push = contactNormal * 3f;
            if (push.y < body2D.velocity.y)
            {
                push.y = body2D.velocity.y;
            }

            AddForceInBody(push);
            timeMoveAgain = 0.5f;
        }
    }

    public void GoingDown()
    {
        if (colliderGround)
        {
            Effector2D effector2D = colliderGround.GetComponent<Effector2D>();

            if (effector2D)
            {
                if (colliderIgnore)
                {
                    Physics2D.IgnoreCollision(coll2D, colliderIgnore, false);
                }

                colliderIgnore = colliderGround;
                colliderGround = null;
                Physics2D.IgnoreCollision(coll2D, colliderIgnore, true);
            }
        }
    }

    public Vector2 NextPos(float time)
    {
        Vector2 pos = transform.position;
        Vector2 vlc = body2D.velocity;

        if (DistancieGround() == 0f || flying)
        {
            if (canMakeMovement || !colliderGround)
            {
                pos += time * vlc;
            }
            else
            {
                float speedXF = vlc.x;
                float friction = time * colliderGround.friction * Physics2D.gravity.y;

                if (vlc.x > 0f)
                {
                    friction *= -1f;
                }

                speedXF -= friction;
                float timeX = time;

                if (speedXF < 0f)
                {
                    speedXF = 0f;
                    timeX = vlc.x / friction;
                }

                pos.x = (vlc.x + speedXF)*timeX / 2f;
            }
        }
        else
        {
            pos.x += vlc.x* time;
            float displacementY = time * (vlc.y + Physics2D.gravity.y * body2D.gravityScale * time);
            if (displacementY < -DistancieGround())
            {
                displacementY = -DistancieGround();
            }
            pos.y += displacementY;
        }

        return pos;
    }
}
