using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectSprite : MonoBehaviour
{
    public Sprite[] sprites;

    private Sprite sp;
    void Start()
    {
        sp = GetComponent<Sprite>();
        if (!(sprites == null)) sp = sprites[Random.Range(0, sprites.Length - 1)];
    }
    
}
