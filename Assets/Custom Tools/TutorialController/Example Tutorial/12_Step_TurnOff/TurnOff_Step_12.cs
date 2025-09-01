using UnityEngine;
using System.Linq;

public class TurnOff_Step_12 : ActionStep
{
    CarController carController;

    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
    }


    private void Update()
    {
        if (!carController.onOffController.IsOn)
            NextStep();
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {
        base.StartStep(actorsInStep, actorsInTutorial, stepState);
    }
}