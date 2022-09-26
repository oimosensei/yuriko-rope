using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public StageCtrl sc;
    //public GameObject CameraRouteObject;
    public BackgroundController BC; 
    public float offset = 0f;
    public float offsetMoveVelocity = 1.5f;

    [HideInInspector]public bool offsetReverse = false;//playerの向きが変わったらplayerから呼ばれる
    [HideInInspector]public bool following = true;
    [HideInInspector]public bool InfiniteScroll = false;

    private Vector2[] CameraRoute;
    private Vector2 teleportFROM;
    private Vector2 teleportTO;
    private int currentRouteNum;//ルートのうち左側の番号
    private GameObject CurrentRoute;
    private Tween offsetTween = null;
    private float offsetInitialValue = 0;
    private bool previousPlayerRight = true;
    void Start()
    {
        offsetInitialValue = offset;
    }

    void LateUpdate()
    {
        if (InfiniteScroll && transform.position.x > teleportFROM.x) 
        {
            Vector3 amoutOfMovement = teleportFROM - teleportTO;
            Player.transform.position -= amoutOfMovement;
            if (Player.GetComponent<player>().isConnected) Player.GetComponent<player>().startPos -= (Vector2)amoutOfMovement;
            foreach (GameObject shot in GameObject.FindGameObjectsWithTag("Shot"))
            {
                shot.transform.position -= amoutOfMovement;
            }
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")??new GameObject[1])
            {
                if (enemy.transform.parent==CurrentRoute.transform)
                {
                    if (enemy.GetComponent<Enemy>().isRendered) enemy.transform.position -= amoutOfMovement;
                    else Destroy(enemy);
                }

            }
            var boss = GameObject.Find("Boss");
            boss.transform.position -= amoutOfMovement;
            BC.TextureOffsetAdjust(amoutOfMovement);
        }
        if (following)
        {
            if (CameraRoute == null) CameraRouteInitialize(sc.RouteUpdate());
            while (Player.transform.position.x < CameraRoute[currentRouteNum].x) currentRouteNum--;
            if (currentRouteNum >= CameraRoute.Length-2) CameraRouteInitialize(sc.RouteUpdate());
            while (Player.transform.position.x >= CameraRoute[currentRouteNum + 1].x) currentRouteNum++;
            //if ((!previousPlayerRight && Player.GetComponent<player>().velocity.x > 0) || (previousPlayerRight && Player.GetComponent<player>().velocity.x <= 0))
            if(offsetReverse)
            {
                offsetTween.Kill();
                offsetTween = null;
                offsetTween = DOTween.To(() => offset, (n) => offset = n, offsetInitialValue * ((Player.GetComponent<player>().velocity.x) > 0 ? 1 : -1), (Mathf.Abs(((offsetInitialValue * Player.GetComponent<player>().velocity.x > 0 ? 1 : -1) - offset)) / offsetMoveVelocity));
                previousPlayerRight = !previousPlayerRight;//向きのフラグを反転
                offsetReverse = false;
                //Debug.Log("cameraReverse");
            }
            transform.position = new Vector3(
                Player.transform.position.x + offset, 
                Player.transform.position.y,
                //(CameraRoute[currentRouteNum + 1].y - CameraRoute[currentRouteNum].y) / (CameraRoute[currentRouteNum + 1].x - CameraRoute[currentRouteNum].x) * (Player.transform.position.x-CameraRoute[currentRouteNum].x) + CameraRoute[currentRouteNum].y, 
                transform.position.z);
        }
        if (Input.GetKey(KeyCode.G))
        {
            int i = 0;
            offsetTween.Kill();
        }

    }
    private void CameraRouteInitialize(GameObject CameraRouteObject)
    {
        CameraRoute = new Vector2[CameraRouteObject.transform.childCount + 1];
        int i = 0;
        foreach (Transform child in CameraRouteObject.transform)
        {
            CameraRoute[i] = child.transform.position;
            i++;
        }
        CameraRoute[i] = new Vector2(CameraRoute[i - 1].x + 500f, CameraRoute[i - 1].y);//万が一ゴールを超えてもバグらないようにその他バグ避け実際使わない
        currentRouteNum = 0;
        CurrentRoute = CameraRouteObject.transform.parent.gameObject;
    }

    public void InfiniteScrollInitialize(GameObject TeleportObject)
    {
        InfiniteScroll = true;
        teleportFROM = TeleportObject.transform.GetChild(1).position;
        teleportTO = TeleportObject.transform.GetChild(0).position;
    }


}
