using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextLocalization : MonoBehaviour
{
    public string key;
    TextMeshPro _text;
    Text _textUI;
    public enum TextType {TM, UI};
    public TextType textType;
    
    private void Start()
    {
        GetText();
    }
    public void GetText()
    {
        if (textType == TextType.TM)
        {
            _text = GetComponent<TextMeshPro>();
            _text.text = LocalizationManager.instance.GetLocalizedValue(key);
        }
        else
        {
            _textUI = GetComponent<Text>();
            _textUI.text = LocalizationManager.instance.GetLocalizedValue(key);
        }
    }

    public void AddToText(string t)
    {
        GetText();
        if (textType == TextType.TM)
        {
            _text.text += t;
        }
        else
        {
            _textUI.text += t;
        }
    }

    private void OnEnable()
    {
        LocalizationManager.instance.Addtext(this);
    }
    private void OnDisable()
    {
        LocalizationManager.instance.RemoveText(this);
    }
}
