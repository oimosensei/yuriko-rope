using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class StartMenuConrtoller : MonoBehaviour
{
    private bool firstPush = true;
    public FadeOutAlpha BlackOut;
    public float ReloadSceneDurationAfterBlackOut = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && firstPush)
        {
            DOVirtual.DelayedCall(BlackOut.startDuration + BlackOut.fadeDuration + ReloadSceneDurationAfterBlackOut, () => SceneManager.LoadScene("SampleBossStage"));
            BlackOut.startFade();
            firstPush = false;
        }
    }
}
