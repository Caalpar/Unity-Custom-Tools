using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetConditionalTest : MonoBehaviour
{
    [SerializeField] int indexCondition1;
    [SerializeField] int indexCondition2;
    

    TextMeshProUGUI text;

    void Start()
    {
        /*
        text = GetComponent<TextMeshProUGUI>();

        if (ScenseManagerPrefab.instance.globalSettings.test)
            text.text = LanguageManagerPrefab.instance.languageManager.GetText(indexCondition1);
        else
            text.text = LanguageManagerPrefab.instance.languageManager.GetText(indexCondition2);
        
        */
    }
}
