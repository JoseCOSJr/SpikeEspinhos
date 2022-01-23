using UnityEngine;

[CreateAssetMenu(fileName = "new Action", menuName = "ScriptableObjects/Action", order = 1)]
public class actions : ScriptableObject
{
    public string actionAnima;
    public GameObject powerAction;
    public bool canTurn = false, canJump = false;
    public float multSpeed = 0f;
}
