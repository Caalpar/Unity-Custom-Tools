using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(LanguageManager))]
public class TutorialController : MonoBehaviour
{
    public Tutorial currentTutorial;
    [SerializeField] ObjectManager objectManager;
    [SerializeField] bool startTutorial = true;
    
    
    private LanguageManager languageManager;
    private int currentStepIndex;
    private bool isStepCompleted;
    private bool isPaused;
    private GameObject prefabStep;
    private AudioSource audioSource;
    private List<Actor> actors; 
    public bool isRunStep {  get; private set; }
    public int currentStep { get { return currentStepIndex; } }
    public bool isAudioPlay { get {  return audioSource.isPlaying; } }

    private void Start()
    {
        actors = new List<Actor>();


        foreach (TutorialStep step in currentTutorial.steps)
        {
            foreach (Actor item in step.actors)
            {
                if (!actors.Contains(item))
                {
                    actors.Add(item);
                }
            }
        }


        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
 
        currentStepIndex = 0;
        isStepCompleted = false;
        isPaused = false;

       languageManager = currentTutorial.languageManager;

        if (startTutorial)
            Play();
    }

    public void Play()
    {
        ShowStep(currentStepIndex);
    }

    void ShowStep(int index)
    {

        if (index >= currentTutorial.steps.Length)
        {
            Debug.Log("Tutorial completed!");
            return;
        }

        if (isPaused) return;

        isRunStep = true;
        TutorialStep step = currentTutorial.steps[index];

        if (step.prefabStep != null)
        {
            prefabStep = Instantiate(step.prefabStep, transform.position, Quaternion.identity);
            ActionStep action = prefabStep.GetComponent<ActionStep>();
            action.tutorialController = this;
            action.StartStep(step.actors, actors.ToArray());   
        }

        if (step.audio)
        {
            AudioClip clip = null;

            if(languageManager != null)
            {
                clip = languageManager.GetAudio(step.AudioIndex);

                if (clip != null)
                {
                    audioSource.PlayOneShot(clip);
                }
            }
   
        }

        if (step.completionCondition == CompletionCondition.TimeBased)
        {
            StartCoroutine(WaitAndCompleteStep(step.stepDuration));
        }
    }

    IEnumerator WaitAndCompleteStep(float duration)
    {
        yield return new WaitForSeconds(duration);
        isStepCompleted = true;
        isRunStep = false;
        NextStep();
    }

    public void TryNextStep()
    {
        if (isPaused) return;

        if (currentTutorial.steps[currentStepIndex].completionCondition == CompletionCondition.ActionBased)
        {
                isStepCompleted = true;
                NextStep();
        }
    }

    public void NextStepWithDelay(int sec)
    {
        StartCoroutine(Delay(sec));
    }

    IEnumerator Delay(int sec)
    {
        yield return new WaitForSeconds(sec);
        TryNextStep();
    }

    public void PauseTutorial()
    {
        isPaused = true;
    }

    public void ResumeTutorial(int setStep = 0)
    {
        currentStepIndex += setStep;
        isPaused = false;
        ShowStep(currentStepIndex);
    }

    void NextStep()
    {

        Debug.Log("next");

        if (!isStepCompleted || audioSource.isPlaying)
        {
            Invoke("NextStep", 1);
        }
        else
        {
            if (prefabStep != null)
            {
                isRunStep = false;
                Destroy(prefabStep);
            }

            currentStepIndex++;
            if (currentStepIndex < currentTutorial.steps.Length)
            {

                ShowStep(currentStepIndex);
                isStepCompleted = false;
            }
            else
            {
                Debug.Log("Tutorial completed!");
                currentStepIndex = 0;
                isStepCompleted = true;
            }

            
        }
    }

    public void SelectStep(int indexStep)
    {
        if (indexStep < 0) return;
        if (indexStep >= currentTutorial.steps.Length) return;

        currentStepIndex = indexStep-1;
        audioSource.Stop();
        NextStep();
    }

    public void RestartStep()
    {
        if (currentStepIndex <= 0) currentStepIndex = 1;
        if (currentStepIndex > currentTutorial.steps.Length) currentStepIndex = currentTutorial.steps.Length;

        currentStepIndex--;
        audioSource.Stop();
        NextStep();
    }
}
