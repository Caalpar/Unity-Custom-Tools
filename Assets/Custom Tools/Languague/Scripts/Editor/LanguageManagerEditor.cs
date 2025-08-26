using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LanguageManager))]
public class LanguageManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LanguageManager manager = (LanguageManager)target;

        if (GUILayout.Button("Create the enum text"))
        {

                string ItemName = target.name.Replace(" ", "_");
                ItemName = ItemName.Replace("-", "");
                ItemName = ItemName.ToUpper();
                ItemName = EnumCreator.EliminarCaracteresDuplicadosConsecutivos(ItemName, '_');
                EnumCreator.CrearEnum("LANGUAGUE_TEXT_" + ItemName, manager.GetAllWords(), "EnumsLanguageManager/Texts/");
        }

        if (GUILayout.Button("Create the enum audios"))
        {
            string ItemName = target.name.Replace(" ", "_");
            ItemName = ItemName.Replace("-", "");
            ItemName = ItemName.ToUpper();
            ItemName = EnumCreator.EliminarCaracteresDuplicadosConsecutivos(ItemName, '_');

            AudioClip[] audios = manager.GetAllAudios();

            if (audios != null)
            {
                string[] audiosNames = new string[audios.Length];
                for (int i = 0; i < audios.Length; i++)
                {
                    audiosNames[i] = audios[i].name;
                }
                 EnumCreator.CrearEnum("LANGUAGUE_AUDIO_" + ItemName, audiosNames, "EnumsLanguageManager/Audios/"); ;
            }

        }

    }
}
