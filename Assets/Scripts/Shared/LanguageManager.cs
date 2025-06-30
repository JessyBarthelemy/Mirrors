using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }
    private Dictionary<string, string> translations;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        translations = new Dictionary<string, string>();
        LoadLangage();
    }

    void LoadLangage()
    {
        string locale = "en";
        if (Application.systemLanguage == SystemLanguage.French)
            locale = "fr";
            
        TextAsset textAsset = Resources.Load<TextAsset>("Langage/" + locale);
        string[] lines = textAsset.text.Split('\n');
        int index;
        for (int i = 0; i < lines.Length; i++)
        {
            index = lines[i].IndexOf(':');
            translations.Add(lines[i].Substring(0, index), lines[i].Substring(index+1));
        }
    }

    public string GetTranslation(string id)
    {
        return translations[id];
    }
}
