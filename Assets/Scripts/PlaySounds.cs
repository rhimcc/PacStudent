using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic; 
    public AudioClip normalMusic; 

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        PlayIntroMusic();
    }

    void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();

        Invoke("PlayNormalMusic", introMusic.length);
    }

    void PlayNormalMusic()
    {
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}