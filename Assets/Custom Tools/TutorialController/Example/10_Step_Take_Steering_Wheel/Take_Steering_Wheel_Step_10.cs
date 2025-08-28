using UnityEngine;
using System.Linq;
using Oculus.Interaction;

public class Take_Steering_Wheel_Step_10 : ActionStep
{
    CustomActionGrab customActionGrab;
    private void Start()
    {
        customActionGrab = GetComponentFrom<CustomActionGrab>(ITEM_TUTORIALCONTROLLER.CustomActionGrab);
        customActionGrab.Select.AddListener(AllEventDebug);
    }

    private void OnDisable()
    {
        if (customActionGrab != null)
            customActionGrab.Select.RemoveListener(AllEventDebug);
    }

    private void AllEventDebug(PointerEvent evt)
    {
        NextStep();
    }

    public override void StartStep(Actor[] actorsInStep, Actor[] actorsInTutorial, StepState stepState)
    {
        base.StartStep(actorsInStep, actorsInTutorial, stepState);
    }
}