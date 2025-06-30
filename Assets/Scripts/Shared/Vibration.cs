using UnityEngine;
using System.Collections;

public static class Vibration
{
    private static readonly AndroidJavaObject Vibrator;

    static Vibration()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (Application.isEditor) Handheld.Vibrate();

        Vibrator = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity")
            .Call<AndroidJavaObject>("getSystemService", "vibrator");
        #endif
    }

    public static void Vibrate(long milliseconds)
    {
        if(Vibrator != null)
            Vibrator.Call("vibrate", milliseconds);
    }
}