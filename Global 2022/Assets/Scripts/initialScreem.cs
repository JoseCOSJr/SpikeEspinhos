using UnityEngine;

public class initialScreem : MonoBehaviour
{
    public bool canSkip = false;


    // Start is called before the first frame update
    void Start()
    {
        controllMovement.canPlayerInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && canSkip)
        {
            //GetComponent<Animator>().SetTrigger("Exit");
            ClosedScreem();
        }
    }

    public void ClosedScreem()
    {
        controllMovement.canPlayerInput = true;
        gameObject.SetActive(false);
    }
}
