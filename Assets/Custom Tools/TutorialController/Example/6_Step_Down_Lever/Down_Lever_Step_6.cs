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
    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial)
    {
        base.StartStep(actorsInStep, actorsInTutorial);
    }
}