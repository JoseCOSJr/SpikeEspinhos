using UnityEngine;
using System.Collections.Generic;

public class actionControll : MonoBehaviour
{
    public List<actions> actionsList = new List<actions>();
    private bool canAction = true;
    private actions actionNow = null, actionWait = null;
    private Vector2 direAction = Vector2.zero;
    private movement2D movement;
    public Transform transformExitPower;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<movement2D>();

        for(int i = 0; i < actionsList.Count; i++)
        {
            if (actionsList[i].powerAction)
            {
                repository.repositoryX.AddResapwObj(actionsList[i].powerAction);
            }
        }
    }

    public void SetCanAction(bool can)
    {
        canAction = can;
        if (!canAction && actionNow)
        {
            actionNow = null;
        }
    }

    public void TryAction(actions act, Vector2 dire)
    {
        if (canAction && actionsList.Exists(x=>x==act) && movement.GetAnimaControll())
        {
            if (!actionNow)
            {
                movement.SetActivedAnimations(false);
                movement.GetAnimaControll().SetActionAnimation(act.actionAnima, true);
                actionNow = act;
                movement.SetCanJump(act.canJump);
                movement.SetCanTurn(act.canTurn);
                movement.SetMultSpeed(act.multSpeed);

                if (dire.x != 0f)
                {
                    movement.Turn(dire.x > 0f, true);
                }
            }
            else
            {
                actionWait = act;
                direAction = dire;
            }
        }
    }

    public bool GetCanAction()
    {
        return canAction;
    }

    public void ResetAction()
    {
        actionNow = null;
        if (actionWait)
        {
            TryAction(actionWait, direAction);
            actionWait = null;
        }
        else
        {
            movement.SetActivedAnimations(true);
            movement.SetCanJump(true);
            movement.SetCanTurn(true);
            movement.SetMultSpeed(1f);
        }
    }

    public void RespawPower()
    {
        GameObject obj = repository.repositoryX.GetRespawObj(actionNow.powerAction);
        obj.SetActive(false);
        obj.transform.position = transformExitPower.position;
        obj.transform.rotation = transformExitPower.rotation;
        obj.SetActive(true);
    }
}
