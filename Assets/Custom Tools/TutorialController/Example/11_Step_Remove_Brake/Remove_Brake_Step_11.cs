using UnityEngine;
using System.Linq;

public class Remove_Brake_Step_11 : ActionStep
{
    CarController carController;

    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
    }


    private void Update()
    {
        if (carController.mBreak == 0)
            NextStep();
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {
        base.StartStep(actorsInStep, actorsInTutorial, stepState);
    }
}