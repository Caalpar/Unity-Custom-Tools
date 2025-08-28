using UnityEngine;
using System.Linq;

public class Decelerator_Step_7 : ActionStep
{
    CarController carController;
    float lastTorque = 0;

    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
        lastTorque = carController.torque;
    }


    private void Update()
    {
        if (lastTorque > carController.torque)
            NextStep();
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {
        base.StartStep(actorsInStep, actorsInTutorial, stepState);
    }
}