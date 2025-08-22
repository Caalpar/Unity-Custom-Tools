using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.IO;

[CustomEditor(typeof(TutorialStep))]
public class TutorialStepEditor : Editor
{
    private Type[] enumTypes;
    private int selectedEnumIndex = 0;
    private string selectedEnumName;
    private Type selectedEnumType;
    private int lastActorCount = 0; // Agregamos esta variable

    private void OnEnable()
    {
        string folderPath = "Assets/GeneratedEnums";
        string[] scriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { folderPath })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path => path.EndsWith(".cs"))
            .ToArray();

        string[] classNames = scriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();

        enumTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsEnum && type.IsPublic && classNames.Contains(type.Name))
            .ToArray();

        TutorialStep tutorialStep = (TutorialStep)target;
        if (tutorialStep.actors != null && tutorialStep.actors.Length > 0 && tutorialStep.actors[0] != null)
        {
            selectedEnumName = tutorialStep.actors[0].enumTypeName;
            selectedEnumType = enumTypes.FirstOrDefault(t => t.Name == selectedEnumName);
            if (selectedEnumType != null)
            {
                selectedEnumIndex = Array.IndexOf(enumTypes.Select(t => t.Name).ToArray(), selectedEnumName);
            }
        }
        else if (enumTypes.Length > 0)
        {
            // Si no hay actores, selecciona el primer enum por defecto
            selectedEnumName = enumTypes[0].Name;
            selectedEnumType = enumTypes[0];
            selectedEnumIndex = 0;
        }

        lastActorCount = tutorialStep.actors != null ? tutorialStep.actors.Length : 0;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", "actors");

        if (enumTypes.Length > 0)
        {
            string[] enumNames = enumTypes.Select(t => t.Name).ToArray();
            int newIndex = EditorGUILayout.Popup("Select items of scence", selectedEnumIndex, enumNames);

            if (newIndex != selectedEnumIndex)
            {
                selectedEnumIndex = newIndex;
                selectedEnumName = enumNames[newIndex];
                selectedEnumType = enumTypes[newIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No se encontraron enums en la carpeta especificada.", MessageType.Warning);
        }

        SerializedProperty actorsProp = serializedObject.FindProperty("actors");

        // La clave: detectamos si la lista cambió de tamaño
        if (actorsProp.arraySize != lastActorCount)
        {
            // Si la lista creció, inicializamos los nuevos elementos
            if (actorsProp.arraySize > lastActorCount)
            {
                for (int i = lastActorCount; i < actorsProp.arraySize; i++)
                {
                    SerializedProperty actorProp = actorsProp.GetArrayElementAtIndex(i);
                    actorProp.FindPropertyRelative("enumTypeName").stringValue = selectedEnumName;
                }
            }
            lastActorCount = actorsProp.arraySize;
        }

        EditorGUILayout.PropertyField(actorsProp, true);

        serializedObject.ApplyModifiedProperties();
    }
}