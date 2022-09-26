using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;
using DG.Tweening;
public class StageCtrl : MonoBehaviour
{
    public CameraController CC;
    public GameObject ClearGUIs;
    public GameObject RetryGUIs;
    public bool pause = false;
    public GameObject[] RoutePrefab;
    public int[] RouteIndex;//0からスタート
    public GameObject GoalPrefab;

    private GameObject currentRoute;
    private GameObject previousRoute;
    private GameObject nextRoute;
    private int routeCounter = 0;//0からスタート(nextRouteを生成するところから始まるため）
    private int currentRouteNum = 0;
    private bool isWaitingForRetry = false;

    private void Start()
    {
        //null対策
        currentRoute = new GameObject();
        previousRoute = new GameObject();
        routeCounter = GManager.instance.currentStageNum-1;
        nextRoute = Instantiate(RoutePrefab[FixRouteNum()], Vector3.zero, Quaternion.identity);
        Vector2 startPos = nextRoute.transform.GetChild(0).GetChild(0).transform.position;
        GameObject.Find("player").transform.position = (Vector3)startPos + new Vector3(1, 1, 0);
    }

    private void Update()
    {
        if (isWaitingForRetry)
        {
            if (Input.GetKey(KeyCode.D))
            {
                GManager.instance.currentStageNum = routeCounter;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    public void StageClear()
    {
        CC.following = false;
        ClearGUIs.gameObject.SetActive(true);
        pause = true;
        DOVirtual.DelayedCall(7f, () => SceneManager.LoadScene(0));
        SEManager.Instance.Play(SEPath.CLEAR, volumeRate: 0.5f);
        BGMManager.Instance.Stop();
    }

    public void RetryGame()
    {
        RetryGUIs.gameObject.SetActive(true);
        pause = true;
        isWaitingForRetry = true;
    }

    public GameObject RouteUpdate() //CameraControllerに呼び出される
    {
        routeCounter++;
        if (!(previousRoute == null)) Destroy(previousRoute);
        previousRoute = currentRoute;
        currentRoute = nextRoute;
        Vector3 currentRouteEndPosition = currentRoute.transform.GetChild(0).GetChild(currentRoute.transform.GetChild(0).transform.childCount - 1).transform.position;//一番最後のとこの座標取る
        if (currentRoute.gameObject.name.Contains("Boss")) CC.InfiniteScrollInitialize(currentRoute.transform.GetChild(1).gameObject);
        else CC.InfiniteScroll = false;//InfiniteScrollするかの判定後で変える
        if (!(routeCounter == RouteIndex.Length)) nextRoute = Instantiate(RoutePrefab[FixRouteNum()], currentRouteEndPosition - RoutePrefab[FixRouteNum()].transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity);
        else Instantiate(GoalPrefab, currentRouteEndPosition, Quaternion.identity);//最後だったらゴールの描写
        return currentRoute.transform.GetChild(0).gameObject;
    }

    private int FixRouteNum()//ここの仕組みは後々考える
    {
        return RouteIndex[routeCounter + 1 - 1];
    }
}
//public class StageCtrl : MonoBehaviour
//{
//    public CameraController CC;
//    public GameObject ClearGUIs;
//    public GameObject RetryGUIs;
//    public bool pause = false;
//    public GameObject[] RoutePrefab;
//    public int[] RouteIndex;//0からスタート
//    public GameObject GoalPrefab;

//    private GameObject currentRoute;
//    private GameObject previousRoute;
//    private GameObject nextRoute;
//    private int routeCounter = 1;//1からスタート
//    private int currentRouteNum = 0;
//    private bool isWaitingForRetry = false;

//    private void Start()
//    {
//        //null対策
//        currentRoute = new GameObject();
//        previousRoute = new GameObject();
//        routeCounter++;
//        nextRoute = Instantiate(RoutePrefab[FixRouteNum()], Vector3.zero, Quaternion.identity);
//        Vector2 startPos = nextRoute.transform.GetChild(0).GetChild(0).transform.position;
//        GameObject.Find("player").transform.position = (Vector3)startPos + new Vector3(1, 1, 0);
//    }

//    private void Update()
//    {
//        if (isWaitingForRetry)
//        {
//            if (Input.GetKey(KeyCode.K)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//        }
//    }
//    public void StageClear()
//    {
//        CC.following = false;
//        ClearGUIs.gameObject.SetActive(true);
//        pause = true;
//    }

//    public void RetryGame()
//    {
//        RetryGUIs.gameObject.SetActive(true);
//        pause = true;
//        isWaitingForRetry = true;
//    }

//    public GameObject RouteUpdate() //CameraControllerに呼び出される
//    {
//        routeCounter++;
//        if (!(previousRoute == null)) Destroy(previousRoute);
//        previousRoute = currentRoute;
//        currentRoute = nextRoute;
//        Vector3 currentRouteEndPosition = currentRoute.transform.GetChild(0).GetChild(currentRoute.transform.GetChild(0).transform.childCount - 1).transform.position;//一番最後のとこの座標取る
//        if (currentRoute.gameObject.name.Contains("Boss")) CC.InfiniteScrollInitialize(currentRoute.transform.GetChild(1).gameObject);
//        else CC.InfiniteScroll = false;//InfiniteScrollするかの判定後で変える
//        if (!(routeCounter == RouteIndex.Length + 2)) nextRoute = Instantiate(RoutePrefab[FixRouteNum()], currentRouteEndPosition - RoutePrefab[FixRouteNum()].transform.GetChild(0).GetChild(0).transform.position, Quaternion.identity);
//        else Instantiate(GoalPrefab, currentRouteEndPosition, Quaternion.identity);//最後だったらゴールの描写
//        return currentRoute.transform.GetChild(0).gameObject;
//    }

//    private int FixRouteNum()//ここの仕組みは後々考える
//    {
//        return RouteIndex[routeCounter - 2];
//    }
//}
