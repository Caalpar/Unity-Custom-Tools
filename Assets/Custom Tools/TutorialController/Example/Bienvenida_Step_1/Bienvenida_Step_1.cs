using UnityEngine;

public class Bienvenida_Step_1 : ActionStep
{
    public override void ResetStep(Actor[] actors)
    {
        for (int i = 0; i < actors.Length; i++)
        {
            Debug.Log(actors[i].enumTypeName);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}