using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(SetText))]
public class SetTextEditor : Editor
{
    // Listas separadas para los enums de cada carpeta

    private Type[] audioEnums;

    private int selectedAudioEnumIndex = 0;
    private string selectedAudioEnumName;
    private Type selectedAudioEnumType;
    private int selectedAudioValueIndex = 0;

    private int lastActorCount = 0;

    private void OnEnable()
    {
        // ➡️ Paso 1: Definimos las rutas de las carpetas

        string audioFolderPath = "Assets/EnumsLanguagueManager/Texts";

        // ➡️ Paso 2: Buscamos y almacenamos los enums de la carpeta
        string[] audioScriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { audioFolderPath })
      .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
      .Where(path => path.EndsWith(".cs"))
      .ToArray();
        string[] audioClassNames = audioScriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
        audioEnums = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(assembly => assembly.GetTypes())
          .Where(type => type.IsEnum && type.IsPublic && audioClassNames.Contains(type.Name))
          .ToArray();

        if (audioEnums.Length > 0)
        {
            selectedAudioEnumName = audioEnums[0].Name;
            selectedAudioEnumType = audioEnums[0];
            selectedAudioEnumIndex = 0;
        }

    
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        // ➡️ Paso 5: Dibujamos el primer selector para los enums de "EnumsObjectManager"


        // ➡️ Dibujamos el selector de audio (ya maneja tanto el tipo de enum como el valor)
        if (audioEnums.Length > 0)
        {
            string[] audioEnumNames = audioEnums.Select(t => t.Name).ToArray();
            int newAudioIndex = EditorGUILayout.Popup("Select text Enum", selectedAudioEnumIndex, audioEnumNames);

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
                SetText tutorialStep = (SetText)target;
                tutorialStep.Index = selectedAudioValueIndex;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No se encontraron enums en la carpeta de Audios.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}