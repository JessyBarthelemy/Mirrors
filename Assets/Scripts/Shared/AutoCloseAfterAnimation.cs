using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCloseAfterAnimation : MonoBehaviour
{
    public void CloseWinPanel(){
        gameObject.SetActive(false);
    }
}
