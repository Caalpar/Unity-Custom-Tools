using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] LanguageManager languageManager;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }
    }


    public void Play(int index)
    {
        if(audioSource.isPlaying)
            audioSource.Pause();
        AudioClip clip = languageManager.GetAudio(index);
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        
    }
}
