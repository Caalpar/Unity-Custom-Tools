using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialController))]
public class TutorialControllerEditor : Editor
{
    private GUIStyle leftAlignedButtonStyle;
    public void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TutorialController tutorialController = (TutorialController)target;

        // Ensure the style is initialized
        if (leftAlignedButtonStyle == null)
        {
            leftAlignedButtonStyle = new GUIStyle(GUI.skin.button);
            leftAlignedButtonStyle.alignment = TextAnchor.MiddleLeft;

        }

        // Draw the "Next Step" button as it was, or you can apply the style
        if (GUILayout.Button("Next Step With Action"))
        {
            tutorialController.TryNextStep();
        }

        GUILayout.Space(10);

        // Iterate through all steps and draw a button for each, aligned to the left.
        for (int i = 0; i < tutorialController.currentTutorial.steps.Length; i++)
        {
            GUILayout.BeginHorizontal();

            // Draw the button using the left-aligned style
            if (GUILayout.Button(tutorialController.currentTutorial.steps[i].name, leftAlignedButtonStyle))
            {
                tutorialController.SelectStep(i);
            }

            // Use FlexibleSpace to push the button to the left
           // GUILayout.FlexibleSpace();
      

            GUILayout.EndHorizontal();
        }

    }
}
