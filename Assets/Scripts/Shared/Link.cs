using UnityEngine.UI;
using UnityEngine;

public class Link : MonoBehaviour
{
    public string url; 

    void Start()
    {
        if (url == "facebook")
            url = LocalizedText.Translate("facebook");
        GetComponent<Button>().onClick.AddListener(() => OnMouseDown());
    }

    public void OnMouseDown()
    {
        Application.OpenURL(url);
    }
}
