using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterReader
{
    private static ParameterReader instance;

    public static ParameterReader Instance{
        get{
            if(instance == null)
                instance = new ParameterReader();

            return instance;
        }

        private set{
            instance = value;
        }
    }

    private bool isSoundActivatedParam;
    public bool IsSoundActivatedParam{
         get{
            return isSoundActivatedParam;
        }

        set{
            PlayerPrefs.SetInt("IsSoundActivated", value ? 1 : 0);
            isSoundActivatedParam = value;
        }
    }

    private bool isDaltonianModeParam;
    public bool IsDaltonianModeParam{
        get{
            return isDaltonianModeParam;
        }

        set{
            PlayerPrefs.SetInt("IsDaltonianMode", value ? 1 : 0);
            isDaltonianModeParam = value;
        }
    }

    private bool isVibrationActivatedParam;
    public bool IsVibrationActivatedParam
    {
        get
        {
            return isVibrationActivatedParam;
        }

        set
        {
            PlayerPrefs.SetInt("IsVibrationActivatedParam", value ? 1 : 0);
            isVibrationActivatedParam = value;
        }
    }

    public bool IsPremium { get; set; }

    private ParameterReader(){
        IsSoundActivatedParam = PlayerPrefs.GetInt("IsSoundActivated", 1) == 1 ? true : false;
        IsDaltonianModeParam = PlayerPrefs.GetInt("IsDaltonianMode", 0) == 1 ? true : false;
        IsVibrationActivatedParam = PlayerPrefs.GetInt("IsVibrationActivatedParam", 1) == 1 ? true : false;
        IsPremium = PlayerPrefs.GetInt("IsPremium", 0) == 1 ? true : false;
    }

    public void LockOrientation(bool lockOrientation)
    {
        if (!lockOrientation)
            Screen.orientation = ScreenOrientation.AutoRotation;
        else
        {
            DeviceOrientation orientation = Input.deviceOrientation;
            switch (orientation)
            {
                case DeviceOrientation.FaceUp:
                case DeviceOrientation.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    break;
                case DeviceOrientation.FaceDown:
                case DeviceOrientation.PortraitUpsideDown:
                    Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                    break;
                case DeviceOrientation.LandscapeLeft:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    break;
                case DeviceOrientation.LandscapeRight:
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    break;
            }
        }
    }
}