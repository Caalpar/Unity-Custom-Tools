using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "Tutorial", menuName = "Tutorial/Tutorial")]
public class Tutorial : ScriptableObject
{
    public LanguageManager languageManager;
    public TutorialStep[] steps;
}
