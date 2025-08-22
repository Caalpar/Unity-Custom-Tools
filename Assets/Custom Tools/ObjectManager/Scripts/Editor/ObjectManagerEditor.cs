using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectManager))]

public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectManager manager = (ObjectManager)target;

        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty listObjectsProperty = serializedObject.FindProperty("listObjects");

        EditorGUILayout.PropertyField(listObjectsProperty, true);

        // --- Título para la lista de checkboxes ---
        EditorGUILayout.Space(5); // Pequeño espacio para separar
        EditorGUILayout.LabelField("Objects control", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        // ------------------------------------------

        // Dibuja el contenido de cada elemento del array
        for (int i = 0; i < manager.listObjects.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();

            // Campo para el GameObject (si quieres mantenerlo)
            // No es necesario, ya que PropertyField ya lo hace.
            // EditorGUILayout.ObjectField($"Element {i}", myTarget.listObjects[i], typeof(GameObject), true);

            // Dibuja el checkbox solo si el objeto no es nulo
            if (manager.listObjects[i] != null)
            {
                // Dibuja el checkbox
                bool currentState = manager.listObjects[i].activeSelf;
                bool newState = EditorGUILayout.Toggle(currentState, GUILayout.Width(20));

                if (newState != currentState)
                {
                    manager.listObjects[i].SetActive(newState);
                }

                // Aquí se agrega el Label con el nombre del objeto
                EditorGUILayout.LabelField(manager.listObjects[i].name);
            }

            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("Create the enum from the GameObjects"))
        {
            manager.CrearEnumDesdeObjetos();
        }


        if (GUILayout.Button("Disable all objects"))
        {
            for (int i = 0; i < manager.listObjectsAmount; i++)
            {
                manager.Desactive(i);
            }
        }

        if (GUILayout.Button("Enable all objects"))
        {
            for (int i = 0; i < manager.listObjectsAmount; i++)
            {
                manager.Active(i);
            }
        }
    }
}
