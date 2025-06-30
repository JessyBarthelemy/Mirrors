using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    private AudioSource audioSource;
    public static Audio Instance;

    public AudioClip buttonSound;
    public AudioClip tickSound;
    public AudioClip wonSound;
    public AudioClip enterDotSound;
    public AudioClip exitDotSound;
    public AudioClip errorSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    public void PlaySound(Sound sound)
    {
        if (!ParameterReader.Instance.IsSoundActivatedParam)
            return;

        switch (sound)
        {
            case Sound.Tick:
                audioSource.PlayOneShot(tickSound);
                break;
            case Sound.Won:
                audioSource.PlayOneShot(wonSound);
                break;
            case Sound.EnterDot:
                audioSource.PlayOneShot(enterDotSound);
                break;
            case Sound.ExitDot:
                audioSource.PlayOneShot(exitDotSound);
                break;
            case Sound.Error:
                audioSource.PlayOneShot(errorSound);
                break;
            default:
                audioSource.PlayOneShot(buttonSound);
                break;
        }
        
    }
}
