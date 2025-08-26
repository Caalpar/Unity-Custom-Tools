using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
public static class EnumCreator
{
    public static void CrearEnum(string nombreEnum, GameObject[] objetos, string customPath = "EnumsObjectManager")
    {
        if (objetos == null || objetos.Length == 0)
        {
            Debug.LogWarning("El array de GameObjects está vacío.");
            return;
        }

        string nombreArchivo = nombreEnum + ".cs";
        string carpetaDestino = Path.Combine(Application.dataPath, customPath);

        if (!Directory.Exists(carpetaDestino))
            Directory.CreateDirectory(carpetaDestino);

        string path = Path.Combine(carpetaDestino, nombreArchivo);

        StringBuilder contenido = new StringBuilder();
        contenido.AppendLine("public enum " + nombreEnum);
        contenido.AppendLine("{");

        for (int i = 0; i < objetos.Length; i++)
        {
            string nombreLimpio = LimpiarNombre(objetos[i].name);
            contenido.AppendLine($"    {nombreLimpio} = {i},");
        }

        contenido.AppendLine("}");

        bool archivoExiste = File.Exists(path);
        File.WriteAllText(path, contenido.ToString());
        // AssetDatabase.Refresh();

        if (archivoExiste)
            Debug.Log("Archivo sobrescrito en: " + path);
        else
            Debug.Log("Archivo creado en: " + path);
    }


    public static void CrearEnum(string nombreEnum, string[] objetos, string customPath = "EnumsObjectManager")
    {
        if (objetos == null || objetos.Length == 0)
        {
            Debug.LogWarning("El array de GameObjects está vacío.");
            return;
        }

        string nombreArchivo = nombreEnum + ".cs";
        string carpetaDestino = Path.Combine(Application.dataPath, customPath);

        if (!Directory.Exists(carpetaDestino))
            Directory.CreateDirectory(carpetaDestino);

        string path = Path.Combine(carpetaDestino, nombreArchivo);

        StringBuilder contenido = new StringBuilder();
        contenido.AppendLine("public enum " + nombreEnum);
        contenido.AppendLine("{");

        for (int i = 0; i < objetos.Length; i++)
        {
            string nombreLimpio = LimpiarNombre(objetos[i]);
            contenido.AppendLine($"    {nombreLimpio} = {i},");
        }

        contenido.AppendLine("}");

        bool archivoExiste = File.Exists(path);
        File.WriteAllText(path, contenido.ToString());
        // AssetDatabase.Refresh();

        if (archivoExiste)
            Debug.Log("Archivo sobrescrito en: " + path);
        else
            Debug.Log("Archivo creado en: " + path);
    }

    private static string LimpiarNombre(string nombre)
    {
        string limpio = new string(nombre
            .Where(c => char.IsLetterOrDigit(c) || c == '_')
            .ToArray());

        if (string.IsNullOrEmpty(limpio) || char.IsDigit(limpio[0]))
        {
            limpio = "N_" + limpio;
        }

        return limpio;
    }

    public static string EliminarCaracteresDuplicadosConsecutivos(string input, char caracter)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var resultado = new System.Text.StringBuilder();
        bool ultimoFueCaracter = false;

        foreach (char c in input)
        {
            if (c == caracter)
            {
                if (!ultimoFueCaracter)
                {
                    resultado.Append(c);
                    ultimoFueCaracter = true;
                }
                // si fue duplicado, no lo agregamos
            }
            else
            {
                resultado.Append(c);
                ultimoFueCaracter = false;
            }
        }

        return resultado.ToString();
    }
}