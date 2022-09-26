using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class deathEffect : MonoBehaviour
{
    public float DeleteDuration = 1f;
    public float DeleteAnimationTime = 0.5f;
    private void Start()
    {
        transform.DOScale(new Vector3(0, 0, 0), DeleteAnimationTime).SetDelay(DeleteDuration);
        transform.DORotate(new Vector3(0, 0, 360), DeleteAnimationTime, RotateMode.FastBeyond360).SetDelay(DeleteDuration).OnComplete(() => { Destroy(gameObject);});
    }
}
