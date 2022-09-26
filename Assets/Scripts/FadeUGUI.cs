using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeUGUI : MonoBehaviour
{
    [Header("拡大縮小のアニメーションカーブ")] public AnimationCurve curve;
    private bool comp = false;
    private float timer;
    private Vector3 defaultScale;
    private void Start()
    {
        defaultScale = transform.localScale;
    }

    private void Update()
    {
        if (!comp)
        {
            if (timer < 1.0f)
            {
                transform.localScale = defaultScale * curve.Evaluate(timer);
                timer += Time.deltaTime;
            }
            else
            {
                transform.localScale = defaultScale;
                comp = true;
            }
        }
    }
}
