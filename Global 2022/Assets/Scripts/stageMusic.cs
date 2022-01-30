using System.Collections;
using UnityEngine;

public class stageMusic : MonoBehaviour
{
    public static stageMusic stageMusicX;
    public AudioClip[] clips = new AudioClip[4];
    private AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        stageMusicX = this;
        source = GetComponent<AudioSource>();
    }

    public AudioSource GetAudioSource()
    {
        return source;
    }

    public void SetIdMusic(int id)
    {
        if (id >= clips.Length)
        {
            id = clips.Length - 1;
        }

        AudioClip clipNew = clips[id];
        if (clipNew != source.clip)
        {
            StartCoroutine(ShiftMusic(clipNew));
        }
    }

    IEnumerator ShiftMusic(AudioClip newClip)
    {
        for (float volume = source.volume; volume >= 0; volume-=0.1f)
        {
            volume -= Time.deltaTime;
            source.volume = volume;
            yield return new WaitForSeconds(.1f);
        }

        source.Stop();
        source.clip = newClip;
        source.Play();

        for (float volume = source.volume; volume < 1; volume+=0.1f)
        {
            volume += Time.deltaTime;
            source.volume = volume;
            yield return new WaitForSeconds(.1f);
        }

        yield return null;
    }
}
