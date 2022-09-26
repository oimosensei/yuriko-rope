using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RetryButton : MonoBehaviour
{
    public FadeOutAlpha BlackOut;
    public float ReloadSceneDurationAfterBlackOut = 0.1f;


    public void ButtonClicked()
    {
        //Invoke("ReloadScene", BlackOut.fadeSpeed+ReloadSceneDurationAfterBlackOut);
        DOVirtual.DelayedCall(BlackOut.startDuration + BlackOut.fadeDuration + ReloadSceneDurationAfterBlackOut, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        BlackOut.startFade();
    }

}
