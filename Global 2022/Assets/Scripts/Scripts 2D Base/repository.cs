using System.Collections.Generic;
using UnityEngine;

public class repository : MonoBehaviour
{
    public static repository repositoryX;
    [SerializeField]
    private AudioSource audioSourceObj = null;
    private List<AudioSource> audioSourcesList = new List<AudioSource>();
    public PhysicsMaterial2D physicsMaterialDontMove, physicsMaterialCharaterDafault;
    [Header("Layers")]
    [SerializeField]
    private LayerMask layerMaskHurt;
    [SerializeField]
    private LayerMask layerMaskGround, layerMaskObst;
    private List<GameObject> listRespawObj = new List<GameObject>();

    private void Awake()
    {
        repositoryX = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 64; i++)
        {
            AudioSource ad = Instantiate(audioSourceObj);
            ad.transform.SetParent(transform);
            ad.name = audioSourceObj.name;
            audioSourcesList.Add(ad);
        }
    }

    public LayerMask GetLayerMaskHurt()
    {
        return layerMaskHurt;
    }

    public LayerMask GetLayerObst()
    {
        return layerMaskObst;
    }

    public LayerMask GetLayerMaskGround()
    {
        return layerMaskGround;
    }

    public AudioSource GetAudioSource()
    {
        AudioSource ad = audioSourcesList.Find(x => !x.isPlaying);
        if (!ad)
        {
            for (int i = 0; i < 16; i++)
            {
                ad = Instantiate(audioSourceObj);
                ad.transform.SetParent(transform);
                audioSourcesList.Add(ad);
            }
        }
        else
        {
            ad.pitch = 1f;
            ad.loop = false;
        }
        ad.volume = 1f;
        ad.priority = Random.Range(1, 256);

        return ad;
    }

    public List<GameObject> GetListObjs(string name)
    {
        return listRespawObj.FindAll(x => x.name == name);
    }

    public void AddResapwObj(GameObject obj)
    {
        if (!listRespawObj.Exists(x => x.name == obj.name))
        {
            for (int i = 0; i < 32; i++)
            {
                GameObject objX = Instantiate(obj);
                objX.name = obj.name;
                objX.SetActive(false);
                objX.transform.SetParent(transform);
                listRespawObj.Add(objX);
            }
        }
    }

    public GameObject GetRespawObj(GameObject obj, bool noNeedDesatived =true)
    {
        GameObject objX = listRespawObj.Find(x => x.name == obj.name && (!noNeedDesatived || !x.activeInHierarchy));

        if (!objX)
        {
            for (int i = 0; i < 16; i++)
            {
                objX = Instantiate(obj);
                objX.name = obj.name;
                objX.SetActive(false);
                objX.transform.SetParent(transform);
                listRespawObj.Add(objX);
            }
        }

        if (!noNeedDesatived)
        {
            objX.SetActive(true);
        }

        return objX;
    }
}
