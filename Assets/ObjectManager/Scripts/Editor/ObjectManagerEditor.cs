using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectManager))]

public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectManager manager = (ObjectManager)target;

        if (GUILayout.Button("Crear Enum desde GameObjects"))
        {
            manager.CrearEnumDesdeObjetos();
        }
    }
}
