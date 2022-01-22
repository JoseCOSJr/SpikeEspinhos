using UnityEngine;

public class controllMovement : MonoBehaviour
{
    private movement2D movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<movement2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputMv = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //Debug.Log(inputMv);
        movement.MakeMove(inputMv);

        if (Input.GetButtonDown("Jump"))
        {
            movement.MakeJump();
        }
    }
}
