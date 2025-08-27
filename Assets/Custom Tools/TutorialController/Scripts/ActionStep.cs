using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ActionStep : MonoBehaviour
{
    public TutorialController tutorialController;
    protected ObjectManager objectsManager;
    protected bool completed;

    public bool isAudioPlaying { get { return tutorialController.isAudioPlay; } }
    // Start is called before the first frame update
    void Awake()
    {
        objectsManager = GameObject.FindWithTag("ObjectManager").GetComponent<ObjectManager>();
       // tutorialController = objectsManager.tutorialController;
    }

    private void Start()
    {
        completed = false;
    }

    public T GetComponentFrom<T>(Enum value)
    {
        T data = objectsManager[value].GetComponent<T>();

        return data;
    }

    public void ResumeTutorial(int setStep = 0)
    {
        if (tutorialController == null) throw new Exception("Tutorial Controller is null");

        tutorialController.ResumeTutorial(setStep);
    }

    public void Active(Enum value)
    {
        objectsManager.Active(value);
    }

    public void Active(int index)
    {
        objectsManager.Active(index);
    }

    public void Desactive(Enum value)
    {
        objectsManager.Desactive(value);
    }
    public void Desactive(int index)
    {
        objectsManager.Desactive(index);
    }

    public void NextStepWithDelay(int sec)
    {
        Invoke("NextStep", sec);
    }

    public void NextStep()
    {
        if (completed) return;

        if (tutorialController == null) throw new Exception("Tutorial Controller is null");

        tutorialController.TryNextStep();
        completed = true;
    }


    public virtual void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial)
    {
        for (int i = 0; i < actorsInTutorial.Length; i++)
        {
            bool foundActor = false;
            
            for (int j = 0; j < actorsInStep.Length; j++)
            {
                if (actorsInTutorial[i].objectID == actorsInStep[j].objectID)
                {
                    foundActor = true;
                    break;
                }
            }

            if (foundActor)
            {
                Active(actorsInTutorial[i].enumIndexItem);
            }
            else
            {
                Desactive(actorsInTutorial[i].enumIndexItem);
            }

        }
    }


    public GameObject this[Enum valor]
    {
        get
        {
            return objectsManager[valor];
        }
    }

}
