using UnityEngine;
using System.Collections.Generic;

public class platformMove : MonoBehaviour
{
    private List<Transform> transformsListChild = new List<Transform>();
    public List<Vector3> movePos = new List<Vector3>();
    private Vector3 pos0, posOld;
    [Min(1)]
    public float timeMakeAllPath = 1f;
    public float timeWait = 1f;
    private int idGo = 1;
    private float goCount = 0f;
    private float countWait = 0f;
    private bool wait = true, backing = false;
    private float speed;

    // Start is called before the first frame update
    void Awake()
    {
        pos0 = transform.position;
        posOld = pos0;
        float dist = 0f;

        for(int i = 0; i < movePos.Count; i++)
        {
            Vector3 delta = pos0;
            if(i > 0)
            {
                delta = movePos[i-1];
            }
            delta -= movePos[i];
            dist += delta.magnitude;
        }

        speed = dist / timeMakeAllPath;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(movePos.Count > 0)
        {
            if (wait)
            {
                if (countWait >= timeWait)
                {
                    countWait = 0f;
                    wait = false;
                }
                else
                {
                    countWait += Time.fixedDeltaTime;
                }
            }
            else
            {
                Vector3 posX = pos0;
                if (idGo > 0)
                {
                    posX += movePos[idGo-1];
                }

                float timeX = speed / Vector2.Distance(posX, posOld);
                goCount += Time.fixedDeltaTime ;
                Vector3 posAux = Vector3.Lerp(posOld, posX, goCount/timeX);
                transform.position = posAux;

                if (goCount >= timeX)
                {
                    wait = true;
                    goCount = 0f;
                    posOld = posX;

                    if (backing)
                    {
                        idGo -= 1;
                        if (idGo < 0)
                        {
                            backing = false;
                            idGo = 1;
                        }
                    }
                    else
                    {
                        idGo += 1;

                        if (idGo >= movePos.Count)
                        {
                            idGo = movePos.Count - 1;
                            backing = true;
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y < 0f)
        {
            if (!transformsListChild.Contains(collision.collider.transform))
            {
                collision.collider.transform.SetParent(transform);
                transformsListChild.Add(collision.collider.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (transformsListChild.Contains(collision.collider.transform))
        {
            collision.collider.transform.SetParent(null);
            transformsListChild.Remove(collision.collider.transform);
        }
    }
}
