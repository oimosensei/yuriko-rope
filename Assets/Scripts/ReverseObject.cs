using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Contains("player"))
        {
            collision.GetComponent<player>().ReverseDirection();
        }
    }
}
