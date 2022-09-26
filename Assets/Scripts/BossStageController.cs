using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageController : MonoBehaviour
{
    public Boss boss;

    private CameraController cc;
    private StageCtrl sc;
    private void Start()
    {
        cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        sc = GameObject.Find("StageCtrl").GetComponent<StageCtrl>();
    }
    void Update()
    {
        if (boss == null) cc.InfiniteScroll = false;
    }
}
