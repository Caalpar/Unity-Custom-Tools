using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Indica que este es un editor personalizado para la clase 'Language'
[CustomEditor(typeof(Language))]
public class LanguageEditor : Editor
{
    // Campo para arrastrar el archivo de texto/CSV
    public TextAsset csvFile;

    public override void OnInspectorGUI()
    {
        // Dibuja el inspector por defecto
        DrawDefaultInspector();

        // Agrega un espacio
        EditorGUILayout.Space(20);

        // Agrega un campo para arrastrar el archivo CSV
        EditorGUILayout.LabelField("Importar desde CSV", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("Archivo CSV", csvFile, typeof(TextAsset), false);

        // Agrega un botón para importar los datos
        if (GUILayout.Button("Importar CSV (Añadir)"))
        {
            if (csvFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Por favor, arrastre un archivo CSV al campo de arriba.", "OK");
                return;
            }

            // Llama al método de importación
            ImportCSV(csvFile);
        }
    }

    private void ImportCSV(TextAsset file)
    {
        Language languageSO = (Language)target;

        // Obtiene el contenido del archivo CSV.
        string fileContent = file.text;

        // Divide el contenido en líneas.
        string[] lines = fileContent.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        List<string> newWordsList = new List<string>();

        // Itera sobre cada línea y la divide por comas.
        foreach (string line in lines)
        {
            string[] wordsInLine = line.Split(',');
            // Agrega cada palabra de la línea a la lista principal.
            newWordsList.AddRange(wordsInLine);
        }

        // Combina los datos existentes con los nuevos.
        List<string> combinedWords = new List<string>(languageSO.words);
        combinedWords.AddRange(newWordsList);

        // Elimina duplicados.
        List<string> uniqueWords = combinedWords.Distinct().ToList();

        // Asigna el array combinado y único.
        languageSO.words = uniqueWords.ToArray();

        EditorUtility.SetDirty(languageSO);

        Debug.Log("¡Datos del CSV importados y añadidos con éxito!");
    }
}