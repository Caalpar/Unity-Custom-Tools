using UnityEngine;
using System.Linq;

public class Down_Lever_Step_8 : ActionStep
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

        base.StartStep(actorsInStep, actorsInTutorial, stepState);

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
   

        

    }
}