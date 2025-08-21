using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectManager))]

public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectManager manager = (ObjectManager)target;

        if (GUILayout.Button("Create the enum from the GameObjects"))
        {
            manager.CrearEnumDesdeObjetos();
        }
    }
}
