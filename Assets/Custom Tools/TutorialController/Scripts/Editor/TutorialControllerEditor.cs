using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialController))]
public class TutorialControllerEditor : Editor
{

    public void OnEnable()
    {
      
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TutorialController tutorialController = (TutorialController)target;

        if (GUILayout.Button("Next Step With Action"))
        {
            tutorialController.TryNextStep();
        }

        GUILayout.Space(10);


        for (int i = 0; i < tutorialController.currentTutorial.steps.Length; i++)
        {
            if (GUILayout.Button("Step - " + i))
            {
                tutorialController.SelectStep(i);
            }
        }


    }
}
