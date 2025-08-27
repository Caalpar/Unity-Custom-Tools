using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;

[CustomEditor(typeof(SetText))]
public class SetTextEditor : Editor
{
    private Type[] audioEnums;
    private SerializedProperty languageManagerProp;
    private SerializedProperty textUIProp;

    private void OnEnable()
    {
        // ... (Tu código de OnEnable existente)
        string audioFolderPath = "Assets/EnumsLanguagueManager/Texts";
        string[] audioScriptPaths = AssetDatabase.FindAssets("t:TextAsset", new string[] { audioFolderPath })
            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            .Where(path => path.EndsWith(".cs"))
            .ToArray();
        string[] audioClassNames = audioScriptPaths.Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();

        audioEnums = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsEnum && type.IsPublic && audioClassNames.Contains(type.Name))
            .ToArray();

        // Obtener las propiedades serializadas de las variables que quieres mostrar
        languageManagerProp = serializedObject.FindProperty("languageManager");
        textUIProp = serializedObject.FindProperty("textUI");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // **Paso 1: Dibujar las variables que quieres ver en el inspector**
        // Si no son nulas, dibújalas con EditorGUILayout.PropertyField()
        if (languageManagerProp != null)
        {
            EditorGUILayout.PropertyField(languageManagerProp);
        }
        if (textUIProp != null)
        {
            EditorGUILayout.PropertyField(textUIProp);
        }

        EditorGUILayout.Space(10); // Espacio para separar la UI personalizada

        // **Paso 2: Sincronizar y dibujar el selector de enums personalizado**
        SetText targetScript = (SetText)target;
        string savedEnumName = targetScript.SelectedTextEnumName;
        int savedEnumValue = targetScript.SelectedTextEnumValue;

        string ItemName = targetScript.languageManager.name.Replace(" ", "_");
        ItemName = ItemName.Replace("-", "");
        ItemName = ItemName.ToUpper();
        ItemName = EnumCreator.EliminarCaracteresDuplicadosConsecutivos(ItemName, '_');

        Debug.Log("LANGUAGUE_TEXT_" + ItemName);

        Type selectedEnumType = typeof(SetText).Assembly.GetType("LANGUAGUE_TEXT_"+ ItemName);

        Debug.Log("Name:" +selectedEnumType.Name);

        if (selectedEnumType != null)
        {
            string[] audioValues = Enum.GetNames(selectedEnumType);
            if (savedEnumValue >= audioValues.Length)
            {
                savedEnumValue = 0;
            }
            int newEnumValueIndex = EditorGUILayout.Popup("Text Value", savedEnumValue, audioValues);

            if (newEnumValueIndex != savedEnumValue)
            {
                targetScript.SelectedTextEnumValue = newEnumValueIndex;
                EditorUtility.SetDirty(targetScript);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No se encontraron enums en la carpeta de Audios.", MessageType.Warning);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}