using UnityEngine;
using UnityEngine.UI;

public class BackgroundDrawer : MonoBehaviour
{
    public bool dynamicBackground;

    void Awake()
    {
        RawImage img = GetComponent<RawImage>();
        Texture2D backgroundTexture = new Texture2D(1, 2);
        
        backgroundTexture.wrapMode = TextureWrapMode.Clamp;
        backgroundTexture.filterMode = FilterMode.Bilinear;
        
        if(dynamicBackground)
        {
            switch (LevelLoader.currentLevel.level % 4)
            {
                case 1:
                    //red blue
                    backgroundTexture.SetPixels(new Color[] { new Color(0.517f, 0.435f, 0.949f), new Color(0.83f, 0.38f, 0.36f) });
                    break;
                case 2:
                    //red yellow
                    backgroundTexture.SetPixels(new Color[] { new Color(0.945f, 0.635f, 0.145f), new Color(0.83f, 0.38f, 0.36f) });
                    break;
                case 3:
                    //blue green
                    backgroundTexture.SetPixels(new Color[] { new Color(0.517f, 0.435f, 0.949f), new Color(0.258f, 0.686f, 0.607f) });
                    break;
                default:
                    //red green
                    backgroundTexture.SetPixels(new Color[] { new Color(0.258f, 0.686f, 0.278f), new Color(0.83f, 0.38f, 0.36f) });
                    break;
            }
        }
        else
        {
            //red blue
            backgroundTexture.SetPixels(new Color[] { new Color(0.517f, 0.435f, 0.949f), new Color(0.83f, 0.38f, 0.36f) });
        }

        
        backgroundTexture.Apply();
        img.texture = backgroundTexture;
    }
}
