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
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayGhostScaredMusic()
    {
        audioSource.clip = ghostScaredMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayGhostDeadMusic()
    {
        audioSource.clip = ghostDeadMusic;
        audioSource.loop = true;
        audioSource.Play();

    }


}