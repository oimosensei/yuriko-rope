using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<player>()) collision.GetComponent<player>().sc.StageClear();
    }

    private void Start()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position , Vector2.up, 80f, LayerMask.GetMask(new string[] { "Static Environment" }));
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 80f, LayerMask.GetMask(new string[] { "Static Environment" }));
        if (hitUp.collider && hitDown)
        {
            transform.position = new Vector3(0, (hitUp.distance - hitDown.distance) / 2, 0) + transform.position;
            GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, hitUp.distance + hitDown.distance);
            GetComponent<SpriteRenderer>().size = new Vector2(GetComponent<SpriteRenderer>().size.x, hitUp.distance + hitDown.distance);
        }
    }
}
