using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string localizedId;
    public string textParameter;

    private Text text;
   
    void Awake()
    {
        text = GetComponent<Text>();
    }

    void Start()
    {
        text.text = Translate(localizedId, textParameter);
    }

    public static string Translate(string id, string parameter = null)
    {
        if (id == null || id == string.Empty)
            return string.Empty;

        string[] ids = id.Split(',');
        string result = "";
        for (int i = 0; i < ids.Length; i++)
        {
            result += LanguageManager.Instance.GetTranslation(ids[i]);
            if (parameter != null)
                result = result.Replace("$", parameter);
            if ((i+1) < ids.Length)
                result += "\n\n";
        }
        return result;
    }
}
