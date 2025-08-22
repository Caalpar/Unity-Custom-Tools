using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialController))]
public class TutorialControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Next Step"))
        {
            ((TutorialController)target).TryNextStep();
        }
    }
}
