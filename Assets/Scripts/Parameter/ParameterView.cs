using UnityEngine;
using UnityEngine.UI;

public class ParameterView : MonoBehaviour
{
    public Toggle sound;
    public Toggle daltonianMode;
    public Toggle vibration;
    public GameObject warningPanel;

    void Start()
    {
        sound.isOn = ParameterReader.Instance.IsSoundActivatedParam;
        daltonianMode.isOn = ParameterReader.Instance.IsDaltonianModeParam;
        vibration.isOn = ParameterReader.Instance.IsVibrationActivatedParam;

        vibration.onValueChanged.AddListener(delegate {
            ParameterReader.Instance.IsVibrationActivatedParam = vibration.isOn;
            Audio.Instance.PlaySound(Sound.Tick);
        });

        sound.onValueChanged.AddListener(delegate {
            ParameterReader.Instance.IsSoundActivatedParam = sound.isOn;
            Audio.Instance.PlaySound(Sound.Tick);
        });

        daltonianMode.onValueChanged.AddListener(delegate {
            OnDaltonianModeChanged();
        });

        
    }

    public void ValidateParam(){
        ParameterReader.Instance.IsSoundActivatedParam = sound.isOn;
        ParameterReader.Instance.IsDaltonianModeParam = daltonianMode.isOn;

        Audio.Instance.PlaySound(Sound.Button);
        gameObject.SetActive(false);
    }

    public void OnDaltonianModeChanged()
    {
        Audio.Instance.PlaySound(Sound.Tick);
        if (warningPanel != null)
            warningPanel.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void HidePanel()
    {
        Audio.Instance.PlaySound(Sound.Button);
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }
}
