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

    private void OnEnable()
    {
        // 1. Define la ruta de la carpeta que quieres buscar
        string folderPath = "Assets/GeneratedEnums";

        // 2. Encuentra todos los archivos .cs en esa carpeta
        string[] scriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { folderPath })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path => path.EndsWith(".cs"))
            .ToArray();

        // 3. Obtén los nombres de las clases de esos archivos
        string[] classNames = scriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();

        // 4. Busca los tipos de C# que coincidan con esos nombres y que sean enums
        enumTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsEnum && type.IsPublic && classNames.Contains(type.Name))
            .ToArray();

        TutorialStep tutorialStep = (TutorialStep)target;

    }

    public override void OnInspectorGUI()
    {
        TutorialStep tutorialStep = (TutorialStep)target;
        serializedObject.Update();

        // Draw the default inspector for the base properties
        DrawPropertiesExcluding(serializedObject, "m_Script", "startCommands");

        // --- Custom Enum Selector ---

        // Get a list of enum names to display in the dropdown
        string[] enumNames = enumTypes.Select(t => t.Name).ToArray();

        if (selectedEnumName != null)
        {
            int index = Array.IndexOf(enumNames, selectedEnumName);
            selectedEnumIndex = index >= 0 ? index : 0;
        }

        // Display the enum type dropdown
        int newIndex = EditorGUILayout.Popup("Select Enum Type", selectedEnumIndex, enumNames);
        if (newIndex != selectedEnumIndex)
        {
            selectedEnumIndex = newIndex;

            for (int i = 0; i < tutorialStep.actors.Length; i++)
            {
                tutorialStep.actors[i].objectID = enumNames[newIndex];
            }
        }

        SerializedProperty startCommandsProp = serializedObject.FindProperty("startCommands");
        serializedObject.ApplyModifiedProperties();
    }
}