using UnityEngine;

public class itemColection : MonoBehaviour
{
    public int scores = 1;
    public AudioClip getClip;

    // Start is called before the first frame update
    void Start()
    {
        collectionChar.allScores += scores;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collectionChar collect = collision.GetComponentInParent<collectionChar>();

        if (collect)
        {
            collect.AddScores(scores);
            gameObject.SetActive(false);
            repository.repositoryX.GetAudioSource().PlayOneShot(getClip);
        }
    }
}
