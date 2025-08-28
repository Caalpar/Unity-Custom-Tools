using UnityEngine;
using System.Linq;

public class Down_Lever_Step_6 : ActionStep
{
    CarController carController;

    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
    }


    private void Update()
    {
        if (carController.transmisión == -1)
            NextStep();
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {

        FadeEffect fadeInOut = GetComponentFrom<FadeEffect>(ITEM_TUTORIALCONTROLLER.FadeEffect);

        switch (stepState)
        {
            case StepState.REPEAT:
                break;
            case StepState.CURRENT:
                break;
            case StepState.SELECT:
                break;
            case StepState.RESUME:
                break;
            default:
                
                break;
        }
        fadeInOut.FadeInOut(()=> { base.StartStep(actorsInStep, actorsInTutorial, stepState); });

        

    }
}