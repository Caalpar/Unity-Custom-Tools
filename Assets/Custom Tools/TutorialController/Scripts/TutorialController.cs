using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class TutorialController : MonoBehaviour
{
  
    public Tutorial currentTutorial;

    private int currentStepIndex;
    private bool isStepCompleted;
    private int score;
    private bool isPaused;
    private GameObject currentVisualAid;
    private AudioSource audioSource;
    [SerializeField] LanguageManager languageManager;

    [SerializeField] bool startTutorial = true;

    public bool isRunStep {  get; private set; }
    public int currentStep { get { return currentStepIndex; } }

    public bool isAudioPlay { get {  return audioSource.isPlaying; } }

    private void Start()
    {

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        currentStepIndex = 0;
        isStepCompleted = false;
        score = 0;
        isPaused = false;

        if (currentTutorial.languageManager != null)
            languageManager = currentTutorial.languageManager;

        if (startTutorial)
            ShowStep(currentStepIndex);
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
     //   instructionText.text = step.instructionText;

        if (step.visualAidPrefab != null)
        {
            currentVisualAid = Instantiate(step.visualAidPrefab, transform.position, Quaternion.identity);
            currentVisualAid.GetComponent<ActionStep>().tutorialController = this;
        }

        if (step.audio)
        {

            AudioClip clip = null;

            if (languageManager == null) {
                throw new Exception("Language manager is empty");
            }
            else
            {
                clip = languageManager.GetAudio(step.audioIndex);
            }
            
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
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
        CompleteStep();
    }

    public void UserAction(bool isCorrect)
    {
        if (isPaused) return;

        if (currentTutorial.steps[currentStepIndex].completionCondition == CompletionCondition.ActionBased)
        {
            if (isCorrect)
            {
      
                score++;
                isStepCompleted = true;
                NextStep();
            }
            else
            {
                RestartStep();
            }
            UpdateUI();
        }
    }

    public void NextStepWithDelay(int sec)
    {
        StartCoroutine(Delay(sec));
    }

    IEnumerator Delay(int sec)
    {
        yield return new WaitForSeconds(sec);
        UserAction(true);
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

    void CompleteStep()
    {
        isStepCompleted = true;
        isRunStep = false;
        NextStep();
    }

    void NextStep()
    {

        if (!isStepCompleted || audioSource.isPlaying)
        {
            Invoke("NextStep", 1);
        }
        else
        {
            if (currentVisualAid != null)
            {
                isRunStep = false;
                Destroy(currentVisualAid);
            }

            currentStepIndex++;
            if (currentStepIndex < currentTutorial.steps.Length)
            {

                ShowStep(currentStepIndex);
            }
            else
            {
                Debug.Log("Tutorial completed!");
                currentStepIndex = 0;
            }

            isStepCompleted = false;
        }

 
    }

    void RestartStep()
    {
        ShowStep(currentStepIndex);
    }

    void UpdateUI()
    {
        // Actualiza la interfaz de usuario según sea necesario
    }
}
