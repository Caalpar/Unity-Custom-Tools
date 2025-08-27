using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SetText : MonoBehaviour
{
    int index;
    public LanguageManager languageManager;
   
    TextMeshProUGUI textUI;
    TextMeshPro text;

    public int Index { set { index = value; } get { return index; } }
    public string SelectedTextEnumName = "";
    public int SelectedTextEnumValue = 0;

    void Awake()
    {
        UpdateText();
    }

    private void OnEnable()
    {
        languageManager.UpdateText += UpdateText;
    }

    private void OnDisable()
    {
        languageManager.UpdateText -= UpdateText;
    }

    public void SetNewText(int id)
    {
        textUI = GetComponent<TextMeshProUGUI>();
        if (textUI != null)
        {
            if (languageManager != null)
            {
                textUI.text = languageManager.GetText(id);
                Debug.Log("ID: " + id);
                Debug.Log("Text: " + textUI.text);
                Debug.Log("cursom langague " + gameObject.name);
            }
            

        }
        else
        {
            Debug.Log("text ui null in index:" + id);

            text = GetComponent<TextMeshPro>();
            if (text != null)
            {
                if (languageManager != null)
                {
                    text.text = languageManager.GetText(id);
                    Debug.Log("cursom langague " + gameObject.name);
                }
              
            }
            else
            {
                Debug.Log("text null in index:" + id);
            }
        }
    }

    public void UpdateText()
    {
        SetNewText(SelectedTextEnumValue);
    }

}
