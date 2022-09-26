using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KanKikuchi.AudioManager;
public class player : MonoBehaviour
{
    public float gravity = 5f;
    public AnimationCurve dashCurve;
    public GameObject ShotPrefab;
    public linerender line;
    public float dashDuration = 0.3f; //ダッシュにかかる時間
    public float dashDistance = 3f;　　//ダッシュ距離　 
    public bool SuperArmor = false;

    [HideInInspector] public Vector2 startPos { get; set; }
    [HideInInspector] public bool isDash;
    [HideInInspector] public Vector2 velocity = new Vector2(16, 0);


    private bool directionRight = true;
    private float dashTime = 0f;　　　//ダッシュ経過時間　
    [HideInInspector] public float dashCooldownDuration = 1f;　　//ダッシュのクールダウンにかかる時間
    [HideInInspector] public float dashCooldownTime = 0f;　　　　//ダッシュのクールダウンの経過時間
    private Vector3 dashStartPos = Vector3.zero;
    private float dashStartVelocity;
    [HideInInspector] public float shotCooldownTime = 0f;
    [HideInInspector] public float shotCooldownDuration = 0.3f;
    [HideInInspector] public bool isConnected = false;  //ロープが天井にくっついているか
    [HideInInspector] public float connectedCooldownDuration = 0.5f;
    [HideInInspector] public float connectedCooldownTime = 0f;
    private bool rotateDirection;
    private float angle = 0f;
    private float radius = 0f;

    private GameObject cameraContoroller;
    private GameObject dashEffect;
    private GameObject yurikoHand;
    private float handRotationOffset = 0;

    [HideInInspector] public StageCtrl sc;

    private void Start()
    {
        gravity *= Mathf.Pow(10, -2);
        velocity *= Mathf.Pow(10, -2);
        line.gameObject.SetActive(false);
        TimerInitialize();
        sc = GameObject.Find("StageCtrl").GetComponent<StageCtrl>();
        cameraContoroller = GameObject.Find("Main Camera");
        yurikoHand = transform.GetChild(0).gameObject;
        handRotationOffset = transform.rotation.eulerAngles.z; ;
        yurikoHand.transform.Rotate(0, 0, 180);
        dashEffect = transform.Find("dashEffect").gameObject;
        dashEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!sc.pause)
        {
            UpdateTimer();
            if (Input.GetKey(KeyCode.K))
            {
                if (dashCooldownTime < 0 && !isDash) //ダッシュスタート
                {
                    isDash = true;
                    dashStartPos = transform.position;
                    dashTime = dashDuration;
                    if (isConnected)//接続中だった場合の終了処理
                    {
                        isConnected = false;
                        connectedCooldownTime = connectedCooldownDuration;
                        line.gameObject.SetActive(false);
                        yurikoHand.transform.rotation = Quaternion.Euler(0, 0, handRotationOffset + 180);
                    }
                    dashEffect.SetActive(true);//ダッシュエフェクトの開始
                    SEManager.Instance.Play(SEPath.DASH_SOUND);
                }
            }
            if (isDash)//ダッシュ中の処理
            {


                velocity = new Vector2(setDirection() * (dashCurve.Evaluate((dashDuration - dashTime + Time.fixedDeltaTime) / dashDuration) - dashCurve.Evaluate((dashDuration - dashTime))), 0);
                if (dashTime < 0) //ダッシュ終了処理
                {
                    isDash = false;
                    dashCooldownTime = dashCooldownDuration;
                    dashTime = 0;
                    velocity = new Vector2(setDirection() * 0.14f, 0);
                    dashEffect.SetActive(false);//ダッシュエフェクトの終了
                }
                transform.position += (Vector3)velocity;
                //    transform.position = new Vector3(
                //        setDirection() * dashDistance * dashCurve.Evaluate((dashDuration - dashTime) / dashDuration) + dashStartPos.x,
                //        dashStartPos.y,
                //        transform.position.z) ;
                //
            }
            else
            {
                if (Input.GetKey(KeyCode.F) && shotCooldownTime < 0f) //弾の発射処理
                {
                    Vector3 shotOffset = new Vector3(0, -0.1f, 0);
                    GameObject shot = Instantiate(ShotPrefab, transform.position + GetComponent<BoxCollider2D>().bounds.extents.x * Vector3.right, Quaternion.identity);
                    shot.GetComponent<Shot>().goRight = directionRight;
                    shotCooldownTime = shotCooldownDuration;
                    SEManager.Instance.Play(SEPath.SHOT_SOUND, volumeRate: 0.2f);
                }

                if (Input.GetKey(KeyCode.J) && connectedCooldownTime < 0 && (IsStickable() || isConnected))  //接続中の処理
                {
                    if (!isConnected) //初回
                    {
                        isConnected = true;
                        startPos = new Vector2(transform.position.x, GetCeilingY());
                        velocity = new Vector2(velocity.x, velocity.y * 0.8f);//下方向（重力）の速度は0.8倍
                        angle = 0f;
                        radius = Mathf.Abs(transform.position.y - startPos.y);
                        rotateDirection = directionRight;
                        line.gameObject.SetActive(true);
                        SEManager.Instance.Play(SEPath.ROPE_SOUND2, volumeRate: 0.7f);
                    }
                    float distance = velocity.magnitude;
                    angle += setRotateDirection() * distance / radius;
                    transform.position = new Vector3(startPos.x + Mathf.Sin(angle) * radius, startPos.y - Mathf.Cos(angle) * radius);
                    velocity = new Vector2(velocity.magnitude * Mathf.Cos(angle) * setRotateDirection(), velocity.magnitude * Mathf.Sin(angle) * setRotateDirection());
                    //手の回転を指示
                    float dx = yurikoHand.transform.position.x - startPos.x;
                    float dy = yurikoHand.transform.position.y - startPos.y;
                    float rad = Mathf.Atan2(dy, dx);
                    yurikoHand.transform.rotation = Quaternion.Euler(0, 0, rad * Mathf.Rad2Deg + handRotationOffset + 90);

                    //Debug.Log(velocity);
                    if ((directionRight && velocity.x < 0) || (!directionRight && velocity.x >= 0))
                    {
                        ReverseDirection();
                        velocity.x *= -1;//reverseDirectionで逆転してしまったvelocityをもとに戻す
                        rotateDirection = !rotateDirection;
                    }
                }
                else
                {
                    if (isConnected && connectedCooldownTime < 0) //終了処理
                    {
                        isConnected = false;
                        connectedCooldownTime = connectedCooldownDuration;
                        line.gameObject.SetActive(false);
                        yurikoHand.transform.rotation = Quaternion.Euler(0, 0, handRotationOffset + 180);
                    }
                    velocity.y -= gravity * Time.fixedDeltaTime;
                    transform.position += (Vector3)velocity;
                }
            }
        }
    }

    private int setDirection()
    {
        return directionRight ? 1 : -1;
    }
    private int setRotateDirection()
    {
        return rotateDirection ? 1 : -1;
    }

    public void ReverseDirection()//向きの反転は原則ここで
    {
        directionRight = !directionRight;
        rotateDirection = !rotateDirection;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        yurikoHand.transform.localScale = new Vector3(-yurikoHand.transform.localScale.x, yurikoHand.transform.localScale.y, yurikoHand.transform.localScale.z);//手は反転させたくないので戻す
        velocity.x = -velocity.x;
        cameraContoroller.GetComponent<CameraController>().offsetReverse = true;
    }

    private void UpdateTimer()
    {
        dashTime -= Time.fixedDeltaTime;
        dashCooldownTime -= Time.fixedDeltaTime;
        connectedCooldownTime -= Time.fixedDeltaTime;
        shotCooldownTime -= Time.fixedDeltaTime;
    }

    private void TimerInitialize()
    {
        dashTime = dashDuration;
        dashCooldownTime = 0f;
        connectedCooldownTime = 0f;
    }

    private float GetCeilingY()//天井の高さ判定
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y, 0), Vector2.up, 80f, LayerMask.GetMask(new string[] { "Static Environment" }));
        if (hit.collider)
        {
            //Debug.DrawRay(transform.position + new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y), Vector2.up * hit.distance, Color.blue, 3);
            return hit.distance + transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y;
        }
        return 5f;
    }

    private bool IsStickable()//天井にロープをくっつけることができるか判定する
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, GetComponent<BoxCollider2D>().bounds.extents.y, 0), Vector2.up, 80f, LayerMask.GetMask(new string[] { "Static Environment", "Nonstickable Environment" }));
        //if (SuperArmor) return hit.collider.gameObject.layer == LayerMask.NameToLayer("Static Environment") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Nonstickable Environment");
        return hit.collider.gameObject.layer == LayerMask.NameToLayer("Static Environment");
    }


    public void Dead()//死んだとき
    {
        Debug.Log("you died!");
        if (!SuperArmor) sc.RetryGame();
        SEManager.Instance.Play(SEPath.YARARE, volumeRate: 0.2f);
    }


}
