using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Languages", menuName = "Languages/Language")]
public class Language : ScriptableObject
{
    public LanguageType LanguageType;
    public string[] words;
    public AudioClip[] audios; 
}
