using UnityEngine;

public class desativedHit : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
