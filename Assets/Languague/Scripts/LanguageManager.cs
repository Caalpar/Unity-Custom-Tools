using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguageManager", menuName = "Languages/LanguageManager")]
public class LanguageManager : ScriptableObject
{
    [SerializeField] Language[] _languagesStrings;
    [SerializeField] LanguageType _languageType;

    public delegate void UpdateTextDelegate();
    public UpdateTextDelegate UpdateText = null;


    public string GetText(int indexWord)
    {
        if (_languagesStrings.Length == 0) return "Language not found";

       int indexLanguage = -1;

        for (int i = 0; i < _languagesStrings.Length; i++)
        {
            if(_languagesStrings[i].LanguageType == _languageType)
            {
                indexLanguage = i;
                break;
            }
        }

        if(indexLanguage == -1) return "Language not found";

        int lenWords = _languagesStrings[indexLanguage].words.Length;

        if (lenWords < indexWord) return "Word not found at language " + _languageType.ToString();
        if (lenWords == 0) return "Word not found at language " + _languageType.ToString();

        return _languagesStrings[indexLanguage].words[indexWord];
    }

    public AudioClip GetAudio(int indexAudio)
    {
        if (_languagesStrings.Length == 0) return null;

        int indexLanguage = -1;

        for (int i = 0; i < _languagesStrings.Length; i++)
        {
            if (_languagesStrings[i].LanguageType == _languageType)
            {
                indexLanguage = i;
                break;
            }
        }

        if (indexLanguage == -1) return null;

        int lenAudios = _languagesStrings[indexLanguage].audios.Length;

        if (lenAudios < indexAudio) return null;
        if (lenAudios == 0) return null;

        return _languagesStrings[indexLanguage].audios[indexAudio];
    }

    public void SetLanguage(int languageType)
    {

        _languageType = (LanguageType)languageType;

        if (UpdateText != null)
        {
            UpdateText.Invoke();
        }

    }

}
