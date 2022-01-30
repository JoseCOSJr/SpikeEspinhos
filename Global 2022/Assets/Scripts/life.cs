using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class life : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    private int maxLifes = 1;
    private int currentLife;
    public GameObject deadObj;
    public float timeInvuneravleBeforeHit = 0.5f;
    private float countTimeInvulnevable = 0f;
    private SpriteRenderer renderer;
    private float timeGameOver = 0f;
    public AudioClip clipHurt, clipDead;
    public hudLife hudLifeHere = null;
    public Volume volumeModifie;
    private float timeAuxVolume;


    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
        currentLife = maxLifes;
        if (deadObj)
        {
            repository.repositoryX.AddResapwObj(deadObj);
        }
    }


    private void LateUpdate()
    {
        if (countTimeInvulnevable > 0f)
        {
            countTimeInvulnevable -= Time.deltaTime;
            if(countTimeInvulnevable <= 0f)
            {
                renderer.color = Color.white;
                GetComponentInChildren<triggerAnimaEvents>().ResetHitstun();
            }
            else
            {
                Color c = Color.Lerp(Color.white, Color.black, 0.5f);
                if(renderer.color != c)
                {
                    renderer.color = c;
                }
            }
        }

        if (timeGameOver > 0f)
        {
            timeGameOver -= Time.deltaTime;
            

            if (timeAuxVolume > 1f)
            {
                timeAuxVolume += Time.deltaTime;
                volumeModifie.weight = timeAuxVolume;
            }

            if (timeGameOver <= 0f)
            {
                SceneManager.LoadSceneAsync(0);
            }

        }
    }

    public void AddMaxLife()
    {
        maxLifes += 1;
        currentLife += 1;
    }

    public bool GetInvulnerable()
    {
        return countTimeInvulnevable > 0f;
    }

    public void SetInvulnerable()
    {
        countTimeInvulnevable = timeInvuneravleBeforeHit;
    }

    public void AddLife(int add)
    {
        if (add > 0 || countTimeInvulnevable <= 0f)
        {
            if (currentLife > 0)
            {
                currentLife += add;
                if (currentLife > maxLifes)
                {
                    currentLife = maxLifes;
                }
                else if (currentLife < 0)
                {
                    currentLife = 0;
                }

                if (hudLifeHere)
                {
                    hudLifeHere.AttLifesInfs(currentLife);
                }

                if (add < 0)
                {
                    SetInvulnerable();
                    repository.repositoryX.GetAudioSource().PlayOneShot(clipHurt);
                }

                if (currentLife == 0)
                {
                    //Adciona evento de morte do personagens
                    Vector3 posCenter = GetComponent<Collider2D>().bounds.center;
                    if (deadObj)
                    {
                        GameObject deadObjX = repository.repositoryX.GetRespawObj(deadObj);
                        deadObjX.transform.position = posCenter;
                        deadObjX.SetActive(true);
                        AudioSource ad = repository.repositoryX.GetAudioSource();
                        ad.volume = 1f;
                        ad.PlayOneShot(clipDead);
                    }

                    if (CompareTag("Player"))
                    {
                        timeGameOver = 3f;
                        timeAuxVolume = volumeModifie.weight;
                        renderer.enabled = false;
                        movement2D mv = GetComponent<movement2D>();
                        mv.enabled = false;
                        mv.GetRigidbody2D().gravityScale = 0f;
                        mv.GetRigidbody2D().velocity = Vector2.zero;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
