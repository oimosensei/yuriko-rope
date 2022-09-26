using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ClickToRetry : MonoBehaviour
{
    public FadeOutAlpha BlackOut;
    public float ReloadSceneDurationAfterBlackOut = 0.1f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DOVirtual.DelayedCall(BlackOut.startDuration + BlackOut.fadeDuration + ReloadSceneDurationAfterBlackOut, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
            BlackOut.startFade();
        }
    }

}
