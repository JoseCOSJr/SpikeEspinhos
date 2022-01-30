using UnityEngine;

public class uniquePosZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Vector3 pos = transform.position;
        pos.z += Random.value;
        transform.position = pos;
    }
}
