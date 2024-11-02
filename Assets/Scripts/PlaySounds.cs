using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic; 
    public AudioClip normalMusic;
    public AudioClip ghostScaredMusic;
    public AudioClip ghostDeadMusic;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        //PlayNormalMusic();
    }

    void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();

        //Invoke("PlayNormalMusic", introMusic.length);
    }

    public void PlayNormalMusic()
    {
        if (audioSource.clip != normalMusic)
        {
            audioSource.clip = normalMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayGhostScaredMusic()
    {
        if (audioSource.clip != ghostScaredMusic)
        {
            audioSource.clip = ghostScaredMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayGhostDeadMusic()
    {
        if (audioSource.clip != ghostDeadMusic)
        {
            audioSource.clip = ghostDeadMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

    }


}