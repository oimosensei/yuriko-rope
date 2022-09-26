using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using KanKikuchi.AudioManager;
public class Enemy : MonoBehaviour
{
    public bool CanBeDeleted;
    public bool DashDamaged = false;
    public bool shotDamaged = true;
    public GameObject DeathEffect;
    public bool isMITSU = false;

    [HideInInspector]
    public bool isRendered = true;

    private bool nextRendered = true;
    private float deleteTimer = 0f;
    private bool isDead;


    private void Start()
    {
        //transform.localScale = new Vector3(0, 0, 0);
        //transform.DOScale(new Vector3(0, 0, 1), 0.3f).From().SetEase(Ease.InOutBack);
        //transform.DORotate(new Vector3(0, 0, 360), 0.3f,RotateMode.FastBeyond360).SetEase(Ease.InOutBack);
    }
    private void Update()
    {
        if (CanBeDeleted)
        {
            //if (!isRendered)
            //{
            //    deleteTimer += Time.deltaTime;
            //}
            //if (deleteTimer > 10f) Destroy(gameObject);//１０秒画面に映らなかったらDestroy
        }
        isRendered = nextRendered;
        nextRendered = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("player"))
        {
            //プレイヤーがダッシュ中の場合敵が死ぬ
            if (collision.GetComponent<player>().isDash && DashDamaged) Died();
            else
            {
                collision.GetComponent<player>().Dead();
            }
        }
    }

    private void OnWillRenderObject()
    {
        if (Camera.current.tag == "MainCamera")
        {
            nextRendered = true;
        }
    }

    public void Died()
    {
        if (!isMITSU)
        {
            if (DeathEffect != null) Instantiate(DeathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            SEManager.Instance.Play(SEPath.YARARE, volumeRate: 0.2f);
        }
        else
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("Died", true);
            if (Random.Range(0, 2) == 0) SEManager.Instance.Play(SEPath.MITSUDESU, volumeRate: 0.7f);
            else SEManager.Instance.Play(SEPath.SOCIAL_DISTANCE, volumeRate: 0.3f);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
