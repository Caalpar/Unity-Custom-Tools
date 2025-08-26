using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(TutorialStep))]
public class TutorialStepEditor : Editor
{
    // Listas separadas para los enums de cada carpeta
    private Type[] objectEnums;
    private Type[] audioEnums;

    private int selectedObjectEnumIndex = 0;
    private string selectedObjectEnumName;
    private Type selectedObjectEnumType;

    private int selectedAudioEnumIndex = 0;
    private string selectedAudioEnumName;
    private Type selectedAudioEnumType;
    private int selectedAudioValueIndex = 0;

    private int lastActorCount = 0;

    private void OnEnable()
    {
        // ➡️ Paso 1: Definimos las rutas de las carpetas
        string objectFolderPath = "Assets/EnumsObjectManager";
        string audioFolderPath = "Assets/EnumsLanguagueManager/Audios";

        // ➡️ Paso 2: Buscamos y almacenamos los enums de la primera carpeta
        string[] objectScriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { objectFolderPath })
      .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
      .Where(path => path.EndsWith(".cs"))
      .ToArray();
        string[] objectClassNames = objectScriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
        objectEnums = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(type => type.IsEnum && type.IsPublic && objectClassNames.Contains(type.Name))
          .ToArray();

        // ➡️ Paso 3: Buscamos y almacenamos los enums de la segunda carpeta
        string[] audioScriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { audioFolderPath })
      .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
      .Where(path => path.EndsWith(".cs"))
      .ToArray();
        string[] audioClassNames = audioScriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
        audioEnums = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(type => type.IsEnum && type.IsPublic && audioClassNames.Contains(type.Name))
          .ToArray();

        // ➡️ Paso 4: Inicializamos los valores por defecto para cada selector
        if (objectEnums.Length > 0)
        {
            selectedObjectEnumName = objectEnums[0].Name;
            selectedObjectEnumType = objectEnums[0];
            selectedObjectEnumIndex = 0;
        }

        if (audioEnums.Length > 0)
        {
            selectedAudioEnumName = audioEnums[0].Name;
            selectedAudioEnumType = audioEnums[0];
            selectedAudioEnumIndex = 0;
        }

        TutorialStep tutorialStep = (TutorialStep)target;


        // Inicializa el selector de ObjectsManager Enum

        if (tutorialStep.actors != null && tutorialStep.actors.Length > 0 && tutorialStep.actors[0] != null)
        {
            selectedObjectEnumName = tutorialStep.actors[0].enumTypeName;
            selectedObjectEnumType = objectEnums.FirstOrDefault(t => t.Name == selectedObjectEnumName);
            if (selectedObjectEnumType != null)
            {
                selectedObjectEnumIndex = Array.IndexOf(objectEnums.Select(t => t.Name).ToArray(), selectedObjectEnumName);
            }
        }

        lastActorCount = tutorialStep.actors != null ? tutorialStep.actors.Length : 0;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "m_Script", "actors");

        // ➡️ Paso 5: Dibujamos el primer selector para los enums de "EnumsObjectManager"
        if (objectEnums.Length > 0)
        {
            string[] enumNames = objectEnums.Select(t => t.Name).ToArray();
            int newIndex = EditorGUILayout.Popup("Select Object Enum", selectedObjectEnumIndex, enumNames);
            if (newIndex != selectedObjectEnumIndex)
            {
                selectedObjectEnumIndex = newIndex;
                selectedObjectEnumName = enumNames[newIndex];
                selectedObjectEnumType = objectEnums[newIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No se encontraron enums en la carpeta EnumsObjectManager.", MessageType.Warning);
        }

        // ➡️ Dibujamos el selector de audio (ya maneja tanto el tipo de enum como el valor)
        if (audioEnums.Length > 0)
        {
            string[] audioEnumNames = audioEnums.Select(t => t.Name).ToArray();
            int newAudioIndex = EditorGUILayout.Popup("Select Audio Enum", selectedAudioEnumIndex, audioEnumNames);

            if (newAudioIndex != selectedAudioEnumIndex)
            {
                selectedAudioEnumIndex = newAudioIndex;
                selectedAudioEnumName = audioEnumNames[newAudioIndex];
                selectedAudioEnumType = audioEnums[newAudioIndex];

                // Reinicia el índice del valor del audio cuando cambia el tipo de enum
                selectedAudioValueIndex = 0;
            }

            // ➡️ Agrega el selector para los valores del enum de audio
            if (selectedAudioEnumType != null)
            {
                // Obtén los nombres de los valores del enum de audio
                string[] audioValues = Enum.GetNames(selectedAudioEnumType);

                // Dibuja el nuevo selector para los valores del audio
                selectedAudioValueIndex = EditorGUILayout.Popup("Audio Value", selectedAudioValueIndex, audioValues);

                // Guarda el valor seleccionado en el ScriptableObject
                TutorialStep tutorialStep = (TutorialStep)target;
                tutorialStep.audioEnumTypeName = selectedAudioEnumName;
                tutorialStep.AudioIndex = selectedAudioValueIndex;
                tutorialStep.audioEnumValueName = audioValues[selectedAudioValueIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No se encontraron enums en la carpeta de Audios.", MessageType.Warning);
        }

        SerializedProperty actorsProp = serializedObject.FindProperty("actors");
        if (actorsProp.arraySize != lastActorCount)
        {
            if (actorsProp.arraySize > lastActorCount)
            {
                for (int i = lastActorCount; i < actorsProp.arraySize; i++)
                {
                    SerializedProperty actorProp = actorsProp.GetArrayElementAtIndex(i);
                    // Aquí asumimos que el primer actor usará el enum de objetos.
                    // Podrías necesitar una lógica más compleja para decidir cuál usar.
                    actorProp.FindPropertyRelative("enumTypeName").stringValue = selectedObjectEnumName;
                }
            }
            lastActorCount = actorsProp.arraySize;
        }

        EditorGUILayout.PropertyField(actorsProp, true);

        serializedObject.ApplyModifiedProperties();
    }
}