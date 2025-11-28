using UnityEngine;
using System.Linq;

public class Up_Lever_Step_3 : ActionStep
{
    CarController carController;

    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
    }

    private void Update()
    {
        if(carController.transmision == 1)
        {
            NextStep();
        }
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {
        base.StartStep(actorsInStep, actorsInTutorial, stepState);
    }
}