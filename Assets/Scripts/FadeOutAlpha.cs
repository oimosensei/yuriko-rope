using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FadeOutAlpha : MonoBehaviour
{
    public float startDuration = 3f;
    public float fadeDuration = 1f;
    public bool Automated = true;
    float red, green, blue, alfa;
    bool fadeStarted = false;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alfa = GetComponent<Image>().color.a;
    }

    private void OnEnable()
    {
        if (Automated) startFade();
    }

    public void startFade()
    {
        //Invoke("SetBool", startDuration);
        DOVirtual.DelayedCall(startDuration, () => fadeStarted = true);
    }
    void Update()
    {
        if (fadeStarted)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += Time.deltaTime / fadeDuration;
            if (alfa >= 255) fadeStarted = !fadeStarted;
            if (alfa < 0)
            {
                Destroy(transform.parent.gameObject);
                GameObject.Find("StageCtrl").GetComponent<StageCtrl>().pause = false;
            }
        }
    }

}