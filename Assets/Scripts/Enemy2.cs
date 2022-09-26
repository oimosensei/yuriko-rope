using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public bool CanBeDeleted;

    public bool isRendered = true;
    private float deleteTimer = 0f;
    private bool isDead = false;

    private void Update()
    {
        //if (CanBeDeleted)
        //{
        //    if (!isRendered)
        //    {
        //        deleteTimer += Time.deltaTime;
        //    }
        //    if (deleteTimer > 10f) Destroy(gameObject);//１０秒画面に映らなかったらDestroy
        //}
        if (isDead) DeadEffect();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("player"))
        {
            if (collision.GetComponent<player>().isDash) isDead = true;
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
            isRendered = true;
        }
    }

    private void DeadEffect()
    {
        Destroy(gameObject);
    }
}