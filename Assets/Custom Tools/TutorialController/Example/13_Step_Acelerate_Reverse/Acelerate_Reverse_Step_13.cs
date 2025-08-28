using UnityEngine;
using System.Linq;

public class Acelerate_Reverse_Step_13 : ActionStep
{
    CarController carController;
    float lastTorque = 0;
    bool isActive = false;
    private void Start()
    {
        carController = GetComponentFrom<CarController>(ITEM_TUTORIALCONTROLLER.SimpleCarController);
        lastTorque = carController.torque;
    }


    private void Update()
    {
        if (carController.torque > lastTorque && !isActive)
        {
            isActive = true;
            NextStep();
        }
    }
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial)
    {
        base.StartStep(actorsInStep, actorsInTutorial);
    }
}