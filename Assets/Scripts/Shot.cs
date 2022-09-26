using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public float ShotSpeed = 3f;
    [HideInInspector] public bool goRight = true;

    private float destroyDuration = 5f;
    private float time = 0f;
    void Start()
    {
        ShotSpeed *= 0.01f;
    }

    private void FixedUpdate()
    {
        transform.position += setDirection() * ShotSpeed;
        time += Time.fixedDeltaTime;
        if (time > destroyDuration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss" || collision.gameObject.layer == LayerMask.NameToLayer("Static Environment") || collision.gameObject.layer == LayerMask.NameToLayer("Nonstickable Environment"))
        {
            if (collision.tag == "Enemy") if (collision.GetComponent<Enemy>().shotDamaged) collision.GetComponent<Enemy>().Died();
            if (collision.tag == "Boss") collision.transform.parent.GetComponent<Boss>().Damaged();
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.layer == LayerMask.NameToLayer("Static Environment") || collision.gameObject.layer == LayerMask.NameToLayer("Nonstickable Environment"))
        {
            if (collision.gameObject.tag == "Enemy") Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    private Vector3 setDirection()
    {
        return goRight ? Vector3.right : Vector3.left;
    }




}
