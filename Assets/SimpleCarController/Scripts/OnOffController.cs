using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffController : MonoBehaviour
{
    public bool IsOn { get; private set; }

    [Header("Audio Motor")]

    //Mejorar contacto para acceder desde fura del scprit;
    public bool contact;

    public bool tryOn;

    [SerializeField]
    AudioSource onAudio, loopEngine, offAudio;

    [SerializeField]
    float timeToTurningOn = 2f;

    float time = 0;

    private void Awake()
    {
        this.IsOn = false;
    }


    public void TurningOnCar(bool tryToTurningOn) 
    {
        if (this.IsOn) {
            time = 0;
            return;
         };

        if (tryToTurningOn)
        {
            time += Time.deltaTime;

            if (!onAudio.isPlaying && time > timeToTurningOn/4)
                onAudio.PlayOneShot(onAudio.clip,0.7f);

            if(time > timeToTurningOn)
            {
                this.IsOn = true;
                loopEngine.Play();
            }
        }
        else
        {
            time = 0;
            onAudio.Stop();
        }
    }
    public void TurningOffCar()
    {
        if (!this.IsOn)     
            return;

        time = 0;
        onAudio.Stop();
        loopEngine.Stop();
        if (!offAudio.isPlaying)
            offAudio.PlayOneShot(offAudio.clip, 0.7f);

        this.IsOn = false;
        contact = false;
    }

}
